using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Catel;

using Equality.Core.ApiClient.Exceptions;
using Equality.Core.ApiClient.Interfaces;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Equality.Core.ApiClient
{
    internal class ApiClient : HttpClient, IApiClient
    {
        public ApiClient()
        {
            BaseAddress = new Uri("http://equality/api/v1/");
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public new async Task<ApiResponseMessage> GetAsync(string requestUri)
        {
            HttpResponseMessage response = await base.GetAsync(requestUri);

            return await ProccessResponseAsync(response);
        }

        public new async Task<ApiResponseMessage> GetAsync(Uri requestUri)
        {
            HttpResponseMessage response = await base.GetAsync(requestUri);

            return await ProccessResponseAsync(response);
        }

        public async Task<ApiResponseMessage> PostAsync(string requestUri, Dictionary<string, object> content)
        {
            Argument.IsNotNullOrWhitespace(() => requestUri);

            HttpResponseMessage response = await PostAsync(requestUri, PrepareContent(content));

            return await ProccessResponseAsync(response);
        }

        public async Task<ApiResponseMessage> PostAsync(Uri requestUri, Dictionary<string, object> content)
        {
            HttpResponseMessage response = await PostAsync(requestUri, PrepareContent(content));

            return await ProccessResponseAsync(response);
        }

        public async Task<ApiResponseMessage> PatchAsync(string requestUri, Dictionary<string, object> content)
        {
            HttpResponseMessage response = await PatchAsync(requestUri, PrepareContent(content));

            return await ProccessResponseAsync(response);
        }

        public async Task<ApiResponseMessage> PatchAsync(Uri requestUri, Dictionary<string, object> content)
        {
            HttpResponseMessage response = await PatchAsync(requestUri, PrepareContent(content));

            return await ProccessResponseAsync(response);
        }

        public async Task<ApiResponseMessage> PutAsync(string requestUri, Dictionary<string, object> content)
        {
            HttpResponseMessage response = await PutAsync(requestUri, PrepareContent(content));

            return await ProccessResponseAsync(response);
        }

        public async Task<ApiResponseMessage> PutAsync(Uri requestUri, Dictionary<string, object> content)
        {
            HttpResponseMessage response = await PutAsync(requestUri, PrepareContent(content));

            return await ProccessResponseAsync(response);
        }

        public new async Task<ApiResponseMessage> DeleteAsync(string requestUri)
        {
            HttpResponseMessage response = await base.DeleteAsync(requestUri);

            return await ProccessResponseAsync(response);
        }

        public new async Task<ApiResponseMessage> DeleteAsync(Uri requestUri)
        {
            HttpResponseMessage response = await base.DeleteAsync(requestUri);

            return await ProccessResponseAsync(response);
        }

        protected async Task<ApiResponseMessage> ProccessResponseAsync(HttpResponseMessage response)
        {
            JObject responseData = JObject.Parse(await response.Content.ReadAsStringAsync());

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
                    Dictionary<string, string[]> errors = null;
                    string message = "";

                    if (responseData.TryGetValue("errors", out JToken rawErrors)) {
                        errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(rawErrors.ToString());
                    }

                    if (responseData.TryGetValue("message", out JToken rawMessage)) {
                        message = rawMessage.ToString();
                    }

                    throw new UnprocessableEntityHttpException(errors, message);
                }
                default:
                    break;
            }

            response.EnsureSuccessStatusCode();

            return new ApiResponseMessage(response, responseData);
        }

        protected MultipartFormDataContent PrepareContent(Dictionary<string, object> data)
        {
            if (data == null) {
                return new MultipartFormDataContent();
            }

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
