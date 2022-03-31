using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public interface IProjectServiceBase<TProjectModel, TTeamModel, TLeaderNominationModel, TUserModel> : IDeserializeModels<TProjectModel>
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
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(TTeamModel team, QueryParameters query = null);

        /// <inheritdoc cref="GetProjectsAsync(TTeamModel, QueryParameters)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(ulong teamId, QueryParameters query = null);

        /// <summary>
        /// Sends the get nominated users request to the API.
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
        public Task<ApiResponseMessage<TLeaderNominationModel[]>> GetNominatedUsersAsync(TProjectModel project, QueryParameters query = null);

        /// <inheritdoc cref="GetNominatedUsersAsync(TProjectModel, QueryParameters)"/>`
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
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TLeaderNominationModel[]>> NominateUserAsync(TProjectModel project, TUserModel user, QueryParameters query = null);

        /// <inheritdoc cref="NominateUserAsync(TProjectModel, TUserModel, QueryParameters)"/>`
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
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TUserModel>> GetProjectLeaderAsync(TProjectModel project, QueryParameters query = null);

        /// <inheritdoc cref="GetProjectLeaderAsync(TProjectModel, QueryParameters)"/>`
        /// <param name="projectId">The project id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TUserModel>> GetProjectLeaderAsync(ulong projectId, QueryParameters query = null);

        /// <summary>
        /// Sends the create project for team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="project">The project.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(TTeamModel team, TProjectModel project, QueryParameters query = null);

        /// <inheritdoc cref="CreateProjectAsync(TTeamModel, TProjectModel, QueryParameters)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="project">The project.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(ulong teamId, TProjectModel project, QueryParameters query = null);

        /// <summary>
        /// Sends the set image request to the API.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="imagePath">The path to image file.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TProjectModel>> SetImageAsync(TProjectModel project, string imagePath, QueryParameters query = null);

        /// <inheritdoc cref="SetImageAsync(TProjectModel, string, QueryParameters)"/>
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
        public Task<ApiResponseMessage> DeleteImageAsync(TProjectModel project);

        /// <inheritdoc cref="DeleteImageAsync(TProjectModel)"/>
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
        public Task<ApiResponseMessage<TProjectModel>> UpdateProjectAsync(TProjectModel project, QueryParameters query = null);
    }
}
