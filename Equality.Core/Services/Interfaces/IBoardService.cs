using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public interface IBoardServiceBase<TBoardModel> : IDeserializeModels<TBoardModel>
        where TBoardModel : class, IBoard, new()
    {
        /// <summary>
        /// Sends the get boards request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(IProject project, QueryParameters query = null);

        /// <inheritdoc cref="GetBoardsAsync(IProject, QueryParameters)"/>
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
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TBoardModel>> CreateBoardAsync(IProject project, IBoard board, QueryParameters query = null);

        /// <inheritdoc cref="CreateBoardAsync(IProject, IBoard, QueryParameters)"/>
        /// <param name="projectId">The project id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TBoardModel>> CreateBoardAsync(ulong projectId, IBoard board, QueryParameters query = null);

        /// <summary>
        /// Sends the update board request to the API.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TBoardModel>> UpdateBoardAsync(IBoard board, QueryParameters query = null);
    }
}
