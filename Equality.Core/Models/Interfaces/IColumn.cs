using System;
using System.Collections.Generic;

namespace Equality.Models
{
    public interface IColumn
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public interface IColumn<TBoard, TCardsCollection> : IColumn
        where TBoard : class, IBoard, new()
        where TCardsCollection : class, IEnumerable<ICard>, new()
    {
        public TBoard Board { get; set; }

        public TCardsCollection Cards { get; set; }
    }
}
