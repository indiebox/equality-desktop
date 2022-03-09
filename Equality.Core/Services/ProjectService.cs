using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class ProjectServiceBase<TProjectModel, TTeamModel> : IProjectServiceBase<TProjectModel, TTeamModel>
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

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected TProjectModel Deserialize(JToken data) => ((IDeserializeModels<TProjectModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected TProjectModel[] DeserializeRange(JToken data) => ((IDeserializeModels<TProjectModel>)this).DeserializeRange(data);
    }
}
