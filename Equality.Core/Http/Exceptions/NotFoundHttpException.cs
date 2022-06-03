using System;

namespace Equality.Http
{
    public class NotFoundHttpException : ApiException
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

        public string Url { get; set; }
    }
}
