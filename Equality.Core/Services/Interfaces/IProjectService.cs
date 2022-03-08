using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;

namespace Equality.Core.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// Sends the get team projects request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> GetProjectsAsync(ITeam team);

        /// <inheritdoc cref="GetProjectsAsync(ITeam)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage> GetProjectsAsync(ulong teamId);
    }
    public interface IProjectService<TProjectModel, TTeamModel> : IDeserializeModels<TProjectModel>
        where TProjectModel : class, new()
        where TTeamModel : class, new()
    {
        /// <summary>
        /// Sends the get team projects request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(TTeamModel team);

        /// <inheritdoc cref="GetProjectsAsync(TTeamModel)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(ulong teamId);
    }
}
