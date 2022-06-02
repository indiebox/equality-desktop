using System;
using System.Collections.Generic;

namespace Equality.Http
{
    public class UnprocessableEntityHttpException : ApiException
    {
        public Dictionary<string, string[]> Errors { get; protected set; }

        public UnprocessableEntityHttpException() : base("The given data was invalid.")
        {
        }

        public UnprocessableEntityHttpException(string message, Dictionary<string, string[]> errors) : base(message)
        {
            Errors = errors;
        }

        public UnprocessableEntityHttpException(string message, Dictionary<string, string[]> errors, Exception inner) : base(message, inner)
        {
            Errors = errors;
        }
    }
}
