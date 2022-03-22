using System;
using System.Collections.Generic;
using System.Text;

using Catel.Data;

namespace Equality.Models
{
    public class Board : ModelBase, IBoard
    {
        public Board()
        {
        }

        public ulong Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
