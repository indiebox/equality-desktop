using System.Threading.Tasks;

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

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.Deserialize(JToken)"/>
        protected Project Deserialize(JToken data) => ((IProjectService)this).Deserialize(data);

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected Project[] DeserializeRange(JToken data) => ((IProjectService)this).DeserializeRange(data);
    }
}
