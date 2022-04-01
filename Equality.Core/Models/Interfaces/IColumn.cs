using System;

namespace Equality.Models
{
    public interface IColumn
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public interface IColumn<TBoard> : IColumn
        where TBoard : class, IBoard, new()
    {
        public TBoard Board { get; set; }
    }
}
