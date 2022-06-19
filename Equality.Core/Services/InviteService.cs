using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class InviteServiceBase<TInviteModel> : IInviteServiceBase<TInviteModel>
        where TInviteModel : class, IInvite, new()
    {
        protected IApiClient ApiClient;

        protected ITokenResolver TokenResolver;

        public InviteServiceBase(IApiClient apiClient, ITokenResolver tokenResolver)
        {
            Argument.IsNotNull(nameof(apiClient), apiClient);
            Argument.IsNotNull(nameof(tokenResolver), tokenResolver);

            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public Task<PaginatableApiResponse<TInviteModel>> GetTeamInvitesAsync(ITeam team, QueryParameters query = null)
            => GetTeamInvitesAsync(team.Id, query);

        public async Task<PaginatableApiResponse<TInviteModel>> GetTeamInvitesAsync(ulong teamId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(teamId), teamId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"teams/{teamId}/invites"));

            var invites = Json.Deserialize<TInviteModel[]>(response.Content["data"]);

            return new(invites, response);
        }

        public async Task<PaginatableApiResponse<TInviteModel>> GetUserInvitesAsync(QueryParameters query = null)
        {
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse("invites"));

            var invites = Json.Deserialize<TInviteModel[]>(response.Content["data"]);

            return new(invites, response);
        }

        public Task<ApiResponseMessage<TInviteModel>> InviteUserAsync(ITeam team, string email, QueryParameters query = null)
            => InviteUserAsync(team.Id, email, query);

        public async Task<ApiResponseMessage<TInviteModel>> InviteUserAsync(ulong teamId, string email, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(teamId), teamId, 1);
            Argument.IsNotNullOrWhitespace(nameof(email), email);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "email", email }
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync(query.Parse($"teams/{teamId}/invites"), data);

            var invite = Json.Deserialize<TInviteModel>(response.Content["data"]);

            return new(invite, response);
        }

        public Task<ApiResponseMessage> RevokeInviteAsync(IInvite invite) => RevokeInviteAsync(invite.Id);

        public async Task<ApiResponseMessage> RevokeInviteAsync(ulong inviteId)
        {
            Argument.IsMinimal<ulong>(nameof(inviteId), inviteId, 1);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).DeleteAsync($"invites/{inviteId}");
        }

        public Task<ApiResponseMessage> AcceptInviteAsync(IInvite invite) => AcceptInviteAsync(invite.Id);

        public async Task<ApiResponseMessage> AcceptInviteAsync(ulong inviteId)
        {
            Argument.IsMinimal<ulong>(nameof(inviteId), inviteId, 1);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"invites/{inviteId}/accept");
        }

        public Task<ApiResponseMessage> DeclineInviteAsync(IInvite invite) => DeclineInviteAsync(invite.Id);

        public async Task<ApiResponseMessage> DeclineInviteAsync(ulong inviteId)
        {
            Argument.IsMinimal<ulong>(nameof(inviteId), inviteId, 1);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"invites/{inviteId}/decline");
        }
    }
}
