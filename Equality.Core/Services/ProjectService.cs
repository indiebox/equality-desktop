using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Core.Services
{
    public class ProjectService : IProjectService
    {
        protected IApiClient ApiClient;

        protected ITokenResolverService TokenResolver;

        public ProjectService(IApiClient apiClient, ITokenResolverService tokenResolver)
        {
            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public Task<ApiResponseMessage> GetProjectsAsync(ITeam team) => GetProjectsAsync(team.Id);

        public async Task<ApiResponseMessage> GetProjectsAsync(ulong teamId)
        {
            Argument.IsNotNull(nameof(teamId), teamId);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync($"teams/{teamId}/projects");
        }

        public Task<ApiResponseMessage> CreateProjectAsync(ITeam team, IProject project) => CreateProjectAsync(team.Id, project);

        public async Task<ApiResponseMessage> CreateProjectAsync(ulong teamId, IProject project)
        {
            Argument.IsNotNull(nameof(teamId), teamId);
            Argument.IsNotNull(nameof(project), project);

            Dictionary<string, object> data = new()
            {
                { "name", project.Name },
                { "description", project.Description },
            };

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"teams/{teamId}/projects", data);
        }

        public Task<ApiResponseMessage> SetImageAsync(IProject project, string imagePath) => SetImageAsync(project.Id, imagePath);

        public async Task<ApiResponseMessage> SetImageAsync(ulong projectId, string imagePath)
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

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"projects/{projectId}/image", data);
        }

        public Task<ApiResponseMessage> DeleteImageAsync(IProject project) => DeleteImageAsync(project.Id);

        public async Task<ApiResponseMessage> DeleteImageAsync(ulong projectId)
        {
            Argument.IsNotNull(nameof(projectId), projectId);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).DeleteAsync($"projects/{projectId}/image");
        }

        public async Task<ApiResponseMessage> UpdateProjectAsync(IProject project)
        {
            Argument.IsNotNull(nameof(project), project);
            Argument.IsMinimal<ulong>("IProject.Id", project.Id, 1);

            Dictionary<string, object> data = new()
            {
                { "name", project.Name },
                { "description", project.Description },
            };

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PatchAsync($"projects/{project.Id}", data);
        }
    }
}
