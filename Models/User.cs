using System;

using Catel.Data;

namespace Equality.Models
{
    public class User : ModelBase
    {
        public User()
        {
        }

        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public bool IsVerified => EmailVerifiedAt != null;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}