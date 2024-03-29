﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public partial class ColumnServiceBase<TColumnModel> : IColumnService<TColumnModel>
        where TColumnModel : class, IColumn, new()
    {
        protected IApiClient ApiClient;

        protected ITokenResolver TokenResolver;

        public ColumnServiceBase(IApiClient apiClient, ITokenResolver tokenResolver)
        {
            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(IBoard board, QueryParameters query = null)
            => GetColumnsAsync(board.Id, query);

        public async Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(ulong boardId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(boardId), boardId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"boards/{boardId}/columns"));

            var columns = Json.Deserialize<TColumnModel[]>(response.Content["data"]);

            return new(columns, response);
        }

        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(IBoard board, IColumn column, QueryParameters query = null)
            => CreateColumnAsync(board.Id, column, query);

        public async Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(ulong boardId, IColumn column, QueryParameters query = null)
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
            var deserializedColumn = Json.Deserialize<TColumnModel>(response.Content["data"]);

            return new(deserializedColumn, response);
        }

        public async Task<ApiResponseMessage<TColumnModel>> UpdateColumnAsync(IColumn column, QueryParameters query = null)
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
            var deserializedColumn = Json.Deserialize<TColumnModel>(response.Content["data"]);

            return new(deserializedColumn, response);
        }

        public Task<ApiResponseMessage> UpdateColumnOrderAsync(IColumn column, IColumn afterColumn)
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

        public Task<ApiResponseMessage> DeleteColumnAsync(IColumn column)
            => DeleteColumnAsync(column.Id);

        public async Task<ApiResponseMessage> DeleteColumnAsync(ulong columnId)
        {
            Argument.IsMinimal<ulong>(nameof(columnId), columnId, 1);

            return await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .DeleteAsync($"columns/{columnId}");
        }
    }
}
