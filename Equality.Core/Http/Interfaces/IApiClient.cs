using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Equality.Http
{
    public interface IApiClient
    {
        /// <summary>
        /// Set bearer token for all next requests.
        /// </summary>
        /// <param name="token">The api token.</param>
        /// <returns>Returns <see langword="this"/></returns>
        public ApiClient WithToken(string token);

        /// <summary>
        /// Set bearer token only for one next request.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ApiClient WithTokenOnce(string token);

        /// <summary>
        /// Remove bearer token from all next requests.
        /// </summary>
        /// <returns>Returns <see langword="this"/></returns>
        public ApiClient WithoutToken();

        /// <summary>
        /// Gets or sets the HttpClient which perform requests.
        /// </summary>
        public HttpClient HttpClient { get; set; }

        /// <inheritdoc cref="GetAsync(Uri)"/>
        public Task<ApiResponseMessage> GetAsync(string requestUri);

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// 
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// 
        /// <inheritdoc cref="DeleteAsync(Uri)"/>
        public Task<ApiResponseMessage> GetAsync(Uri requestUri);

        /// <inheritdoc cref="PostAsync(Uri, Dictionary{string, object})"/>
        public Task<ApiResponseMessage> PostAsync(string requestUri);

        /// <inheritdoc cref="PostAsync(Uri, Dictionary{string, object})"/>
        public Task<ApiResponseMessage> PostAsync(Uri requestUri);

        /// <inheritdoc cref="PostAsync(Uri, Dictionary{string, object})"></inheritdoc>
        public Task<ApiResponseMessage> PostAsync(string requestUri, Dictionary<string, object> content);

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// 
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The Dictionary of HttpContent sent to the server.</param>
        /// 
        /// <inheritdoc cref="DeleteAsync(Uri)"/>
        public Task<ApiResponseMessage> PostAsync(Uri requestUri, Dictionary<string, object> content);

        /// <inheritdoc cref="PatchAsync(Uri, Dictionary{string, object})"/>
        public Task<ApiResponseMessage> PatchAsync(string requestUri);

        /// <inheritdoc cref="PatchAsync(Uri, Dictionary{string, object})"></inheritdoc>
        public Task<ApiResponseMessage> PatchAsync(Uri requestUri);

        /// <inheritdoc cref="PatchAsync(Uri, Dictionary{string, object})"></inheritdoc>
        public Task<ApiResponseMessage> PatchAsync(string requestUri, Dictionary<string, object> content);

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// 
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The Dictionary of HttpContent sent to the server.</param>
        /// 
        /// <inheritdoc cref="DeleteAsync(Uri)"/>
        public Task<ApiResponseMessage> PatchAsync(Uri requestUri, Dictionary<string, object> content);

        /// <inheritdoc cref="PutAsync(Uri, Dictionary{string, object})"/>
        public Task<ApiResponseMessage> PutAsync(string requestUri);

        /// <inheritdoc cref="PutAsync(Uri, Dictionary{string, object})"/>
        public Task<ApiResponseMessage> PutAsync(Uri requestUri);

        /// <inheritdoc cref="PutAsync(Uri, Dictionary{string, object})"/>
        public Task<ApiResponseMessage> PutAsync(string requestUri, Dictionary<string, object> content);

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// 
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The Dictionary of HttpContent sent to the server.</param>
        /// 
        /// <inheritdoc cref="DeleteAsync(Uri)"/>
        public Task<ApiResponseMessage> PutAsync(Uri requestUri, Dictionary<string, object> content);

        /// <inheritdoc cref="DeleteAsync(Uri)"/>
        public Task<ApiResponseMessage> DeleteAsync(string requestUri);

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// 
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// 
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// 
        /// <exception cref="System.ArgumentNullException">The requestUri is null.</exception>
        /// <exception cref="System.Net.Http.HttpRequestException">
        /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
        /// </exception>
        /// <exception cref="NotFoundHttpException"></exception>
        /// <exception cref="ForbiddenHttpException"></exception>
        /// <exception cref="TooManyRequestsHttpException"></exception>
        /// <exception cref="UnauthorizedHttpException"></exception>
        /// <exception cref="UnprocessableEntityHttpException"></exception>
        public Task<ApiResponseMessage> DeleteAsync(Uri requestUri);

        /// <summary>
        /// Build the Uri to the API with query parameters.
        /// </summary>
        /// 
        /// <param name="requestUri">Uri.</param>
        /// <param name="query">Uri query parameters.</param>
        /// 
        /// <returns>Uri to the api with parsed query parameters.</returns>
        /// 
        /// <example>
        /// For example:
        /// <code>
        /// // Lines below return URI with path: path.to.api/login?filter=accepted
        /// var uri = ApiClient.BuildUri("login", new() { {"filter", "accepted" } });
        /// </code>
        /// </example>
        public Uri BuildUri(string requestUri, Dictionary<string, string> query);
    }
}
