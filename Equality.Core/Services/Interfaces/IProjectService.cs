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
        /// Sends the set image request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="imagePath">The path to image file.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> SetImageAsync(IProject project, string imagePath);

        /// <inheritdoc cref="SetImageAsync(IProject, string)"/>
        /// <param name="projectId">The project id.</param>
        /// <param name="imagePath">The path to image file.</param>
        public Task<ApiResponseMessage> SetImageAsync(ulong projectId, string imagePath);

        /// <summary>
        /// Sends the delete image request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeleteImageAsync(IProject project);

        /// <inheritdoc cref="DeleteImageAsync(IProject)"/>
        /// <param name="projectId">The project id.</param>
        public Task<ApiResponseMessage> DeleteImageAsync(ulong projectId);

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
        /// Sends the set image request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="imagePath">The path to image file.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> SetImageAsync(TProjectModel project, string imagePath);

        /// <inheritdoc cref="SetImageAsync(TProjectModel, string)"/>
        /// <param name="projectId">The project id.</param>
        /// <param name="imagePath">The path to image file.</param>
        public Task<ApiResponseMessage<TProjectModel>> SetImageAsync(ulong projectId, string imagePath);

        /// <summary>
        /// Sends the delete image request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> DeleteImageAsync(TProjectModel project);

        /// <inheritdoc cref="DeleteImageAsync(TProjectModel)"/>
        /// <param name="projectId">The project id.</param>
        public Task<ApiResponseMessage<TProjectModel>> DeleteImageAsync(ulong projectId);

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
