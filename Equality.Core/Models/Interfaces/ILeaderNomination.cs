using System.Collections.Generic;

namespace Equality.Models
{
    public interface ILeaderNomination
    {
        public bool IsLeader { get; set; }

        public int VotersCount { get; set; }
    }

    public interface ILeaderNomination<TTeamMemberModel, TTeamMembersCollection> : ILeaderNomination
        where TTeamMemberModel : class, ITeamMember, new()
        where TTeamMembersCollection : class, IEnumerable<TTeamMemberModel>, new()
    {
        public TTeamMemberModel Nominated { get; set; }

        public TTeamMembersCollection Voters { get; set; }
    }
}
