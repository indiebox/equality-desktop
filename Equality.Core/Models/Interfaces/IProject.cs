using System;

namespace Equality.Models
{
    public interface IProject
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public interface IProject<TTeam> : IProject
        where TTeam : class, ITeam, new()
    {
        public TTeam Team { get; set; }
    }
}
