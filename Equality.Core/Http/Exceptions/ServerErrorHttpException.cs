namespace Equality.Http
{
    public class ServerErrorHttpException : ApiException
    {
        public ServerErrorHttpException()
        {
        }

        public ServerErrorHttpException(string message) : base(message)
        {
        }
    }
}
