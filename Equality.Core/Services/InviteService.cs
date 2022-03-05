using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Http;
using Equality.StateManager;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class InviteService : IInviteService
    {
        protected IApiClient ApiClient;

        protected IStateManager StateManager;

        public InviteService(IApiClient apiClient, IStateManager stateManager)
        {
            ApiClient = apiClient;
            StateManager = stateManager;
        }

        public Task<ApiResponseMessage<Invite[]>> GetTeamInvitesAsync
            (Team team, IInviteService.InviteFilter filter = IInviteService.InviteFilter.All) => GetTeamInvitesAsync(team.Id, filter);

        public async Task<ApiResponseMessage<Invite[]>> GetTeamInvitesAsync(ulong teamId, IInviteService.InviteFilter filter = IInviteService.InviteFilter.All)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            var query = ApiClient.BuildUri($"teams/{teamId}/invites", new()
            {
                { "filter", filter.ToString().ToLower() }
            });

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync(query);

            var invites = DeserializeRange(response.Content["data"]);

            return new(invites, response);
        }

        public async Task<ApiResponseMessage<Invite[]>> GetUserInvitesAsync()
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync("invites");

            var invites = DeserializeRange(response.Content["data"]);

            return new(invites, response);
        }

        public Task<ApiResponseMessage<Invite>> InviteUserAsync(Team team, string email) => InviteUserAsync(team.Id, email);

        public async Task<ApiResponseMessage<Invite>> InviteUserAsync(ulong teamId, string email)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(teamId), teamId);

            Dictionary<string, object> data = new()
            {
                { "email", email }
            };

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync($"teams/{teamId}/invites", data);

            var invite = Deserialize(response.Content["data"]);

            return new(invite, response);
        }

        public Task<ApiResponseMessage> RevokeInviteAsync(Invite invite) => RevokeInviteAsync(invite.Id);

        public async Task<ApiResponseMessage> RevokeInviteAsync(ulong inviteId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(inviteId), inviteId);

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).DeleteAsync($"invites/{inviteId}");
        }

        public Task<ApiResponseMessage> AcceptInviteAsync(Invite invite) => AcceptInviteAsync(invite.Id);

        public async Task<ApiResponseMessage> AcceptInviteAsync(ulong inviteId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(inviteId), inviteId);

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync($"invites/{inviteId}/accept");
        }

        public Task<ApiResponseMessage> DeclineInviteAsync(Invite invite) => DeclineInviteAsync(invite.Id);

        public async Task<ApiResponseMessage> DeclineInviteAsync(ulong inviteId)
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);
            Argument.IsNotNull(nameof(inviteId), inviteId);

            return await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync($"invites/{inviteId}/decline");
        }

        /// <inheritdoc cref="IApiDeserializable{T}.Deserialize(JToken)"/>
        protected Invite Deserialize(JToken data) => ((IInviteService)this).Deserialize(data);

        /// <inheritdoc cref="IApiDeserializable{T}.DeserializeRange(JToken)"/>
        protected Invite[] DeserializeRange(JToken data) => ((IInviteService)this).DeserializeRange(data);
    }
}
