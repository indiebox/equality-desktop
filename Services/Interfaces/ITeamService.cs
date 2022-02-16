using System;
using System.Threading.Tasks;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Models;

namespace Equality.Services
{
    public interface ITeamService
    {
        /// <summary>
        /// Sends the get current user teams request to the API.
        /// </summary>
        /// <returns>Returns current user teams.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<Team[]> GetTeamsAsync();

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
        public Task<ApiResponseMessage> CreateAsync(Team team);

        /// <summary>
        /// Deserializes the JSON string to the <see cref="Team"/> model.
        /// </summary>
        /// <param name="data">The JSON string.</param>
        /// <returns>The deserialized <see cref="Team"/> model.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Team Deserialize(string data);

        /// <summary>
        /// Deserializes the JSON string to the <see cref="Team"/> model array.
        /// </summary>
        /// <param name="data">The JSON string.</param>
        /// <returns>The deserialized array of <see cref="Team"/> models.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Team[] DeserializeRange(string data);
    }
}
