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

namespace Equality.Services
{
    public class ProjectServiceBase<TProjectModel, TTeamModel, ILeaderNominationModel> : IProjectServiceBase<TProjectModel, TTeamModel, ILeaderNominationModel>
        where ILeaderNominationModel : class, ILeaderNomination, new()
        where TProjectModel : class, IProject, new()
        where TTeamModel : class, ITeam, new()
    {
        protected IApiClient ApiClient;

        protected ITokenResolverService TokenResolver;

        public ProjectServiceBase(IApiClient apiClient, ITokenResolverService tokenResolver)
        {
            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(TTeamModel team) => GetProjectsAsync(team.Id);

        public async Task<ApiResponseMessage<TProjectModel[]>> GetProjectsAsync(ulong teamId)
        {
            Argument.IsNotNull(nameof(teamId), teamId);

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync($"teams/{teamId}/projects");

            var projects = DeserializeRange(response.Content["data"]);

            return new(projects, response);
        }

        public Task<ApiResponseMessage<ILeaderNominationModel[]>> GetNominatedUsersAsync(TProjectModel project) => GetNominatedUsersAsync(project.Id);

        public async Task<ApiResponseMessage<ILeaderNominationModel[]>> GetNominatedUsersAsync(ulong projectId)
        {
            Argument.IsNotNull(nameof(projectId), projectId);

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync($"projects/{projectId}/leader-nominations");

            var LeaderNominations = DeserializeNominatedUsersRange(response.Content["data"]);

            return new(LeaderNominations, response);
        }

        public Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(TTeamModel team, TProjectModel project) => CreateProjectAsync(team.Id, project);

        public async Task<ApiResponseMessage<TProjectModel>> CreateProjectAsync(ulong teamId, TProjectModel project)
        {
            Argument.IsNotNull(nameof(teamId), teamId);
            Argument.IsNotNull(nameof(project), project);

            Dictionary<string, object> data = new()
            {
                { "name", project.Name },
                { "description", project.Description },
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"teams/{teamId}/projects", data);

            project = Deserialize(response.Content["data"]);

            return new(project, response);
        }

        public Task<ApiResponseMessage<TProjectModel>> SetImageAsync(TProjectModel project, string imagePath) => SetImageAsync(project.Id, imagePath);

        public async Task<ApiResponseMessage<TProjectModel>> SetImageAsync(ulong projectId, string imagePath)
        {
            Argument.IsNotNull(nameof(projectId), projectId);
            Argument.IsNotNull(nameof(imagePath), imagePath);

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

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"projects/{projectId}/image", data);

            var project = Deserialize(response.Content["data"]);

            return new(project, response);
        }

        public Task<ApiResponseMessage<TProjectModel>> DeleteImageAsync(TProjectModel project) => DeleteImageAsync(project.Id);

        public async Task<ApiResponseMessage<TProjectModel>> DeleteImageAsync(ulong projectId)
        {
            Argument.IsNotNull(nameof(projectId), projectId);

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).DeleteAsync($"projects/{projectId}/image");

            var project = Deserialize(response.Content["data"]);

            return new(project, response);
        }

        public async Task<ApiResponseMessage<TProjectModel>> UpdateProjectAsync(TProjectModel project)
        {
            Argument.IsNotNull(nameof(project), project);
            Argument.IsMinimal<ulong>("TProjectModel.Id", project.Id, 1);

            Dictionary<string, object> data = new()
            {
                { "name", project.Name },
                { "description", project.Description },
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PatchAsync($"projects/{project.Id}", data);

            project = Deserialize(response.Content["data"]);

            return new(project, response);
        }

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected TProjectModel Deserialize(JToken data) => ((IDeserializeModels<TProjectModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected TProjectModel[] DeserializeRange(JToken data) => ((IDeserializeModels<TProjectModel>)this).DeserializeRange(data);

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected ILeaderNominationModel DeserializeNominatedUsers(JToken data) => ((IDeserializeModels<ILeaderNominationModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected ILeaderNominationModel[] DeserializeNominatedUsersRange(JToken data) => ((IDeserializeModels<ILeaderNominationModel>)this).DeserializeRange(data);

    }
}
