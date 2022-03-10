using System;
using System.Collections.Generic;

namespace Equality.Models
{
    public interface ITeam
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Logo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public interface ITeam<TProjectsCollection> : ITeam
        where TProjectsCollection : class, IEnumerable<IProject>
    {
        public TProjectsCollection Projects { get; set; }
    }
}
