using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Catel;

using Equality.Http;
using Equality.Data;
using Equality.Models;

namespace Equality.Core.Services
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

        public async Task<ApiResponseMessage> GetTeamsAsync()
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync("teams");
        }

        public async Task<ApiResponseMessage> CreateAsync(ITeam team)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(team), team);

            Dictionary<string, object> data = new()
            {
                { "name", team.Name },
                { "description", team.Description },
                { "url", team.Url }
            };

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync("teams", data);
        }

        public Task<ApiResponseMessage> GetMembersAsync(ITeam team) => GetMembersAsync(team.Id);

        public async Task<ApiResponseMessage> GetMembersAsync(ulong teamId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync($"teams/{teamId}/members");
        }

        public Task<ApiResponseMessage> SetLogoAsync(ITeam team, string imagePath) => SetLogoAsync(team.Id, imagePath);

        public async Task<ApiResponseMessage> SetLogoAsync(ulong teamId, string imagePath)
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

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync($"teams/{teamId}/logo", data);
        }

        public Task<ApiResponseMessage> DeleteLogoAsync(ITeam team) => DeleteLogoAsync(team.Id);

        public async Task<ApiResponseMessage> DeleteLogoAsync(ulong teamId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).DeleteAsync($"teams/{teamId}/logo");
        }

        public Task<ApiResponseMessage> LeaveTeamAsync(ITeam team) => LeaveTeamAsync(team.Id);

        public async Task<ApiResponseMessage> LeaveTeamAsync(ulong teamId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync($"teams/{teamId}/leave");
        }

        public async Task<ApiResponseMessage> UpdateTeamAsync(ITeam team)
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

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).PatchAsync($"teams/{team.Id}", data);
        }
    }
}
