using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;
using Equality.Services;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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

            var members = DeserializeProjects(response.Content["data"]);

            return new(members, response);
        }
        /// <summary>
        /// Deserializes the JToken to the <c>IProject[]</c>.
        /// </summary>
        /// <param name="data">The JToken.</param>
        /// <returns>Returns the <c>ITeamMember[]</c>.</returns>
        public Project[] DeserializeProjects(JToken data)
        {
            Argument.IsNotNull(nameof(data), data);

            return data.ToObject<Project[]>(new()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }
    }
}
