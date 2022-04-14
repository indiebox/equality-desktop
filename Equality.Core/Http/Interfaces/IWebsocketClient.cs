using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equality.Http
{
    public interface IWebsocketClient
    {
        public string SocketID { get; }

        public Task BindEventAsync(string channelName, string eventName, Action<Dictionary<string, object>> listener);

        public void UnbindEvent(string channelName, string eventName);

        public Task UnsubscribeAsync(string channelName);

        public Task DisconnectAsync();
    }
}