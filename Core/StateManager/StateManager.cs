using Equality.Models;

namespace Equality.Core.StateManager
{
    public class StateManager : IStateManager
    {
        public StateManager()
        {
        }

        public User CurrentUser { get; set; }

        public string ApiToken { get; set; }
    }
}
