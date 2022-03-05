using Equality.Models;

namespace Equality.Data
{
    public class StateManager : IStateManager
    {
        public StateManager()
        {
        }

        public IUser CurrentUser { get; set; }

        public string ApiToken { get; set; }
    }
}
