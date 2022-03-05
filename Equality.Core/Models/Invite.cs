using System;

using Catel.Data;

namespace Equality.Models
{
    public class Invite : ModelBase
    {
        public enum InviteStatus
        {
            Pending,
            Accepted,
            Declined,
        };

        public Invite()
        {
        }

        public ulong Id { get; set; }

        public Team Team { get; set; }

        public User Inviter { get; set; }

        public User Invited { get; set; }

        public InviteStatus Status { get; set; }

        public DateTime? AcceptedAt { get; set; }

        public DateTime? DeclinedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
