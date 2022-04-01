using System.Net.Http;

namespace Equality.Http
{
    public class BadRequestHttpException : HttpRequestException
    {
        public BadRequestHttpException()
        {
        }

        public BadRequestHttpException(string message) : base(message)
        {
        }
    }
}
