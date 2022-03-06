using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using InviteFilter = Equality.Core.Services.IInviteService.InviteFilter;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class InviteService : Core.Services.InviteService, IInviteService
    {
        public InviteService(IApiClient apiClient, Core.Services.ITokenResolverService tokenResolver) : base(apiClient, tokenResolver)
        {
        }

        public Task<ApiResponseMessage<Invite[]>> GetTeamInvitesAsync(Team team, InviteFilter filter = InviteFilter.All)
            => GetTeamInvitesAsync(team.Id, filter);

        public new async Task<ApiResponseMessage<Invite[]>> GetTeamInvitesAsync(ulong teamId, InviteFilter filter = InviteFilter.All)
        {
            var response = await base.GetTeamInvitesAsync(teamId);

            var invites = DeserializeRange(response.Content["data"]);

            return new(invites, response);
        }

        public new async Task<ApiResponseMessage<Invite[]>> GetUserInvitesAsync()
        {
            var response = await base.GetUserInvitesAsync();

            var invites = DeserializeRange(response.Content["data"]);

            return new(invites, response);
        }

        public Task<ApiResponseMessage<Invite>> InviteUserAsync(Team team, string email) => InviteUserAsync(team.Id, email);

        public new async Task<ApiResponseMessage<Invite>> InviteUserAsync(ulong teamId, string email)
        {
            var response = await base.InviteUserAsync(teamId, email);

            var invite = Deserialize(response.Content["data"]);

            return new(invite, response);
        }

        public Task<ApiResponseMessage> RevokeInviteAsync(Invite invite) => RevokeInviteAsync(invite.Id);

        public Task<ApiResponseMessage> AcceptInviteAsync(Invite invite) => AcceptInviteAsync(invite.Id);

        public Task<ApiResponseMessage> DeclineInviteAsync(Invite invite) => DeclineInviteAsync(invite.Id);

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.Deserialize(JToken)"/>
        protected Invite Deserialize(JToken data) => ((IInviteService)this).Deserialize(data);

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected Invite[] DeserializeRange(JToken data) => ((IInviteService)this).DeserializeRange(data);
    }
}
