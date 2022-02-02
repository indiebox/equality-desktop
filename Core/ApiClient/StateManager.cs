using Equality.Core.ApiClient.Interfaces;

namespace Equality.Core.ApiClient
{
    public class StateManager : IStateManager
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public StateManager(string name, string email, string token)
        {
            Name = name;
            Email = email;
            Token = token;
        }
    }
}
