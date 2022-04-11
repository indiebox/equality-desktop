using Catel.IoC;

using Equality.Http;

namespace Equality.Data
{
    public class TokenResolver : ITokenResolver
    {
        public string ResolveApiToken() => StateManager.ApiToken;

        public string ResolveSocketID() => this.GetDependencyResolver().Resolve<IWebsocketClient>().SocketID;
    }
}
