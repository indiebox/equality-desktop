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
    public class ColumnServiceBase<TColumnModel, TBoardModel> : IColumnServiceBase<TColumnModel, TBoardModel>
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

        public Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(IBoard board) => GetColumnsAsync(board.Id);

        public async Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(ulong boardId)
        {
            Argument.IsNotNull(nameof(boardId), boardId);

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync($"boards/{boardId}/columns");

            var columns = DeserializeRange(response.Content["data"]);

            return new(columns, response);
        }

        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(TBoardModel board, TColumnModel column) => CreateColumnAsync(board.Id, column);

        public async Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(ulong boardId, TColumnModel column)
        {
            Argument.IsNotNull(nameof(boardId), boardId);
            Argument.IsNotNull(nameof(column), column);

            Dictionary<string, object> data = new()
            {
                { "name", column.Name },
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"boards/{boardId}/columns", data);

            column = Deserialize(response.Content["data"]);

            return new(column, response);
        }

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected TColumnModel Deserialize(JToken data) => ((IDeserializeModels<TColumnModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected TColumnModel[] DeserializeRange(JToken data) => ((IDeserializeModels<TColumnModel>)this).DeserializeRange(data);
    }
}
