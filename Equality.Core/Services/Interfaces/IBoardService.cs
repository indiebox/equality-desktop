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
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(TProjectModel project);

        /// <inheritdoc cref="GetBoardsAsync(TProjectModel)"/>
        /// <param name="projectId">The project id.</param>
        public Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(ulong projectId);

        /// <summary>
        /// Sends the create boards request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="board">The board.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TBoardModel>> CreateBoardAsync(TProjectModel project, TBoardModel board);

        /// <inheritdoc cref="CreateBoardAsync(TProjectModel, TBoardModel)"/>
        /// <param name="projectId">The project id.</param>
        public Task<ApiResponseMessage<TBoardModel>> CreateBoardAsync(ulong projectId, TBoardModel board);

        /// <summary>
        /// Sends the update board request to the API.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TBoardModel>> UpdateBoardAsync(TBoardModel board);
    }
}
