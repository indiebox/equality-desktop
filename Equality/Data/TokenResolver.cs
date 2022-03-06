using Equality.Core.Services;
using Equality.Models;

namespace Equality.Data
{
    public class TokenResolver : ITokenResolverService
    {
        public string ResolveApiToken() => StateManager.ApiToken;

        public IModelWithId ResolveCurrentUser() => StateManager.CurrentUser;
    }
}
