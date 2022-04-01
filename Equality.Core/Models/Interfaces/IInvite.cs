using System;

namespace Equality.Models
{
    public interface IInvite
    {
        public enum InviteStatus
        {
            Pending,
            Accepted,
            Declined,
        };

        public ulong Id { get; set; }

        public InviteStatus Status { get; set; }

        public DateTime? AcceptedAt { get; set; }

        public DateTime? DeclinedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public interface IInvite<TTeam, TUser> : IInvite
        where TTeam : class, ITeam, new()
        where TUser : class, IUser, new()
    {
        public TTeam Team { get; set; }

        public TUser Inviter { get; set; }

        public TUser Invited { get; set; }
    }
}
