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

        public double PercentageSupport { get; set; }
    }

    public interface ILeaderNomination<TUserModel, TUserModelCollection> : ILeaderNomination
    {
        public TUserModel Nominated { get; set; }

        public TUserModelCollection Voters { get; set; }
    }
}
