using System;
using System.Threading.Tasks;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Models;

namespace Equality.Services
{
    public interface ITeamService : IApiDeserializable<Team>
    {
        /// <summary>
        /// Sends the get current user teams request to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<Team[]>> GetTeamsAsync();

        /// <summary>
        /// Sends the create team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<Team>> CreateAsync(Team team);

        /// <summary>
        /// Sends the get members request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TeamMember[]>> GetMembersAsync(Team team);

        /// <inheritdoc cref="GetMembersAsync(Team)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage<TeamMember[]>> GetMembersAsync(ulong teamId);
    }
}
