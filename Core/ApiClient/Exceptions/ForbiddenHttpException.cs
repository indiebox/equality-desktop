using System;
using System.Net.Http;

namespace Equality.Core.ApiClient.Exceptions
{
    public class ForbiddenHttpException : HttpRequestException
    {
        public ForbiddenHttpException() : base("This action is unauthorized.")
        {
        }

        public ForbiddenHttpException(string message) : base(message)
        {
        }

        public ForbiddenHttpException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
