using System.Collections.Generic;
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
    }
}
