using Catel.IoC;

namespace Equality.Core.StateManager
{
    public static class StateManagerContainer
    {
        static StateManagerContainer()
        {
            Instance = ServiceLocator.Default.ResolveType<IStateManager>();
        }

        public static IStateManager Instance { get; private set; }
    }
}
