using System;
using System.Net.Http;

namespace Equality.Http
{
    public class NotFoundHttpException : HttpRequestException
    {
        public NotFoundHttpException(string url)
        {
            Url = url;
        }

        public NotFoundHttpException(string url, string message) : base(message)
        {
            Url = url;
        }

        public NotFoundHttpException(string url, string message, Exception inner) : base(message, inner)
        {
            Url = url;
        }

        public string Url { get; protected set; }
    }
}
