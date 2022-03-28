using System;
using System.Collections.Generic;
using System.Text;

namespace Equality.Models
{
    public interface IColumn
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


        public int Order { get; set; }
    }

    public interface IColumn<TBoard> : IColumn
    {
        public TBoard Board { get; set; }
    }
}
