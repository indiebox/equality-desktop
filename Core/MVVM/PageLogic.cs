using System;

using Catel.Logging;
using Catel.MVVM;
using Catel.MVVM.Navigation;
using Catel.MVVM.Views;

namespace Equality.Core.MVVM
{
    public class PageLogic : Catel.MVVM.Providers.PageLogic
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public PageLogic(IPage targetPage, Type viewModelType = null) : base(targetPage, viewModelType)
        {
        }

        protected async override void OnNavigatingAwayFromPage(NavigatingEventArgs e)
        {
            // Disable page refresh (F5 key).
            if (e.NavigationMode == NavigationMode.Refresh) {
                e.Cancel = true;
                return;
            }

            if (ViewModelLifetimeManagement != ViewModelLifetimeManagement.Automatic) {
                Log.Debug($"View model lifetime management is set to '{ViewModelLifetimeManagement}', not closing view model on navigation event for '{TargetViewType?.Name}'");
                return;
            }

            await SaveViewModelAsync();
            await CloseViewModelAsync(true);
        }
    }
}
