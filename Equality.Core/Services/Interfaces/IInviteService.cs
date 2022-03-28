using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public interface IInviteServiceBase<TInviteModel, TTeamModel> : IDeserializeModels<TInviteModel>
        where TInviteModel : class, IInvite, new()
        where TTeamModel : class, ITeam, new()
    {
        /// <summary>
        /// Sends the get team invites request to the API.
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
        public Task<ApiResponseMessage<TInviteModel[]>> GetTeamInvitesAsync(TTeamModel team, QueryParameters query = null);

        /// <inheritdoc cref="GetTeamInvitesAsync(TTeamModel, QueryParameters)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TInviteModel[]>> GetTeamInvitesAsync(ulong teamId, QueryParameters query = null);

        /// <summary>
        /// Sends the get user invites request to the API.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TInviteModel[]>> GetUserInvitesAsync(QueryParameters query = null);

        /// <summary>
        /// Sends the invite user to the team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="email">The user email.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TInviteModel>> InviteUserAsync(TTeamModel team, string email);

        /// <inheritdoc cref="InviteUserAsync(TTeamModel, string)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="email">The user email.</param>
        public Task<ApiResponseMessage<TInviteModel>> InviteUserAsync(ulong teamId, string email);

        /// <summary>
        /// Sends the revoke invite request to the API.
        /// </summary>
        /// <param name="invite">The invite.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> RevokeInviteAsync(TInviteModel invite);

        /// <inheritdoc cref="RevokeInviteAsync(TInviteModel)"/>
        /// <param name="inviteId">The invite id.</param>
        public Task<ApiResponseMessage> RevokeInviteAsync(ulong inviteId);

        /// <summary>
        /// Sends the accept invite request to the API.
        /// </summary>
        /// <param name="invite">The invite.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> AcceptInviteAsync(TInviteModel invite);

        /// <inheritdoc cref="AcceptInviteAsync(TInviteModel)"/>
        /// <param name="inviteId">The invite id.</param>
        public Task<ApiResponseMessage> AcceptInviteAsync(ulong inviteId);

        /// <summary>
        /// Sends the decline invite request to the API.
        /// </summary>
        /// <param name="invite">The invite.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeclineInviteAsync(TInviteModel invite);

        /// <inheritdoc cref="DeclineInviteAsync(TInviteModel)"/>
        /// <param name="inviteId">The invite id.</param>
        public Task<ApiResponseMessage> DeclineInviteAsync(ulong inviteId);
    }
}
