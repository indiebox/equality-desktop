using System;
using System.Threading.Tasks;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public partial class CardServiceBase<TCardModel>
        where TCardModel : class, ICard, new()
    {
        protected IWebsocketClient WebsocketClient;

        public CardServiceBase(IApiClient apiClient, ITokenResolver tokenResolver, IWebsocketClient websocketClient)
            : this(apiClient, tokenResolver)
        {
            WebsocketClient = websocketClient;
        }

        public async Task SubscribeCreateCardAsync(IBoard board, Action<TCardModel, ulong, ulong?> action)
        {
            await WebsocketClient.BindEventAsync(GetChannelName(board), "created", (data) =>
            {
                var deserializedCard = Json.Deserialize<TCardModel>(data["card"].ToString());
                var columnId = Json.Deserialize<ulong>(data["column"].ToString());

                if (
                    data["after_card"] != null
                    && ulong.TryParse(data["after_card"].ToString(), out ulong deserializedAfterCardId)
                ) {
                    action.Invoke(deserializedCard, columnId, deserializedAfterCardId);
                } else {
                    action.Invoke(deserializedCard, columnId, null);
                }
            });
        }

        public void UnsubscribeCreateCard(IBoard board)
        {
            WebsocketClient.UnbindEvent(GetChannelName(board), "created");
        }

        protected string GetChannelName(IBoard board) => $"private-boards.{board.Id}.cards";
    }
}
