namespace Equality.Http
{
    public class BadRequestHttpException : ApiException
    {
        public BadRequestHttpException()
        {
        }

        public BadRequestHttpException(string message) : base(message)
        {
        }
    }
}
