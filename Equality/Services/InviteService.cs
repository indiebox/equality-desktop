using Equality.Http;
using Equality.Models;

using Equality.Data;

namespace Equality.Services
{
    public class InviteService : InviteServiceBase<Invite, Team>, IInviteService
    {
        public InviteService(IApiClient apiClient, ITokenResolverService tokenResolver) : base(apiClient, tokenResolver)
        {
        }
    }
}
