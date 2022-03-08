using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<ApiResponseMessage> GetMembersAsync() => await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync("members");

    }
}
