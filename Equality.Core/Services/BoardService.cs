using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Equality.Services
{
    public class BoardServiceBase<TBoardModel, TProjectModel> : IBoardServiceBase<TBoardModel, TProjectModel>
        where TBoardModel : class, IBoard, new()
        where TProjectModel : class, IProject, new()
    {

        protected ITokenResolverService TokenResolver;

        protected IApiClient ApiClient;

        public BoardServiceBase(IApiClient apiClient, ITokenResolverService tokenResolver)
        {
            Argument.IsNotNull(nameof(apiClient), apiClient);
            Argument.IsNotNull(nameof(tokenResolver), tokenResolver);

            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }
        public Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(TProjectModel project) => GetBoardsAsync(project.Id);

        public async Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(ulong projectId)
        {
            Argument.IsNotNull(nameof(projectId), projectId);

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync($"projects/{projectId}/boards");

            var boards = DeserializeRange(response.Content["data"]);

            return new(boards, response);
        }

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected TBoardModel Deserialize(JToken data) => ((IDeserializeModels<TBoardModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected TBoardModel[] DeserializeRange(JToken data) => ((IDeserializeModels<TBoardModel>)this).DeserializeRange(data);
    }
}
