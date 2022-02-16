using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class TeamService : ITeamService
    {
        protected IApiClient ApiClient;

        protected IStateManager StateManager;

        public TeamService(IApiClient apiClient, IStateManager stateManager)
        {
            ApiClient = apiClient;
            StateManager = stateManager;
        }

        public async Task<ApiResponseMessage<Team[]>> GetTeamsAsync()
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync("teams");

            var teams = DeserializeRange(response.Content["data"]);

            return new(teams, response);
        }

        public async Task<ApiResponseMessage<Team>> CreateAsync(Team team)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(team), team);
            Argument.IsNotNullOrWhitespace("team.name", team.Name);

            Dictionary<string, object> data = new()
            {
                { "name", team.Name },
                { "description", team.Description },
                { "url", team.Url }
            };

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync("teams", data);

            team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        /// <inheritdoc cref="IApiDeserializable{T}.Deserialize(JToken)"/>
        protected Team Deserialize(JToken data) => ((ITeamService)this).Deserialize(data);

        /// <inheritdoc cref="IApiDeserializable{T}.DeserializeRange(JToken)"/>
        protected Team[] DeserializeRange(JToken data) => ((ITeamService)this).DeserializeRange(data);
    }
}
