using System;
using System.Threading.Tasks;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public partial class ColumnServiceBase<TColumnModel>
        where TColumnModel : class, IColumn, new()
    {
        protected IWebsocketClient WebsocketClient;

        public ColumnServiceBase(IApiClient apiClient, ITokenResolver tokenResolver, IWebsocketClient websocketClient)
            : this(apiClient, tokenResolver)
        {
            WebsocketClient = websocketClient;
        }

        public async Task SubscribeCreateColumnAsync(IBoard board, Action<TColumnModel, ulong?> action)
        {
            await WebsocketClient.BindEventAsync(GetChannelName(board), "created", (data) =>
            {
                var deserializedColumn = Json.Deserialize<TColumnModel>(data["column"].ToString());

                if (
                    data["after_column"] != null
                    && ulong.TryParse(data["after_column"].ToString(), out ulong deserializedAfterColumnId)
                ) {
                    action.Invoke(deserializedColumn, deserializedAfterColumnId);
                } else {
                    action.Invoke(deserializedColumn, null);
                }
            });
        }

        public void UnsubscribeCreateColumn(IBoard board)
        {
            WebsocketClient.UnbindEvent(GetChannelName(board), "created");
        }

        public async Task SubscribeUpdateColumnAsync(IBoard board, Action<TColumnModel> action)
        {
            await WebsocketClient.BindEventAsync(GetChannelName(board), "updated", (data) =>
            {
                var deserializedColumn = Json.Deserialize<TColumnModel>(data["column"].ToString());
                action.Invoke(deserializedColumn);
            });
        }

        public void UnsubscribeUpdateColumn(IBoard board)
        {
            WebsocketClient.UnbindEvent(GetChannelName(board), "updated");
        }

        public async Task SubscribeUpdateColumnOrderAsync(IBoard board, Action<ulong, ulong> action)
        {
            await WebsocketClient.BindEventAsync(GetChannelName(board), "order-changed", (data) =>
            {
                action.Invoke(ulong.Parse(data["id"].ToString()), ulong.Parse(data["after"].ToString()));
            });
        }

        public void UnsubscribeUpdateColumnOrder(IBoard board)
        {
            WebsocketClient.UnbindEvent(GetChannelName(board), "order-changed");
        }

        public async Task SubscribeDeleteColumnAsync(IBoard board, Action<ulong> action)
        {
            await WebsocketClient.BindEventAsync(GetChannelName(board), "deleted", (data) =>
            {
                action.Invoke(ulong.Parse(data["id"].ToString()));
            });
        }

        public void UnsubscribeDeleteColumn(IBoard board)
        {
            WebsocketClient.UnbindEvent(GetChannelName(board), "deleted");
        }

        protected string GetChannelName(IBoard board) => $"private-boards.{board.Id}.columns";
    }
}
