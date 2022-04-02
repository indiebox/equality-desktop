using System;

using Catel.Data;

namespace Equality.Models
{
    public class Card : ModelBase, ICard
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
