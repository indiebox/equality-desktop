using System;
using System.Collections.Generic;
using System.Text;

using Equality.Models;

namespace Equality.Core.ApiClient.Interfaces
{
    public interface IStateManager
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
