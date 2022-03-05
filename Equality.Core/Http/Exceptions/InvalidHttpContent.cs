using System;

namespace Equality.Http
{
    public class InvalidHttpContent : Exception
    {
        public InvalidHttpContent()
        {
        }

        public InvalidHttpContent(string message) : base(message)
        {
        }
    }
}
