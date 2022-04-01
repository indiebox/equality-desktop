using System.Net.Http;

namespace Equality.Http
{
    public class ServerErrorHttpException : HttpRequestException
    {
        public ServerErrorHttpException()
        {
        }

        public ServerErrorHttpException(string message) : base(message)
        {
        }
    }
}
