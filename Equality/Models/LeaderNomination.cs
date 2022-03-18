using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Catel.Data;

using Equality.Data;
using Equality.Models;

namespace Equality.Models
{
    public class LeaderNomination : ModelBase, ILeaderNomination<User>
    {
        public LeaderNomination()
        {
        }

        public User Nominated { get; set; }

        public int Count { get; set; }

        public ObservableCollection<User> Voters { get; set; }

        public double PercentageSupport { get; set; }

        public bool IsCurrentUserVotes { get; set; } = false;
    }
}
