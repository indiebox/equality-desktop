﻿using System;
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
    }
}