using Equality.Models;

namespace Equality.Services
{
    public interface IInviteService : IInviteServiceBase<Invite, Team>
    {
        public new enum InviteFilter
        {
            All,
            Pending,
            Accepted,
            Declined,
        };
    }
}
