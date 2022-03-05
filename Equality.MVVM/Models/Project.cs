using System;

using Catel.Data;

namespace Equality.Models
{
    public class Project : ModelBase, IProject
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public ITeam Team { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

}
