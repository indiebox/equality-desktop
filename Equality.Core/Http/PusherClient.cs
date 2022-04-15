using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Equality.Data;

using PusherClient;

namespace Equality.Http
{
    public class PusherClient : Pusher, IWebsocketClient
    {
        public PusherClient(string applicationKey, PusherOptions options = null) : base(applicationKey, options)
        {
        }

        public async Task BindEventAsync(string channelName, string eventName, Action<Dictionary<string, object>> listener)
        {
            await ConnectAsync();

            var channel = await SubscribeAsync(channelName);
            channel.Bind(eventName, (PusherEvent data) =>
            {
                listener.Invoke(Json.Deserialize<Dictionary<string, object>>(data.Data));
            });
        }

        public void UnbindEvent(string channelName, string eventName)
        {
            var channel = GetChannel(channelName);

            if (channel != null) {
                channel.Unbind(eventName);
            }
        }
    }
}
