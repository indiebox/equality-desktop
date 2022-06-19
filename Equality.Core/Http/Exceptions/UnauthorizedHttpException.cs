using System;

namespace Equality.Http
{
    public class UnauthorizedHttpException : ApiException
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
