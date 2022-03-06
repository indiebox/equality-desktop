using System;
using System.Net.Http;

namespace Equality.Http
{
    public class NotFoundHttpException : HttpRequestException
    {
        public NotFoundHttpException()
        {
        }

        public NotFoundHttpException(string message) : base(message)
        {
        }

        public NotFoundHttpException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
