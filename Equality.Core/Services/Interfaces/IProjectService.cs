using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public partial interface IProjectServiceBase<TProjectModel, TTeamModel, TLeaderNominationModel, TUserModel>
        where TProjectModel : class, IProject, new()
        where TTeamModel : class, ITeam, new()
        where TLeaderNominationModel : class, ILeaderNomination, new()
        where TUserModel : class, IUser, new()
    {
        /// <summary>
        /// Sends the get team projects request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<PaginatableApiResponse<TProjectModel>> GetProjectsAsync(ITeam team, QueryParameters query = null);

        /// <inheritdoc cref="GetProjectsAsync(ITeam, QueryParameters)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<PaginatableApiResponse<TProjectModel>> GetProjectsAsync(ulong teamId, QueryParameters query = null);

        /// <summary>
        /// Sends the get project request to the API.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> GetProjectAsync(ulong projectId, QueryParameters query = null);

        /// <summary>
        /// Sends the get nominated users request to the API.
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
        public Task<ApiResponseMessage<TLeaderNominationModel[]>> GetNominatedUsersAsync(IProject project, QueryParameters query = null);

        /// <inheritdoc cref="GetNominatedUsersAsync(IProject, QueryParameters)"/>`
        /// <param name="projectId">The project id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TLeaderNominationModel[]>> GetNominatedUsersAsync(ulong projectId, QueryParameters query = null);

        /// <summary>
        /// Sends the nominate user request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="user">The user.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TLeaderNominationModel[]>> NominateUserAsync(IProject project, IUser user, QueryParameters query = null);

        /// <inheritdoc cref="NominateUserAsync(IProject, IUser, QueryParameters)"/>`
        /// <param name="projectId">The project id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TLeaderNominationModel[]>> NominateUserAsync(ulong projectId, ulong userId, QueryParameters query = null);

        /// <summary>
        /// Sends the get project leader request to the API.
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
        public Task<ApiResponseMessage<TUserModel>> GetProjectLeaderAsync(IProject project, QueryParameters query = null);

        /// <inheritdoc cref="GetProjectLeaderAsync(IProject, QueryParameters)"/>`
        /// <param name="projectId">The project id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TUserModel>> GetProjectLeaderAsync(ulong projectId, QueryParameters query = null);

        /// <summary>
        /// Sends the get project team request to the API.
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
        public Task<ApiResponseMessage<TTeamModel>> GetTeamForProjectAsync(IProject project, QueryParameters query = null);

        /// <inheritdoc cref="GetTeamForProjectAsync(IProject, QueryParameters)"/>`
        /// <param name="projectId">The project id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TTeamModel>> GetTeamForProjectAsync(ulong projectId, QueryParameters query = null);

        /// <summary>
        /// Sends the create project for team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="project">The project.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(ITeam team, IProject project, QueryParameters query = null);

        /// <inheritdoc cref="CreateProjectAsync(ITeam, IProject, QueryParameters)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="project">The project.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(ulong teamId, IProject project, QueryParameters query = null);

        /// <summary>
        /// Sends the set image request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="imagePath">The path to image file.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> SetImageAsync(IProject project, string imagePath, QueryParameters query = null);

        /// <inheritdoc cref="SetImageAsync(IProject, string, QueryParameters)"/>
        /// <param name="projectId">The project id.</param>
        /// <param name="imagePath">The path to image file.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TProjectModel>> SetImageAsync(ulong projectId, string imagePath, QueryParameters query = null);

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
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> UpdateProjectAsync(IProject project, QueryParameters query = null);
    }
}
