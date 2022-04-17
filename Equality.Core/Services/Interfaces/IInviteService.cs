using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public interface IInviteServiceBase<TInviteModel> : IDeserializeModels<TInviteModel>
        where TInviteModel : class, IInvite, new()
    {
        /// <summary>
        /// Sends the get team invites request to the API.
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
        public Task<ApiResponseMessage<TInviteModel[]>> GetTeamInvitesAsync(ITeam team, QueryParameters query = null);

        /// <inheritdoc cref="GetTeamInvitesAsync(ITeam, QueryParameters)"/>
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
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TInviteModel[]>> GetUserInvitesAsync(QueryParameters query = null);

        /// <summary>
        /// Sends the invite user to the team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="email">The user email.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TInviteModel>> InviteUserAsync(ITeam team, string email, QueryParameters query = null);

        /// <inheritdoc cref="InviteUserAsync(ITeam, string, QueryParameters)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="email">The user email.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TInviteModel>> InviteUserAsync(ulong teamId, string email, QueryParameters query = null);

        /// <summary>
        /// Sends the revoke invite request to the API.
        /// </summary>
        /// <param name="invite">The invite.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> RevokeInviteAsync(IInvite invite);

        /// <inheritdoc cref="RevokeInviteAsync(IInvite)"/>
        /// <param name="inviteId">The invite id.</param>
        public Task<ApiResponseMessage> RevokeInviteAsync(ulong inviteId);

        /// <summary>
        /// Sends the accept invite request to the API.
        /// </summary>
        /// <param name="invite">The invite.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> AcceptInviteAsync(IInvite invite);

        /// <inheritdoc cref="AcceptInviteAsync(IInvite)"/>
        /// <param name="inviteId">The invite id.</param>
        public Task<ApiResponseMessage> AcceptInviteAsync(ulong inviteId);

        /// <summary>
        /// Sends the decline invite request to the API.
        /// </summary>
        /// <param name="invite">The invite.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeclineInviteAsync(IInvite invite);

        /// <inheritdoc cref="DeclineInviteAsync(IInvite)"/>
        /// <param name="inviteId">The invite id.</param>
        public Task<ApiResponseMessage> DeclineInviteAsync(ulong inviteId);
    }
}
