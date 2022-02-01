using System;
using System.Collections.Generic;
using System.Text;

namespace Equality.Models.Interfaces
{
    public interface IUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
