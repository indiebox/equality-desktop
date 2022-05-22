using System.Collections.ObjectModel;

using Catel.Data;

namespace Equality.Models
{
    public class LeaderNomination : ModelBase, ILeaderNomination<TeamMember, ObservableCollection<TeamMember>>
    {
        public LeaderNomination()
        {
        }

        public bool IsLeader { get; set; }

        public int VotersCount { get; set; }

        #region Relations

        public TeamMember Nominated { get; set; }

        public ObservableCollection<TeamMember> Voters { get; set; }

        #endregion

        #region Custom properties

        public double PercentageSupport { get; set; }

        public bool IsCurrentUserVotes { get; set; } = false;

        #endregion
    }
}
