using System.Net;
using System.Net.Http;

using Newtonsoft.Json.Linq;

namespace Equality.Core.ApiClient
{
    public class ApiResponseMessage
    {
        public HttpResponseMessage Original { get; protected set; }

        public JObject Content { get; protected set; }

        public ApiResponseMessage(HttpResponseMessage original, JObject content)
        {
            Original = original;
            Content = content;
        }
    }
}
