using System.Windows.Controls;

namespace Equality.Core.MVVM
{
    public class NavigationRootService : Catel.Services.NavigationRootService
    {
        private readonly object _locker = new();

        public static Frame TemporaryNagivationRoot = null;

        public override object GetNavigationRoot()
        {
            lock (_locker) {
                if (TemporaryNagivationRoot != null) {
                    var root = TemporaryNagivationRoot;
                    TemporaryNagivationRoot = null;

                    return root;
                }

                return GetApplicationRootFrame();
            }
        }
    }
}
