using System.Collections.Generic;

namespace Equality.Models
{
    public interface ILeaderNomination
    {
        public bool IsLeader { get; set; }

        public int VotersCount { get; set; }
    }

    public interface ILeaderNomination<TTeamMember, TTeamMembersCollection> : ILeaderNomination
        where TTeamMember : class, ITeamMember, new()
        where TTeamMembersCollection : class, IEnumerable<TTeamMember>, new()
    {
        public TTeamMember Nominated { get; set; }

        public TTeamMembersCollection Voters { get; set; }
    }
}
