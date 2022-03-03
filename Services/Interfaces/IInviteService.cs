using System;
using System.Threading.Tasks;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Models;

namespace Equality.Services
{
    public interface IInviteService : IApiDeserializable<Invite>
    {
        public enum InviteFilter
        {
            All,
            Pending,
            Accepted,
            Declined,
        };

        /// <summary>
        /// Sends the get team invites request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="filter">Filter.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<Invite[]>> GetTeamInvitesAsync(Team team, InviteFilter filter = InviteFilter.All);

        /// <inheritdoc cref="GetTeamInvitesAsync(Team, InviteFilter)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="filter">Filter.</param>
        public Task<ApiResponseMessage<Invite[]>> GetTeamInvitesAsync(ulong teamId, InviteFilter filter = InviteFilter.All);

        /// <summary>
        /// Sends the get user invites request to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<Invite[]>> GetUserInvitesAsync();

        /// <summary>
        /// Sends the invite user to the team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="email">The user email.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<Invite>> InviteUserAsync(Team team, string email);

        /// <inheritdoc cref="InviteUserAsync(Team, string)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="email">The user email.</param>
        public Task<ApiResponseMessage<Invite>> InviteUserAsync(ulong teamId, string email);

        /// <summary>
        /// Sends the revoke invite request to the API.
        /// </summary>
        /// <param name="invite">The invite.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> RevokeInviteAsync(Invite invite);

        /// <inheritdoc cref="RevokeInviteAsync(Invite)"/>
        /// <param name="inviteId">The invite id.</param>
        public Task<ApiResponseMessage> RevokeInviteAsync(ulong inviteId);
    }
}
