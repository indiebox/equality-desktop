using Equality.Http;
using Equality.Models;

using Equality.Data;

namespace Equality.Services
{
    public class InviteService : InviteServiceBase<Invite>, IInviteService
    {
        public InviteService(IApiClient apiClient, ITokenResolver tokenResolver) : base(apiClient, tokenResolver)
        {
        }
    }
}
