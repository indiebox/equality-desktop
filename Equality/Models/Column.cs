using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Catel.Data;

namespace Equality.Models
{
    public class Column : ModelBase, IColumn<Board, ObservableCollection<Card>>
    {
        public Column()
        {
            Cards.CollectionChanged += (s, e) => RaisePropertyChanged(nameof(IsCardsLimitReached));
        }

        public ulong Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #region Relations

        public Board Board { get; set; }

        public ObservableCollection<Card> Cards { get; set; } = new();

        #endregion

        #region Custom properties

        public bool IsCardsLimitReached => Cards.Count >= 5;

        /// <summary>
        /// The list of messages why we cant move current draggable card to this column.
        /// This property setup when we start dragging card and clears when we stop dragging card.
        /// </summary>
        public ObservableCollection<string> CantMoveCardMessages { get; set; } = new();

        #endregion
    }
}
