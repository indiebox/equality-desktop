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

        public ITeam Team { get; set; }

        public IUser Inviter { get; set; }

        public IUser Invited { get; set; }

        public InviteStatus Status { get; set; }

        public DateTime? AcceptedAt { get; set; }

        public DateTime? DeclinedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
