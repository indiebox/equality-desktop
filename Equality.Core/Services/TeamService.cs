using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Catel;

using Equality.Http;
using Equality.Data;
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

        public async Task<ApiResponseMessage<ITeam[]>> GetTeamsAsync()
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync("teams");

            var teams = DeserializeRange(response.Content["data"]);

            return new(teams, response);
        }

        public async Task<ApiResponseMessage<ITeam>> CreateAsync(ITeam team)
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

        public Task<ApiResponseMessage<ITeamMember[]>> GetMembersAsync(ITeam team) => GetMembersAsync(team.Id);

        public async Task<ApiResponseMessage<ITeamMember[]>> GetMembersAsync(ulong teamId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync($"teams/{teamId}/members");

            var members = DeserializeMembers(response.Content["data"]);

            return new(members, response);
        }

        public Task<ApiResponseMessage<ITeam>> SetLogoAsync(ITeam team, string imagePath) => SetLogoAsync(team.Id, imagePath);

        public async Task<ApiResponseMessage<ITeam>> SetLogoAsync(ulong teamId, string imagePath)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);
            Argument.IsNotNull(nameof(imagePath), imagePath);

            const string fieldName = "logo";

            var fileInfo = new FileInfo(imagePath);
            string mimeType = fileInfo.Extension switch
            {
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                _ => "application/octet-stream",
            };
            using var fileStream = fileInfo.OpenRead();

            var content = new StreamContent(fileStream);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = fieldName,
                FileName = fileInfo.Name,
                FileNameStar = fileInfo.Name,
            };

            Dictionary<string, object> data = new()
            {
                { fieldName, content }
            };

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync($"teams/{teamId}/logo", data);

            var team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        public Task<ApiResponseMessage<ITeam>> DeleteLogoAsync(ITeam team) => DeleteLogoAsync(team.Id);

        public async Task<ApiResponseMessage<ITeam>> DeleteLogoAsync(ulong teamId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).DeleteAsync($"teams/{teamId}/logo");

            var team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        public Task<ApiResponseMessage> LeaveTeamAsync(ITeam team) => LeaveTeamAsync(team.Id);

        public async Task<ApiResponseMessage> LeaveTeamAsync(ulong teamId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync($"teams/{teamId}/leave");
        }

        public async Task<ApiResponseMessage<ITeam>> UpdateTeamAsync(ITeam team)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(team), team);
            Argument.IsMinimal<ulong>("ITeam.Id", team.Id, 1);

            Dictionary<string, object> data = new()
            {
                { "name", team.Name },
                { "description", team.Description },
                { "url", team.Url }
            };

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).PatchAsync($"teams/{team.Id}", data);

            team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        /// <summary>
        /// Deserializes the JToken to the <c>ITeamMember[]</c>.
        /// </summary>
        /// <param name="data">The JToken.</param>
        /// <returns>Returns the <c>ITeamMember[]</c>.</returns>
        public ITeamMember[] DeserializeMembers(JToken data)
        {
            Argument.IsNotNull(nameof(data), data);

            return data.ToObject<ITeamMember[]>(new()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }

        /// <inheritdoc cref="IApiDeserializable{T}.Deserialize(JToken)"/>
        protected ITeam Deserialize(JToken data) => ((ITeamService)this).Deserialize(data);

        /// <inheritdoc cref="IApiDeserializable{T}.DeserializeRange(JToken)"/>
        protected ITeam[] DeserializeRange(JToken data) => ((ITeamService)this).DeserializeRange(data);
    }
}
