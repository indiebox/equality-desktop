using System;
using System.Collections.Generic;
using System.Text;

namespace Equality.Models
{
    public interface IBoard
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
