using System;

namespace Equality.Http
{
    public class ForbiddenHttpException : ApiException
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
