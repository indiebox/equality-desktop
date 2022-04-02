using System;

namespace Equality.Models
{
    public interface ICard
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
