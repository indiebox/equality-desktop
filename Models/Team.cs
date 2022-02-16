using System;

using Catel.Data;

namespace Equality.Models
{
    public class Team : ModelBase
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
    }
}
