using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public interface IProjectServiceBase<TProjectModel, TTeamModel, TLeaderNominationModel, TUserModel> : IDeserializeModels<TProjectModel>
        where TUserModel : class, IUser, new()
        where TProjectModel : class, IProject, new()
        where TTeamModel : class, ITeam, new()
        where TLeaderNominationModel : class, ILeaderNomination, new()
    {
        /// <summary>
        /// Sends the get team projects request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(TTeamModel team);

        /// <inheritdoc cref="GetProjectsAsync(ITeam)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(ulong teamId);

        /// <summary>
        /// Sends the get nominated users request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TLeaderNominationModel[]>> GetNominatedUsersAsync(TProjectModel project);

        /// <inheritdoc cref="GetNominatedUsersAsync(TProjectModel)"/>`
        /// <param name="projectId">The project id.</param>
        public Task<ApiResponseMessage<TLeaderNominationModel[]>> GetNominatedUsersAsync(ulong projectId);

        /// <summary>
        /// Sends the get leader project request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TUserModel>> GetProjectLeader(TProjectModel project);

        /// <inheritdoc cref="GetProjectLeader(TProjectModel)"/>`
        /// <param name="projectId">The project id.</param>
        public Task<ApiResponseMessage<TUserModel>> GetProjectLeader(ulong projectId);

        /// <summary>
        /// Sends the create project for team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="project">The project.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(TTeamModel team, TProjectModel project);

        /// <inheritdoc cref="CreateProjectAsync(ITeam, IProject)"/>
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
