using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public interface IBoardServiceBase<TBoardModel, TProjectModel> : IDeserializeModels<TBoardModel>
        where TBoardModel : class, IBoard, new()
        where TProjectModel : class, IProject, new()
    {
        /// <summary>
        /// Sends the get boards request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(TProjectModel project, QueryParameters query = null);

        /// <inheritdoc cref="GetBoardsAsync(TProjectModel, QueryParameters)"/>
        /// <param name="projectId">The project id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(ulong projectId, QueryParameters query = null);

        /// <summary>
        /// Sends the create boards request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="board">The board.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TBoardModel>> CreateBoardAsync(TProjectModel project, TBoardModel board, QueryParameters query = null);

        /// <inheritdoc cref="CreateBoardAsync(TProjectModel, TBoardModel, QueryParameters)"/>
        /// <param name="projectId">The project id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TBoardModel>> CreateBoardAsync(ulong projectId, TBoardModel board, QueryParameters query = null);

        /// <summary>
        /// Sends the update board request to the API.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TBoardModel>> UpdateBoardAsync(TBoardModel board, QueryParameters query = null);
    }
}
