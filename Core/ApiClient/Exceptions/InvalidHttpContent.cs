using System;

namespace Equality.Core.ApiClient
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
