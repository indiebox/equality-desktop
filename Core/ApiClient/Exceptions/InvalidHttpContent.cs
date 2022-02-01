using System;

namespace Equality.Core.ApiClient.Exceptions
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
