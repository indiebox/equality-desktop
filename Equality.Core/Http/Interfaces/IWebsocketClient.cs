using System;
using System.Threading.Tasks;

namespace Equality.Http
{
    public interface IWebsocketClient
    {
        public string SocketID { get; }

        public Task BindEventAsync(string channelName, string eventName, Action<string> listener);

        public void UnbindEvent(string channelName, string eventName);

        public Task UnsubscribeAsync(string channelName);

        public Task DisconnectAsync();
    }
}