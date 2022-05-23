using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class BoardServiceBase<TBoardModel> : IBoardServiceBase<TBoardModel>
        where TBoardModel : class, IBoard, new()
    {
        protected ITokenResolver TokenResolver;

        protected IApiClient ApiClient;

        public BoardServiceBase(IApiClient apiClient, ITokenResolver tokenResolver)
        {
            Argument.IsNotNull(nameof(apiClient), apiClient);
            Argument.IsNotNull(nameof(tokenResolver), tokenResolver);

            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }
        public Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(IProject project, QueryParameters query = null)
            => GetBoardsAsync(project.Id, query);

        public async Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(ulong projectId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(projectId), projectId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"projects/{projectId}/boards"));

            var boards = Json.Deserialize<TBoardModel[]>(response.Content["data"]);

            return new(boards, response);
        }

        public async Task<ApiResponseMessage<TBoardModel>> GetBoardAsync(ulong boardId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(boardId), boardId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"boards/{boardId}"));

            var board = Json.Deserialize<TBoardModel>(response.Content["data"]);

            return new(board, response);
        }

        public Task<ApiResponseMessage<TBoardModel>> CreateBoardAsync(IProject project, IBoard board, QueryParameters query = null)
            => CreateBoardAsync(project.Id, board, query);

        public async Task<ApiResponseMessage<TBoardModel>> CreateBoardAsync(ulong projectId, IBoard board, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(projectId), projectId, 1);
            Argument.IsNotNull(nameof(board), board);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", board.Name },
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync(query.Parse($"projects/{projectId}/boards"), data);
            var deserializedBoard = Json.Deserialize<TBoardModel>(response.Content["data"]);

            return new(deserializedBoard, response);
        }

        public async Task<ApiResponseMessage<TBoardModel>> UpdateBoardAsync(IBoard board, QueryParameters query = null)
        {
            Argument.IsNotNull(nameof(board), board);
            Argument.IsMinimal<ulong>("board.Id", board.Id, 1);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", board.Name },
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PatchAsync(query.Parse($"boards/{board.Id}"), data);
            var deserializedBoard = Json.Deserialize<TBoardModel>(response.Content["data"]);

            return new(deserializedBoard, response);
        }
    }
}
