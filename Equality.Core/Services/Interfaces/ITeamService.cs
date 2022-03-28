using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public interface ITeamServiceBase<TTeamModel, TTeamMemberModel> : IDeserializeModels<TTeamModel>
        where TTeamModel : class, ITeam, new()
        where TTeamMemberModel : class, ITeamMember, new()
    {
        /// <summary>
        /// Sends the get current user teams request to the API.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TTeamModel[]>> GetTeamsAsync(QueryParameters query = null);

        /// <summary>
        /// Sends the create team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TTeamModel>> CreateAsync(TTeamModel team);

        /// <summary>
        /// Sends the get members request to the API.
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
        public Task<ApiResponseMessage<TTeamMemberModel[]>> GetMembersAsync(TTeamModel team, QueryParameters query = null);

        /// <inheritdoc cref="GetMembersAsync(TTeamModel, QueryParameters)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TTeamMemberModel[]>> GetMembersAsync(ulong teamId, QueryParameters query = null);

        /// <summary>
        /// Sends the leave team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> LeaveTeamAsync(TTeamModel team);

        /// <inheritdoc cref="LeaveTeamAsync(TTeamModel)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage> LeaveTeamAsync(ulong teamId);

        /// <summary>
        /// Sends the set logo request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="imagePath">The path to image file.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TTeamModel>> SetLogoAsync(TTeamModel team, string imagePath);

        /// <inheritdoc cref="SetLogoAsync(TTeamModel, string)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="imagePath">The path to image file.</param>
        public Task<ApiResponseMessage<TTeamModel>> SetLogoAsync(ulong teamId, string imagePath);

        /// <summary>
        /// Sends the delete logo request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TTeamModel>> DeleteLogoAsync(TTeamModel team);

        /// <inheritdoc cref="DeleteLogoAsync(TTeamModel)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage<TTeamModel>> DeleteLogoAsync(ulong teamId);

        /// <summary>
        /// Sends the update team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TTeamModel>> UpdateTeamAsync(TTeamModel team);
    }
}
