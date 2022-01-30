using System;
using System.Net.Http;

namespace Equality.Core.ApiClient.Exceptions
{
    public class TooManyRequestsHttpException : HttpRequestException
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
    }
}
