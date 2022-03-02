using System;
using System.Threading.Tasks;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Models;

namespace Equality.Services
{
    public interface IInviteService : IApiDeserializable<Invite>
    {
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
    }
}
