using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class TeamServiceBase<TTeamModel, TTeamMemberModel> : ITeamServiceBase<TTeamModel, TTeamMemberModel>
        where TTeamModel : class, ITeam, new()
        where TTeamMemberModel : class, ITeamMember, new()
    {
        protected IApiClient ApiClient;

        protected ITokenResolver TokenResolver;

        public TeamServiceBase(IApiClient apiClient, ITokenResolver tokenResolver)
        {
            Argument.IsNotNull(nameof(apiClient), apiClient);
            Argument.IsNotNull(nameof(tokenResolver), tokenResolver);

            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public async Task<PaginatableApiResponse<TTeamModel>> GetTeamsAsync(QueryParameters query = null)
        {
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse("teams"));

            var teams = Json.Deserialize<TTeamModel[]>(response.Content["data"]);

            return new(teams, response);
        }

        public async Task<ApiResponseMessage<TTeamModel>> GetTeamAsync(ulong teamId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(teamId), teamId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"teams/{teamId}"));

            var team = Json.Deserialize<TTeamModel>(response.Content["data"]);

            return new(team, response);
        }

        public async Task<ApiResponseMessage<TTeamModel>> CreateAsync(ITeam team, QueryParameters query = null)
        {
            Argument.IsNotNull(nameof(team), team);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", team.Name },
                { "description", team.Description },
                { "url", team.Url }
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync(query.Parse("teams"), data);
            var deserializedTeam = Json.Deserialize<TTeamModel>(response.Content["data"]);

            return new(deserializedTeam, response);
        }

        public Task<PaginatableApiResponse<TTeamMemberModel>> GetMembersAsync(ITeam team, QueryParameters query = null)
            => GetMembersAsync(team.Id, query);

        public async Task<PaginatableApiResponse<TTeamMemberModel>> GetMembersAsync(ulong teamId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(teamId), teamId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"teams/{teamId}/members"));
            var members = Json.Deserialize<TTeamMemberModel[]>(response.Content["data"]);

            return new(members, response);
        }

        public Task<ApiResponseMessage> LeaveTeamAsync(ITeam team) => LeaveTeamAsync(team.Id);

        public async Task<ApiResponseMessage> LeaveTeamAsync(ulong teamId)
        {
            Argument.IsMinimal<ulong>(nameof(teamId), teamId, 1);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"teams/{teamId}/leave");
        }

        public Task<ApiResponseMessage<TTeamModel>> SetLogoAsync(ITeam team, string imagePath, QueryParameters query = null)
            => SetLogoAsync(team.Id, imagePath, query);

        public async Task<ApiResponseMessage<TTeamModel>> SetLogoAsync(ulong teamId, string imagePath, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(teamId), teamId, 1);
            Argument.IsNotNull(nameof(imagePath), imagePath);
            query ??= new QueryParameters();

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

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync(query.Parse($"teams/{teamId}/logo"), data);

            var team = Json.Deserialize<TTeamModel>(response.Content["data"]);

            return new(team, response);
        }

        public Task<ApiResponseMessage> DeleteLogoAsync(ITeam team) => DeleteLogoAsync(team.Id);

        public async Task<ApiResponseMessage> DeleteLogoAsync(ulong teamId)
        {
            Argument.IsMinimal<ulong>(nameof(teamId), teamId, 1);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).DeleteAsync($"teams/{teamId}/logo");
        }

        public async Task<ApiResponseMessage<TTeamModel>> UpdateTeamAsync(ITeam team, QueryParameters query = null)
        {
            Argument.IsNotNull(nameof(team), team);
            Argument.IsMinimal<ulong>("team.Id", team.Id, 1);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", team.Name },
                { "description", team.Description },
                { "url", team.Url }
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PatchAsync(query.Parse($"teams/{team.Id}"), data);
            var deserializedTeam = Json.Deserialize<TTeamModel>(response.Content["data"]);

            return new(deserializedTeam, response);
        }
    }
}
