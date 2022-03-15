using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Equality.Data;

namespace Equality.Models
{
    public interface ILeaderNomination
    {
        public int Count { get; set; }

        public int PercentageSupport { get; set; }

        public bool IsCurrentUserVotes();
    }

    public interface ILeaderNomination<TUserModel> : ILeaderNomination
    {
        public TUserModel User { get; set; }

        public ObservableCollection<TUserModel> Electorate { get; set; }
    }
}
