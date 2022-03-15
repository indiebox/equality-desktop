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

        public User User { get; set; }

        public int Count { get; set; }

        public ObservableCollection<User> Electorate { get; set; }

        public int PercentageSupport { get; set; }

        public bool IsCurrentUserVotes()
        {
            foreach (var member in Electorate) {
                if (member == StateManager.CurrentUser) {
                    return true;
                }
            }
            return false;
        }
    }
}
