using System;

using Catel.Data;

namespace Equality.Models
{
    public class Invite : ModelBase, IInvite<Team, User>, IEquatable<Invite>
    {
        public Invite()
        {
        }

        public ulong Id { get; set; }

        public Team Team { get; set; }

        public User Inviter { get; set; }

        public User Invited { get; set; }

        public IInvite.InviteStatus Status { get; set; }

        public DateTime? AcceptedAt { get; set; }

        public DateTime? DeclinedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #region Override operators

        public static bool operator ==(Invite obj1, Invite obj2)
        {
            if (ReferenceEquals(obj1, obj2)) {
                return true;
            }

            if (obj1 is null || obj2 is null) {
                return false;
            }

            return obj1.Equals(obj2);
        }
        public static bool operator !=(Invite obj1, Invite obj2) => !(obj1 == obj2);

        public bool Equals(Invite other)
        {
            if (other is null) {
                return false;
            }

            return other.Id != 0
                && other.Id == Id;
        }
        public override bool Equals(object obj) => Equals(obj as Invite);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
