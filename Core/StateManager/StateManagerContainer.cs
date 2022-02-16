using Catel;
using Catel.IoC;

namespace Equality.Core.StateManager
{
    public static class StateManagerContainer
    {
        static StateManagerContainer()
        {
            if (CatelEnvironment.IsInDesignMode) {
                Instance = new StateManager();
                return;
            }

            Instance = ServiceLocator.Default.ResolveType<IStateManager>();
        }

        public static IStateManager Instance { get; private set; }
    }
}
