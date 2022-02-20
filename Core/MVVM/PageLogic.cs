using System;

using Catel.MVVM.Navigation;
using Catel.MVVM.Views;

namespace Equality.Core.MVVM
{
    public class PageLogic : Catel.MVVM.Providers.PageLogic
    {
        public PageLogic(IPage targetPage, Type viewModelType = null) : base(targetPage, viewModelType)
        {
        }

        protected async override void OnNavigatingAwayFromPage(NavigatingEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Refresh) {
                e.Cancel = true;
                return;
            }

            base.OnNavigatingAwayFromPage(e);

            // We revert base cancellation of navigation if there are validation errors and close a view model.
            if (e.Cancel) {
                await CancelAndCloseViewModelAsync();
                e.Cancel = false;
            }
        }
    }
}
