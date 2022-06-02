using System;

namespace Equality.Http
{
    public class TooManyRequestsHttpException : ApiException
    {
        public TooManyRequestsHttpException()
        {
        }

        public TooManyRequestsHttpException(string message) : base(message)
        {
        }

        public TooManyRequestsHttpException(string message, Exception inner) : base(message, inner)
        {
        }

        public TimeSpan? RetryAfter { get; set; }

        public int Limit { get; set; }
    }
}
