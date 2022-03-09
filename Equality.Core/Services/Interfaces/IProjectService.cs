using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

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
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
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
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> CreateProjectAsync(ITeam team, IProject project);

        /// <inheritdoc cref="CreateProjectAsync(ITeam, IProject)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="project">The project.</param>
        public Task<ApiResponseMessage> CreateProjectAsync(ulong teamId, IProject project);
    }
    public interface IProjectService<TProjectModel, TTeamModel> : IDeserializeModels<TProjectModel>
        where TProjectModel : class, new()
        where TTeamModel : class, new()
    {
        /// <inheritdoc cref="IProjectService.GetProjectsAsync(ITeam)" />
        public Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(TTeamModel team);

        /// <inheritdoc cref="IProjectService.GetProjectsAsync(ulong)" />
        public Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(ulong teamId);

        /// <inheritdoc cref="IProjectService.CreateProjectAsync(ITeam, IProject)" />
        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(TTeamModel team, TProjectModel project);

        /// <inheritdoc cref="IProjectService.CreateProjectAsync(ulong, IProject)" />
        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(ulong teamId, TProjectModel project);
    }
}
