using System;

namespace Equality.Models
{
    public interface IUser
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
