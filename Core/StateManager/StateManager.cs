using Equality.Models;

namespace Equality.Core.StateManager
{
    public class StateManager : IStateManager
    {
        public User CurrentUser { get; set; }

        public string ApiToken { get; set; }

        public StateManager()
        {
        }
    }
}
