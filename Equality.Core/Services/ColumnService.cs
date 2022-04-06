using System;
using System.Collections.Generic;
using System.Text;
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
        IApiClient ApiClient;
        ITokenResolverService TokenResolver;

        public ColumnServiceBase(IApiClient apiClient, ITokenResolverService tokenResolver)
        {
            ApiClient = apiClient;
            TokenResolver = tokenResolver;
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

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync(query.Parse($"boards/{boardId}/columns"), data);

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

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"columns/{columnId}/order", data);
        }

        public Task<ApiResponseMessage> DeleteColumnAsync(TColumnModel column)
            => DeleteColumnAsync(column.Id);

        public async Task<ApiResponseMessage> DeleteColumnAsync(ulong columnId)
        {
            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).DeleteAsync($"columns/{columnId}");
        }

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected TColumnModel Deserialize(JToken data) => ((IDeserializeModels<TColumnModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected TColumnModel[] DeserializeRange(JToken data) => ((IDeserializeModels<TColumnModel>)this).DeserializeRange(data);
    }
}
