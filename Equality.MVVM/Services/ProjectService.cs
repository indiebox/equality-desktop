using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class ProjectService : Core.Services.ProjectService, IProjectService
    {
        public ProjectService(IApiClient apiClient, ITokenResolverService tokenResolver) : base(apiClient, tokenResolver)
        {
        }

        public Task<ApiResponseMessage<Project[]>> GetProjectsAsync(Team team) => GetProjectsAsync(team.Id);

        public new async Task<ApiResponseMessage<Project[]>> GetProjectsAsync(ulong teamId)
        {
            var response = await base.GetProjectsAsync(teamId);

            var projects = DeserializeRange(response.Content["data"]);

            return new(projects, response);
        }

        public Task<ApiResponseMessage<Project>> CreateProjectAsync(Team team, Project project) => CreateProjectAsync(team.Id, project);

        public async Task<ApiResponseMessage<Project>> CreateProjectAsync(ulong teamId, Project project)
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

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.Deserialize(JToken)"/>
        protected Project Deserialize(JToken data) => ((IProjectService)this).Deserialize(data);

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected Project[] DeserializeRange(JToken data) => ((IProjectService)this).DeserializeRange(data);
    }
}
