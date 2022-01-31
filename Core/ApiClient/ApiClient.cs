using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Equality.Core.ApiClient.Exceptions;
using Equality.Core.ApiClient.Interfaces;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Equality.Core.ApiClient
{
    public class ApiClient : IApiClient
    {
        public HttpClient HttpClient { get; set; }

        public ApiClient()
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("http://equality/api/v1/")
            };
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ApiClient WithToken(string token)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return this;
        }

        public ApiClient WithoutToken()
        {
            HttpClient.DefaultRequestHeaders.Authorization = null;

            return this;
        }

        public async Task<ApiResponseMessage> GetAsync(string requestUri)
        {
            var response = await HttpClient.GetAsync(SanitizeUri(requestUri));

            return await ProcessResponseAsync(response);
        }

        public async Task<ApiResponseMessage> GetAsync(Uri requestUri)
        {
            var response = await HttpClient.GetAsync(requestUri);

            return await ProcessResponseAsync(response);
        }

        public async Task<ApiResponseMessage> PostAsync(string requestUri) => await PostAsync(requestUri, new());

        public async Task<ApiResponseMessage> PostAsync(string requestUri, Dictionary<string, object> content)
        {
            return await SendPostRequest(requestUri, content);
        }

        public async Task<ApiResponseMessage> PostAsync(Uri requestUri) => await PostAsync(requestUri, new());

        public async Task<ApiResponseMessage> PostAsync(Uri requestUri, Dictionary<string, object> content)
        {
            return await SendPostRequest(requestUri, content);
        }

        public async Task<ApiResponseMessage> PatchAsync(string requestUri) => await PatchAsync(requestUri, new());

        public async Task<ApiResponseMessage> PatchAsync(string requestUri, Dictionary<string, object> content)
        {
            SpoofRequestMethod(content, HttpRequestMethod.Patch);

            return await SendPostRequest(requestUri, content);
        }

        public async Task<ApiResponseMessage> PatchAsync(Uri requestUri) => await PatchAsync(requestUri, new());

        public async Task<ApiResponseMessage> PatchAsync(Uri requestUri, Dictionary<string, object> content)
        {
            SpoofRequestMethod(content, HttpRequestMethod.Patch);

            return await SendPostRequest(requestUri, content);
        }

        public async Task<ApiResponseMessage> PutAsync(string requestUri) => await PutAsync(requestUri, new());

        public async Task<ApiResponseMessage> PutAsync(string requestUri, Dictionary<string, object> content)
        {
            SpoofRequestMethod(content, HttpRequestMethod.Put);

            return await SendPostRequest(requestUri, content);
        }

        public async Task<ApiResponseMessage> PutAsync(Uri requestUri) => await PutAsync(requestUri, new());

        public async Task<ApiResponseMessage> PutAsync(Uri requestUri, Dictionary<string, object> content)
        {
            SpoofRequestMethod(content, HttpRequestMethod.Put);

            return await SendPostRequest(requestUri, content);
        }

        public async Task<ApiResponseMessage> DeleteAsync(string requestUri)
        {
            var response = await HttpClient.DeleteAsync(SanitizeUri(requestUri));

            return await ProcessResponseAsync(response);
        }

        public async Task<ApiResponseMessage> DeleteAsync(Uri requestUri)
        {
            var response = await HttpClient.DeleteAsync(requestUri);

            return await ProcessResponseAsync(response);
        }

        protected async Task<ApiResponseMessage> SendPostRequest(string requestUri, Dictionary<string, object> content)
        {
            var response = await HttpClient.PostAsync(SanitizeUri(requestUri), PrepareContent(content));

            return await ProcessResponseAsync(response);
        }

        protected async Task<ApiResponseMessage> SendPostRequest(Uri requestUri, Dictionary<string, object> content)
        {
            var response = await HttpClient.PostAsync(requestUri, PrepareContent(content));

            return await ProcessResponseAsync(response);
        }

        /// <summary>
        /// Spoof the request method to the specified.
        /// </summary>
        /// 
        /// <param name="content">The Dictionary of HttpContent sent to the server.</param>
        /// <param name="method">Request method.</param>
        /// <remarks>
        /// About method spoofing:
        /// <see href="https://stackoverflow.com/questions/50691938/patch-and-put-request-does-not-working-with-form-data">Stack Overflow thread</see>
        /// </remarks>
        protected void SpoofRequestMethod(Dictionary<string, object> content, string method)
        {
            content.Remove("_method");
            content.Add("_method", method);
        }

        /// <summary>
        /// Sanitize Uri before request.
        /// </summary>
        /// 
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// 
        /// <returns>Returns sanitized Uri ready for performing request.</returns>
        protected string SanitizeUri(string requestUri)
        {
            return requestUri.Trim(new[] { '/' });
        }

        /// <summary>
        /// Process the given response.
        /// </summary>
        /// 
        /// <param name="response">Response from request.</param>
        /// 
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected async Task<ApiResponseMessage> ProcessResponseAsync(HttpResponseMessage response)
        {
            JObject responseData = JObject.Parse(await response.Content.ReadAsStringAsync());

            HandleStatusCode(response, responseData);

            return new ApiResponseMessage(response, responseData);
        }

        /// <summary>
        /// Handle a status code of the request and throw specified exceptions.
        /// </summary>
        /// 
        /// <param name="response">Response from request.</param>
        /// <param name="responseData">Parsed response data.</param>
        /// 
        /// <exception cref="UnauthorizedHttpException"></exception>
        /// <exception cref="ForbiddenHttpException"></exception>
        /// <exception cref="UnprocessableEntityHttpException"></exception>
        /// <exception cref="NotFoundHttpException"></exception>
        /// <exception cref="TooManyRequestsHttpException"></exception>
        protected void HandleStatusCode(HttpResponseMessage response, JObject responseData)
        {
            switch (response.StatusCode) {
                case HttpStatusCode.Unauthorized: {
                    if (responseData.TryGetValue("message", out JToken message)) {
                        throw new UnauthorizedHttpException(message.ToString());
                    }

                    throw new UnauthorizedHttpException();
                }

                case HttpStatusCode.Forbidden: {
                    if (responseData.TryGetValue("message", out JToken message)) {
                        throw new ForbiddenHttpException(message.ToString());
                    }

                    throw new ForbiddenHttpException();
                }

                case HttpStatusCode.UnprocessableEntity: {
                    string message = "The given data was invalid.";
                    Dictionary<string, string[]> errors = new();

                    if (responseData.TryGetValue("errors", out JToken rawErrors)) {
                        errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(rawErrors.ToString());
                    }

                    if (responseData.TryGetValue("message", out JToken rawMessage)) {
                        message = rawMessage.ToString();
                    }

                    throw new UnprocessableEntityHttpException(message, errors);
                }

                case HttpStatusCode.NotFound: {
                    if (responseData.TryGetValue("message", out JToken message)) {
                        throw new NotFoundHttpException(message.ToString());
                    }

                    throw new NotFoundHttpException();
                }

                case HttpStatusCode.TooManyRequests: {
                    if (responseData.TryGetValue("message", out JToken message)) {
                        throw new TooManyRequestsHttpException(message.ToString());
                    }

                    throw new TooManyRequestsHttpException();
                }

                default:
                    break;
            }

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Prepare content for request.
        /// </summary>
        /// 
        /// <param name="data">The content the request will send with.</param>
        /// 
        /// <remarks>
        /// If value of the Dictionary is a <see langword="string"/>, than it will be converted to <see cref="StringContent"/>.
        /// <code></code>
        /// If value of the Dictionary is any type of <see cref="HttpContent" />, than it will remain unchanged.
        /// <code></code>
        /// If value of the Dictionary is not a <see langword="string"/> or <see cref="HttpContent" />, than <c>ArgumentException</c> will be thrown.
        /// </remarks>
        /// 
        /// <returns>Content with type of <see cref="MultipartFormDataContent"/> that ready for request.</returns>
        /// 
        /// <exception cref="ArgumentException"></exception>
        protected MultipartFormDataContent PrepareContent(Dictionary<string, object> data)
        {
            MultipartFormDataContent preparedContent = new();

            foreach (KeyValuePair<string, object> value in data) {
                HttpContent content;

                if (value.Value is string stringValue) {
                    content = new StringContent(stringValue, Encoding.UTF8);
                } else if (value.Value is HttpContent contentValue) {
                    content = contentValue;
                } else {
                    throw new ArgumentException("One of the content values of request is not supported or not valid.");
                }

                preparedContent.Add(content, value.Key);
            }

            return preparedContent;
        }
    }
}
