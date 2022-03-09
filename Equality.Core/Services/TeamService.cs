using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Equality.Services
{
    public class TeamServiceBase<TTeamModel, TTeamMemberModel> : ITeamServiceBase<TTeamModel, TTeamMemberModel>
        where TTeamModel : class, ITeam, new()
        where TTeamMemberModel : class, ITeamMember, new()
    {
        protected IApiClient ApiClient;

        protected ITokenResolverService TokenResolver;

        public TeamServiceBase(IApiClient apiClient, ITokenResolverService tokenResolver)
        {
            Argument.IsNotNull(nameof(apiClient), apiClient);
            Argument.IsNotNull(nameof(tokenResolver), tokenResolver);

            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public async Task<ApiResponseMessage<TTeamModel[]>> GetTeamsAsync()
        {
            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync("teams");

            var teams = DeserializeRange(response.Content["data"]);

            return new(teams, response);
        }

        public async Task<ApiResponseMessage<TTeamModel>> CreateAsync(TTeamModel team)
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

        public Task<ApiResponseMessage<TTeamMemberModel[]>> GetMembersAsync(TTeamModel team) => GetMembersAsync(team.Id);

        public async Task<ApiResponseMessage<TTeamMemberModel[]>> GetMembersAsync(ulong teamId)
        {
            Argument.IsNotNull(nameof(teamId), teamId);

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync($"teams/{teamId}/members");

            var members = DeserializeMembers(response.Content["data"]);

            return new(members, response);
        }

        public Task<ApiResponseMessage<TTeamModel>> SetLogoAsync(TTeamModel team, string imagePath) => SetLogoAsync(team.Id, imagePath);

        public async Task<ApiResponseMessage<TTeamModel>> SetLogoAsync(ulong teamId, string imagePath)
        {
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

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"teams/{teamId}/logo", data);

            var team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        public Task<ApiResponseMessage<TTeamModel>> DeleteLogoAsync(TTeamModel team) => DeleteLogoAsync(team.Id);

        public async Task<ApiResponseMessage<TTeamModel>> DeleteLogoAsync(ulong teamId)
        {
            Argument.IsNotNull(nameof(teamId), teamId);

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).DeleteAsync($"teams/{teamId}/logo");

            var team = Deserialize(response.Content["data"]);

            return new(team, response);
        }

        public Task<ApiResponseMessage> LeaveTeamAsync(TTeamModel team) => LeaveTeamAsync(team.Id);

        public async Task<ApiResponseMessage> LeaveTeamAsync(ulong teamId)
        {
            Argument.IsNotNull(nameof(teamId), teamId);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"teams/{teamId}/leave");
        }

        public async Task<ApiResponseMessage<TTeamModel>> UpdateTeamAsync(TTeamModel team)
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
        public TTeamMemberModel[] DeserializeMembers(JToken data)
        {
            Argument.IsNotNull(nameof(data), data);

            return data.ToObject<TTeamMemberModel[]>(new()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected TTeamModel Deserialize(JToken data) => ((IDeserializeModels<TTeamModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected TTeamModel[] DeserializeRange(JToken data) => ((IDeserializeModels<TTeamModel>)this).DeserializeRange(data);
    }
}
