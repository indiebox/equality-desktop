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

    public class ApiResponseMessage<TObject> : ApiResponseMessage
        where TObject : class
    {
        /// <summary>
        /// Gets the parsed response object.
        /// </summary>
        /// <remarks>
        /// If API response returns some object, or collection of objects,
        /// they will be stored here.
        /// </remarks>
        public TObject Object { get; protected set; }

        public ApiResponseMessage(TObject obj, ApiResponseMessage response) : this(obj, response.Original, response.Content)
        {
        }

        public ApiResponseMessage(TObject obj, HttpResponseMessage original, JObject content) : base(original, content)
        {
            Object = obj;
        }
    }
}
