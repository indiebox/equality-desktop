using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Equality.Core.ApiClient.Exceptions
{
    public class UnauthorizedHttpException : HttpRequestException
    {
        public UnauthorizedHttpException() : base("Unauthenticated.")
        {
        }

        public UnauthorizedHttpException(string message) : base(message)
        {
        }

        public UnauthorizedHttpException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
