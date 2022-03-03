using Catel;
using Catel.IoC;

namespace Equality.Core.StateManager
{
    public static class StateManagerContainer
    {
        static StateManagerContainer()
        {
            if (CatelEnvironment.IsInDesignMode) {
                Instance = new StateManager()
                {
                    CurrentUser = new Models.User()
                    {
                        Id = 1,
                        Name = "Logged user",
                        Email = "example@example.org",
                        CreatedAt = System.DateTime.Today,
                    },
                };
                return;
            }

            Instance = ServiceLocator.Default.ResolveType<IStateManager>();
        }

        public static IStateManager Instance { get; private set; }
    }
}
