namespace Equality.Data
{
    public class TokenResolver : ITokenResolverService
    {
        public string ResolveApiToken() => StateManager.ApiToken;
    }
}
