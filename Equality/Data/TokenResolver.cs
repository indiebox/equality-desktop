using Catel.IoC;

using Equality.Http;

namespace Equality.Data
{
    public class TokenResolver : ITokenResolver
    {
        public string ResolveApiToken() => StateManager.ApiToken;

        public string ResolveSocketID()
        {
            var client = this.GetDependencyResolver().TryResolve<IWebsocketClient>();

            return client is IWebsocketClient websocket ? websocket.SocketID : null;
        }
    }
}
