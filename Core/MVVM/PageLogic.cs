using System;
using System.Collections.Generic;
using System.Text;

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
            base.OnNavigatingAwayFromPage(e);

            if (e.Cancel) {
                await CancelAndCloseViewModelAsync();
                e.Cancel = false;
            }
        }
    }
}
