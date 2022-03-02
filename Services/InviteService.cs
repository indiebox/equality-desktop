﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
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

        /// <inheritdoc cref="IApiDeserializable{T}.Deserialize(JToken)"/>
        protected Invite Deserialize(JToken data) => ((IInviteService)this).Deserialize(data);

        /// <inheritdoc cref="IApiDeserializable{T}.DeserializeRange(JToken)"/>
        protected Invite[] DeserializeRange(JToken data) => ((IInviteService)this).DeserializeRange(data);
    }
}
