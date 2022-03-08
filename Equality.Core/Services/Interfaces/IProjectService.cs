using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;

namespace Equality.Core.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// Sends the get current team project request to the API.
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

        /// <inheritdoc cref="GetProjectsAsync(Project)"/>
        /// <param name="teamId">The project id.</param>
        public Task<ApiResponseMessage> GetProjectsAsync(ulong teamId);
    }
    public interface IProjectService<TTeamModel, IProjectModel> : IDeserializeModels<TTeamModel>
        where TTeamModel : class, new()
        where IProjectModel : class, new()
    {
        /// <summary>
        /// Sends the get project request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<IProjectModel[]>> GetProjectsAsync(TTeamModel team);

        /// <inheritdoc cref="GetProjectsAsync(TTeamModel)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage<IProjectModel[]>> GetProjectsAsync(ulong teamId);
    }
}
