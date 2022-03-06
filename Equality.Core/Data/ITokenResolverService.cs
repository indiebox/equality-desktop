using Equality.Models;

namespace Equality.Core.Services
{
    public interface ITokenResolverService
    {
        public string ResolveApiToken();

        public IModelWithId ResolveCurrentUser();
    }
}
