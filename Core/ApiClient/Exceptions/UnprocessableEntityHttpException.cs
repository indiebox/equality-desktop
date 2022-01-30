using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Equality.Core.ApiClient.Exceptions
{
    public class UnprocessableEntityHttpException : HttpRequestException
    {
        public Dictionary<string, string[]> Errors { get; protected set; }

        public UnprocessableEntityHttpException() : base("The given data was invalid.")
        {
        }

        public UnprocessableEntityHttpException(Dictionary<string, string[]> errors, string message) : base(message)
        {
            Errors = errors;
        }

        public UnprocessableEntityHttpException(Dictionary<string, string[]> errors, string message, Exception inner) : base(message, inner)
        {
            Errors = errors;
        }
    }
}
