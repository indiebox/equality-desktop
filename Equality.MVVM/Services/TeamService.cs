using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Http;
using Equality.Models;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Equality.Services
{
    public class TeamService : Core.Services.TeamService, ITeamService
    {
        public TeamService(IApiClient apiClient, Core.Services.ITokenResolverService tokenResolver) : base(apiClient, tokenResolver)
        {
        }

        public new async Task<ApiResponseMessage<Team[]>> GetTeamsAsync()
        {
            var response = await base.GetTeamsAsync();

            var teams = DeserializeRange(response.Content["data"]);

            return new(teams, response);
        }

        public async Task<ApiResponseMessage<Team>> CreateAsync(Team team)
        {
            Argument.IsNotNull(nameof(team), team);

            Dictionary<string, object> data = new()
            {
                { "name", team.Name },
                { "description", team.Description },
                { "url", team.Url }
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync("teams", data);

            team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        public Task<ApiResponseMessage<TeamMember[]>> GetMembersAsync(Team team) => GetMembersAsync(team.Id);

        public new async Task<ApiResponseMessage<TeamMember[]>> GetMembersAsync(ulong teamId)
        {
            var response = await base.GetMembersAsync(teamId);

            var members = DeserializeMembers(response.Content["data"]);

            return new(members, response);
        }

        public Task<ApiResponseMessage<Team>> SetLogoAsync(Team team, string imagePath) => SetLogoAsync(team.Id, imagePath);

        public new async Task<ApiResponseMessage<Team>> SetLogoAsync(ulong teamId, string imagePath)
        {
            var response = await base.SetLogoAsync(teamId, imagePath);

            var team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        public Task<ApiResponseMessage<Team>> DeleteLogoAsync(Team team) => DeleteLogoAsync(team.Id);

        public new async Task<ApiResponseMessage<Team>> DeleteLogoAsync(ulong teamId)
        {
            var response = await base.DeleteLogoAsync(teamId);

            var team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        public Task<ApiResponseMessage> LeaveTeamAsync(Team team) => LeaveTeamAsync(team.Id);

        public async Task<ApiResponseMessage<Team>> UpdateTeamAsync(Team team)
        {
            Argument.IsNotNull(nameof(team), team);
            Argument.IsMinimal<ulong>("Team.Id", team.Id, 1);

            Dictionary<string, object> data = new()
            {
                { "name", team.Name },
                { "description", team.Description },
                { "url", team.Url }
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PatchAsync($"teams/{team.Id}", data);

            team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        /// <summary>
        /// Deserializes the JToken to the <c>ITeamMember[]</c>.
        /// </summary>
        /// <param name="data">The JToken.</param>
        /// <returns>Returns the <c>ITeamMember[]</c>.</returns>
        public TeamMember[] DeserializeMembers(JToken data)
        {
            Argument.IsNotNull(nameof(data), data);

            return data.ToObject<TeamMember[]>(new()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.Deserialize(JToken)"/>
        protected Team Deserialize(JToken data) => ((ITeamService)this).Deserialize(data);

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected Team[] DeserializeRange(JToken data) => ((ITeamService)this).DeserializeRange(data);
    }
}
