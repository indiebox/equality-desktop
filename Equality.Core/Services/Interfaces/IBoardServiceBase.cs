using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public interface IBoardServiceBase<TBoardModel, TProjectModel>
        where TBoardModel : class, IBoard
        where TProjectModel : class, IProject
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
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage<TBoardModel[]>> GetBoardsAsync(ulong teamId);
    }
}
