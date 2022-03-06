using System;

using Catel.Data;

namespace Equality.Models
{
    public class Project : ModelBase, IEquatable<Project>
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public Team Team { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #region Override operators

        public static bool operator ==(Project obj1, Project obj2)
        {
            if (ReferenceEquals(obj1, obj2)) {
                return true;
            }

            if (obj1 is null || obj2 is null) {
                return false;
            }

            return obj1.Equals(obj2);
        }
        public static bool operator !=(Project obj1, Project obj2) => !(obj1 == obj2);

        public bool Equals(Project other)
        {
            if (other is null) {
                return false;
            }

            return other.Id != 0
                && other.Id == Id;
        }
        public override bool Equals(object obj) => Equals(obj as Project);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }

}
