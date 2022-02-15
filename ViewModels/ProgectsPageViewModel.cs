using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class ProgectsPageViewModel : ViewModel
    {
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
