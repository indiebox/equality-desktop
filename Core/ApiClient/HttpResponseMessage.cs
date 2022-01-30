using System.Net.Http;

using Newtonsoft.Json.Linq;

namespace Equality.Core.ApiClient
{
    public class ApiResponseMessage
    {
        /// <summary>
        /// Get the original response was returned after request.
        /// </summary>
        public HttpResponseMessage Original { get; protected set; }

        /// <summary>
        /// Get the parsed content of response was returned after request.
        /// </summary>
        public JObject Content { get; protected set; }

        public ApiResponseMessage(HttpResponseMessage original, JObject content)
        {
            Original = original;
            Content = content;
        }
    }
}
