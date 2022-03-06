using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Core.Services
{
    public class InviteService : IInviteService
    {
        protected IApiClient ApiClient;

        protected ITokenResolverService TokenResolver;

        public InviteService(IApiClient apiClient, ITokenResolverService tokenResolver)
        {
            Argument.IsNotNull(nameof(apiClient), apiClient);
            Argument.IsNotNull(nameof(tokenResolver), tokenResolver);

            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public Task<ApiResponseMessage> GetTeamInvitesAsync
            (ITeam team, IInviteService.InviteFilter filter = IInviteService.InviteFilter.All)
            => GetTeamInvitesAsync(team.Id, filter);

        public async Task<ApiResponseMessage> GetTeamInvitesAsync(ulong teamId, IInviteService.InviteFilter filter = IInviteService.InviteFilter.All)
        {
            Argument.IsNotNull(nameof(teamId), teamId);

            var query = ApiClient.BuildUri($"teams/{teamId}/invites", new()
            {
                { "filter", filter.ToString().ToLower() }
            });

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query);
        }

        public async Task<ApiResponseMessage> GetUserInvitesAsync()
            => await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync("invites");

        public Task<ApiResponseMessage> InviteUserAsync(ITeam team, string email) => InviteUserAsync(team.Id, email);

        public async Task<ApiResponseMessage> InviteUserAsync(ulong teamId, string email)
        {
            Argument.IsNotNull(nameof(teamId), teamId);

            Dictionary<string, object> data = new()
            {
                { "email", email }
            };

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"teams/{teamId}/invites", data);
        }

        public Task<ApiResponseMessage> RevokeInviteAsync(IInvite invite) => RevokeInviteAsync(invite.Id);

        public async Task<ApiResponseMessage> RevokeInviteAsync(ulong inviteId)
        {
            Argument.IsNotNull(nameof(inviteId), inviteId);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).DeleteAsync($"invites/{inviteId}");
        }

        public Task<ApiResponseMessage> AcceptInviteAsync(IInvite invite) => AcceptInviteAsync(invite.Id);

        public async Task<ApiResponseMessage> AcceptInviteAsync(ulong inviteId)
        {
            Argument.IsNotNull(nameof(inviteId), inviteId);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"invites/{inviteId}/accept");
        }

        public Task<ApiResponseMessage> DeclineInviteAsync(IInvite invite) => DeclineInviteAsync(invite.Id);

        public async Task<ApiResponseMessage> DeclineInviteAsync(ulong inviteId)
        {
            Argument.IsNotNull(nameof(inviteId), inviteId);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"invites/{inviteId}/decline");
        }
    }
}
