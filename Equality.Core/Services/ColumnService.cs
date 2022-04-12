using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class ColumnServiceBase<TColumnModel, TBoardModel> : IColumnService<TColumnModel, TBoardModel>
        where TColumnModel : class, IColumn, new()
        where TBoardModel : class, IBoard, new()
    {
        protected IApiClient ApiClient;

        protected ITokenResolver TokenResolver;

        protected IWebsocketClient WebsocketClient;

        public ColumnServiceBase(IApiClient apiClient, ITokenResolver tokenResolver)
        {
            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public ColumnServiceBase(IApiClient apiClient, ITokenResolver tokenResolver, IWebsocketClient websocketClient)
            : this(apiClient, tokenResolver)
        {
            WebsocketClient = websocketClient;
        }

        public Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(TBoardModel board, QueryParameters query = null)
            => GetColumnsAsync(board.Id, query);

        public async Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(ulong boardId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(boardId), boardId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"boards/{boardId}/columns"));

            var columns = DeserializeRange(response.Content["data"]);

            return new(columns, response);
        }

        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(TBoardModel board, TColumnModel column, QueryParameters query = null)
            => CreateColumnAsync(board.Id, column, query);

        public async Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(ulong boardId, TColumnModel column, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(boardId), boardId, 1);
            Argument.IsNotNull(nameof(column), column);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", column.Name },
            };

            var response = await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .PostAsync(query.Parse($"boards/{boardId}/columns"), data);

            column = Deserialize(response.Content["data"]);

            return new(column, response);
        }

        public async Task<ApiResponseMessage<TColumnModel>> UpdateColumnAsync(TColumnModel column, QueryParameters query = null)
        {
            Argument.IsNotNull(nameof(column), column);
            Argument.IsMinimal<ulong>("column.Id", column.Id, 1);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", column.Name },
            };

            var response = await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .PatchAsync(query.Parse($"columns/{column.Id}"), data);

            column = Deserialize(response.Content["data"]);

            return new(column, response);
        }

        public Task<ApiResponseMessage> UpdateColumnOrderAsync(TColumnModel column, TColumnModel afterColumn)
            => UpdateColumnOrderAsync(column.Id, afterColumn?.Id ?? 0);

        public async Task<ApiResponseMessage> UpdateColumnOrderAsync(ulong columnId, ulong afterColumnId)
        {
            Argument.IsMinimal<ulong>(nameof(columnId), columnId, 1);

            Dictionary<string, object> data = new()
            {
                { "after", afterColumnId.ToString() },
            };

            return await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .PostAsync($"columns/{columnId}/order", data);
        }

        public Task<ApiResponseMessage> DeleteColumnAsync(TColumnModel column)
            => DeleteColumnAsync(column.Id);

        public async Task<ApiResponseMessage> DeleteColumnAsync(ulong columnId)
        {
            Argument.IsMinimal<ulong>(nameof(columnId), columnId, 1);

            return await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .DeleteAsync($"columns/{columnId}");
        }

        #region Websockets

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

        #endregion

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected TColumnModel Deserialize(JToken data) => ((IDeserializeModels<TColumnModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected TColumnModel[] DeserializeRange(JToken data) => ((IDeserializeModels<TColumnModel>)this).DeserializeRange(data);
    }
}
