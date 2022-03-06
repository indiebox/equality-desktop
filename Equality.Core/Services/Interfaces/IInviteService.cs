using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;

namespace Equality.Core.Services
{
    public interface IInviteService
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
        public Task<ApiResponseMessage> GetTeamInvitesAsync(ITeam team, InviteFilter filter = InviteFilter.All);

        /// <inheritdoc cref="GetTeamInvitesAsync(ITeam, InviteFilter)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="filter">Filter.</param>
        public Task<ApiResponseMessage> GetTeamInvitesAsync(ulong teamId, InviteFilter filter = InviteFilter.All);

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
        public Task<ApiResponseMessage> GetUserInvitesAsync();

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
        public Task<ApiResponseMessage> InviteUserAsync(ITeam team, string email);

        /// <inheritdoc cref="InviteUserAsync(ITeam, string)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="email">The user email.</param>
        public Task<ApiResponseMessage> InviteUserAsync(ulong teamId, string email);

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
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
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
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeclineInviteAsync(IInvite invite);

        /// <inheritdoc cref="DeclineInviteAsync(IInvite)"/>
        /// <param name="inviteId">The invite id.</param>
        public Task<ApiResponseMessage> DeclineInviteAsync(ulong inviteId);
    }

    public interface IInviteService<TInviteModel, TTeamModel> : IDeserializeModels<TInviteModel>
        where TInviteModel : class, new()
        where TTeamModel : class, new()
    {
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
        public Task<ApiResponseMessage<TInviteModel[]>> GetTeamInvitesAsync(TTeamModel team, IInviteService.InviteFilter filter = IInviteService.InviteFilter.All);

        /// <inheritdoc cref="GetTeamInvitesAsync(TTeamModel, InviteFilter)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="filter">Filter.</param>
        public Task<ApiResponseMessage<TInviteModel[]>> GetTeamInvitesAsync(ulong teamId, IInviteService.InviteFilter filter = IInviteService.InviteFilter.All);

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
        public Task<ApiResponseMessage<TInviteModel[]>> GetUserInvitesAsync();

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
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
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
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
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
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeclineInviteAsync(TInviteModel invite);

        /// <inheritdoc cref="DeclineInviteAsync(TInviteModel)"/>
        /// <param name="inviteId">The invite id.</param>
        public Task<ApiResponseMessage> DeclineInviteAsync(ulong inviteId);
    }
}
