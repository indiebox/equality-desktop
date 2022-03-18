using System.Collections.ObjectModel;

using Catel.Data;

namespace Equality.Models
{
    public class LeaderNomination : ModelBase, ILeaderNomination<TeamMember, ObservableCollection<TeamMember>>
    {
        public LeaderNomination()
        {
        }

        public int Count { get; set; }

        #region Custom properties

        public double PercentageSupport { get; set; }

        public bool IsCurrentUserVotes { get; set; } = false;

        #endregion

        #region Relations

        public TeamMember Nominated { get; set; }

        public ObservableCollection<TeamMember> Voters { get; set; }

        #endregion
    }
}
