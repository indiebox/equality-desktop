using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Catel;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Models;

using Newtonsoft.Json.Linq;
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

        public Task<ApiResponseMessage<TeamMember[]>> GetMembersAsync(Team team) => GetMembersAsync(team.Id);

        public async Task<ApiResponseMessage<TeamMember[]>> GetMembersAsync(ulong teamId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync($"teams/{teamId}/members");

            var members = DeserializeMembers(response.Content["data"]);

            return new(members, response);
        }

        public Task<ApiResponseMessage<Team>> SetLogoAsync(Team team, string imagePath) => SetLogoAsync(team.Id, imagePath);

        public async Task<ApiResponseMessage<Team>> SetLogoAsync(ulong teamId, string imagePath)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);
            Argument.IsNotNull(nameof(imagePath), imagePath);

            var fileInfo = new FileInfo(imagePath);
            using var fileStream = fileInfo.OpenRead();

            var content = new StreamContent(fileStream);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(MimeTypes.GetMimeType(imagePath));
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "logo",
                FileName = fileInfo.Name,
                FileNameStar = fileInfo.Name,
            };

            Dictionary<string, object> data = new()
            {
                { "logo", content }
            };

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync($"teams/{teamId}/logo", data);

            var team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        public Task<ApiResponseMessage<Team>> DeleteLogoAsync(Team team) => DeleteLogoAsync(team.Id);

        public async Task<ApiResponseMessage<Team>> DeleteLogoAsync(ulong teamId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).DeleteAsync($"teams/{teamId}/logo");

            var team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        /// <summary>
        /// Deserializes the JToken to the <c>TeamMember[]</c>.
        /// </summary>
        /// <param name="data">The JToken.</param>
        /// <returns>Returns the <c>TeamMember[]</c>.</returns>
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

        /// <inheritdoc cref="IApiDeserializable{T}.Deserialize(JToken)"/>
        protected Team Deserialize(JToken data) => ((ITeamService)this).Deserialize(data);

        /// <inheritdoc cref="IApiDeserializable{T}.DeserializeRange(JToken)"/>
        protected Team[] DeserializeRange(JToken data) => ((ITeamService)this).DeserializeRange(data);
    }
}
