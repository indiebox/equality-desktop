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
    public partial class ProjectServiceBase<TProjectModel, TLeaderNominationModel, TUserModel> : IProjectServiceBase<TProjectModel, TLeaderNominationModel, TUserModel>
        where TProjectModel : class, IProject, new()
        where TLeaderNominationModel : class, ILeaderNomination, new()
        where TUserModel : class, IUser, new()
    {
        protected IApiClient ApiClient;

        protected ITokenResolver TokenResolver;

        public ProjectServiceBase(IApiClient apiClient, ITokenResolver tokenResolver)
        {
            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public Task<PaginatableApiResponse<TProjectModel>> GetProjectsAsync(ITeam team, QueryParameters query = null)
            => GetProjectsAsync(team.Id, query);

        public async Task<PaginatableApiResponse<TProjectModel>> GetProjectsAsync(ulong teamId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(teamId), teamId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"teams/{teamId}/projects"));

            var projects = Json.Deserialize<TProjectModel[]>(response.Content["data"]);

            return new(projects, response);
        }

        public async Task<ApiResponseMessage<TProjectModel>> GetProjectAsync(ulong projectId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(projectId), projectId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"projects/{projectId}"));

            var project = Json.Deserialize<TProjectModel>(response.Content["data"]);

            return new(project, response);
        }

        public Task<ApiResponseMessage<TLeaderNominationModel[]>> GetNominatedUsersAsync(IProject project, QueryParameters query = null)
            => GetNominatedUsersAsync(project.Id, query);

        public async Task<ApiResponseMessage<TLeaderNominationModel[]>> GetNominatedUsersAsync(ulong projectId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(projectId), projectId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"projects/{projectId}/leader-nominations"));

            var nominations = Json.Deserialize<TLeaderNominationModel[]>(response.Content["data"]);

            return new(nominations, response);
        }

        public Task<ApiResponseMessage<TLeaderNominationModel[]>> NominateUserAsync(IProject project, IUser user, QueryParameters query = null)
            => NominateUserAsync(project.Id, user.Id, query);

        public async Task<ApiResponseMessage<TLeaderNominationModel[]>> NominateUserAsync(ulong projectId, ulong userId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(projectId), projectId, 1);
            Argument.IsMinimal<ulong>(nameof(userId), userId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .PostAsync(query.Parse($"projects/{projectId}/leader-nominations/{userId}"));

            var nominations = Json.Deserialize<TLeaderNominationModel[]>(response.Content["data"]);

            return new(nominations, response);
        }

        public Task<ApiResponseMessage<TUserModel>> GetProjectLeaderAsync(IProject project, QueryParameters query = null)
            => GetProjectLeaderAsync(project.Id, query);

        public async Task<ApiResponseMessage<TUserModel>> GetProjectLeaderAsync(ulong projectId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(projectId), projectId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"projects/{projectId}/leader"));

            var leader = Json.Deserialize<TUserModel>(response.Content["data"]);

            return new(leader, response);
        }

        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(ITeam team, IProject project, QueryParameters query = null)
            => CreateProjectAsync(team.Id, project, query);

        public async Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(ulong teamId, IProject project, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(teamId), teamId, 1);
            Argument.IsNotNull(nameof(project), project);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", project.Name },
                { "description", project.Description },
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync(query.Parse($"teams/{teamId}/projects"), data);
            var deserializedProject = Json.Deserialize<TProjectModel>(response.Content["data"]);

            return new(deserializedProject, response);
        }

        public Task<ApiResponseMessage<TProjectModel>> SetImageAsync(IProject project, string imagePath, QueryParameters query = null)
            => SetImageAsync(project.Id, imagePath, query);

        public async Task<ApiResponseMessage<TProjectModel>> SetImageAsync(ulong projectId, string imagePath, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(projectId), projectId, 1);
            Argument.IsNotNull(nameof(imagePath), imagePath);
            query ??= new QueryParameters();

            const string fieldName = "image";

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

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync(query.Parse($"projects/{projectId}/image"), data);

            var project = Json.Deserialize<TProjectModel>(response.Content["data"]);

            return new(project, response);
        }

        public Task<ApiResponseMessage> DeleteImageAsync(IProject project) => DeleteImageAsync(project.Id);

        public async Task<ApiResponseMessage> DeleteImageAsync(ulong projectId)
        {
            Argument.IsMinimal<ulong>(nameof(projectId), projectId, 1);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).DeleteAsync($"projects/{projectId}/image");
        }

        public async Task<ApiResponseMessage<TProjectModel>> UpdateProjectAsync(IProject project, QueryParameters query = null)
        {
            Argument.IsNotNull(nameof(project), project);
            Argument.IsMinimal<ulong>("project.Id", project.Id, 1);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", project.Name },
                { "description", project.Description },
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PatchAsync(query.Parse($"projects/{project.Id}"), data);
            var deserializedProject = Json.Deserialize<TProjectModel>(response.Content["data"]);

            return new(deserializedProject, response);
        }
    }
}
