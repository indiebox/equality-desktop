using Equality.Core.ApiClient.Interfaces;
using Equality.Models;

namespace Equality.Core.ApiClient
{
    public class StateManager : IStateManager
    {
        public User User { get; set; }
        public string Token { get; set; }

        public StateManager(User user = null, string token = "")
        {
            User = user;
            Token = token;
        }
    }
}
