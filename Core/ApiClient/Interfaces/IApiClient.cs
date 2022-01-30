using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equality.Core.ApiClient.Interfaces
{
    public interface IApiClient
    {
        public Task<ApiResponseMessage> GetAsync(string requestUri);

        public Task<ApiResponseMessage> GetAsync(Uri requestUri);

        public Task<ApiResponseMessage> PostAsync(string requestUri, Dictionary<string, object> content);

        public Task<ApiResponseMessage> PostAsync(Uri requestUri, Dictionary<string, object> content);

        public Task<ApiResponseMessage> PatchAsync(string requestUri, Dictionary<string, object> content);

        public Task<ApiResponseMessage> PatchAsync(Uri requestUri, Dictionary<string, object> content);

        public Task<ApiResponseMessage> PutAsync(string requestUri, Dictionary<string, object> content);

        public Task<ApiResponseMessage> PutAsync(Uri requestUri, Dictionary<string, object> content);

        public Task<ApiResponseMessage> DeleteAsync(string requestUri);

        public Task<ApiResponseMessage> DeleteAsync(Uri requestUri);
    }
}
