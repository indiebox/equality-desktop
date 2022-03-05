using System;

using Catel.Data;

namespace Equality.Models
{
    public class Invite : ModelBase, IInvite
    {
        public Invite()
        {
        }

        public ulong Id { get; set; }

        public ITeam Team { get; set; }

        public IUser Inviter { get; set; }

        public IUser Invited { get; set; }

        public IInvite.InviteStatus Status { get; set; }

        public DateTime? AcceptedAt { get; set; }

        public DateTime? DeclinedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
