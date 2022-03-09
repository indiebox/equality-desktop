using System;
using System.Collections.ObjectModel;

using Catel.Data;

namespace Equality.Models
{
    public class Team : ModelBase, IEquatable<Team>
    {
        public Team()
        {
        }

        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Logo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Project[] TeamProjects { get; set; }

        #region Override operators

        public static bool operator ==(Team obj1, Team obj2)
        {
            if (ReferenceEquals(obj1, obj2)) {
                return true;
            }

            if (obj1 is null || obj2 is null) {
                return false;
            }

            return obj1.Equals(obj2);
        }
        public static bool operator !=(Team obj1, Team obj2) => !(obj1 == obj2);

        public bool Equals(Team other)
        {
            if (other is null) {
                return false;
            }

            return other.Id != 0
                && other.Id == Id;
        }
        public override bool Equals(object obj) => Equals(obj as Team);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
