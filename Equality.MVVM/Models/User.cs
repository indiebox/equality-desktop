﻿using System;

using Catel.Data;
using Catel.IoC;

using Equality.Core.Services;

namespace Equality.Models
{
    public class User : ModelBase, IModelWithId, IEquatable<User>
    {
        public User()
        {
        }

        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #region Custom properties

        public bool IsVerified => EmailVerifiedAt != null;

        public bool IsCurrentUser => this == (User)this.GetDependencyResolver().TryResolve<ITokenResolverService>()?.ResolveCurrentUser();

        #endregion

        #region Override operators

        public static bool operator ==(User obj1, User obj2)
        {
            if (ReferenceEquals(obj1, obj2)) {
                return true;
            }

            if (obj1 is null || obj2 is null) {
                return false;
            }

            return obj1.Equals(obj2);
        }
        public static bool operator !=(User obj1, User obj2) => !(obj1 == obj2);

        public bool Equals(User other)
        {
            if (other is null) {
                return false;
            }

            return other.Id != 0
                && other.Id == Id;
        }
        public override bool Equals(object obj) => Equals(obj as User);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}