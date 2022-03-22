using System;
using System.Collections.Generic;
using System.Text;

namespace Equality.Models
{
    public interface IBoard
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
