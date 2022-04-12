namespace Equality.Data
{
    public interface ITokenResolver
    {
        public string ResolveApiToken();

        public string ResolveSocketID();
    }
}
