using System.Collections.Generic;

namespace Equality.Models
{
    public interface ILeaderNomination
    {
        public int Count { get; set; }
    }

    public interface ILeaderNomination<TTeamModel, TTeamMembersCollection> : ILeaderNomination
        where TTeamMembersCollection : class, IEnumerable<ITeamMember>
    {
        public TTeamModel Nominated { get; set; }

        public TTeamMembersCollection Voters { get; set; }
    }
}
