using System.Windows.Controls;

namespace Equality.MVVM
{
    public class NavigationRootService : Catel.Services.NavigationRootService
    {
        private readonly object _locker = new();

        public static Frame TemporaryNavigationRoot = null;

        public override object GetNavigationRoot()
        {
            lock (_locker) {
                if (TemporaryNavigationRoot != null) {
                    var root = TemporaryNavigationRoot;
                    TemporaryNavigationRoot = null;

                    return root;
                }

                return GetApplicationRootFrame();
            }
        }
    }
}
