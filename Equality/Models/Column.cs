using System;
using System.Collections.ObjectModel;

using Catel.Data;

namespace Equality.Models
{
    public class Column : ModelBase, IColumn<Board, ObservableCollection<Card>>
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #region Relations

        public Board Board { get; set; }

        public ObservableCollection<Card> Cards { get; set; } = new();

        #endregion
    }
}
