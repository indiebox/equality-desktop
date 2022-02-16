using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        public async Task<Team[]> GetTeamsAsync()
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync("teams");

            var teams = DeserializeRange(response.Content["data"].ToString());

            return teams;
        }

        public async Task<ApiResponseMessage> CreateAsync(Team team)
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

            return response;
        }

        public Team Deserialize(string data)
        {
            Argument.IsNotNullOrWhitespace(nameof(data), data);

            var team = JsonConvert.DeserializeObject<Team>(data, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
            });

            return team;
        }

        public Team[] DeserializeRange(string data)
        {
            Argument.IsNotNullOrWhitespace(nameof(data), data);

            var team = JsonConvert.DeserializeObject<Team[]>(data, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
            });

            return team;
        }
    }
}
