using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Catel.Data;

using Equality.Models;

namespace Equality.Models
{
    public class LeaderNomination : ModelBase
    {
        public LeaderNomination()
        {
        }

        public User User { get; set; }

        public int Count { get; set; }

        public int PercentageSupport { get; set; }
    }
}
