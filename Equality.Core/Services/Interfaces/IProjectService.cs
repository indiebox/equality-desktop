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

        /// <summary>
        /// Sends the create project for team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="project">The project.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> CreateProjectAsync(ITeam team, IProject project);

        /// <inheritdoc cref="CreateProjectAsync(ITeam, IProject)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="project">The project.</param>
        public Task<ApiResponseMessage> CreateProjectAsync(ulong teamId, IProject project);

        /// <summary>
        /// Sends the update project request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> UpdateProjectAsync(IProject project);
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

        /// <summary>
        /// Sends the create project for team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="project">The project.</param>
        /// <returns>Returns the API response.</returns>
        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(TTeamModel team, TProjectModel project);

        /// <inheritdoc cref="CreateProjectAsync(TTeamModel, TProjectModel)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="project">The project.</param>
        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(ulong teamId, TProjectModel project);

        /// <summary>
        /// Sends the update project request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> UpdateProjectAsync(TProjectModel project);
    }
}
