using System;
using System.Collections.Generic;
using System.Text;

using Catel.Data;

namespace Equality.Models
{
    public class Column : ModelBase, IColumn<Board>
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #region Relations

        public Board Board { get; set; }

        #endregion
    }
}
