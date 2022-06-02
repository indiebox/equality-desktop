using System;
using System.Net.Http;

namespace Equality.Http
{
    public class ApiException : HttpRequestException
    {
        public ApiException()
        {
        }

        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
