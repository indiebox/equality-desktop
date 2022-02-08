using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Catel.Services;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    class ApplicationWindowViewModel : ViewModel
    {
        //protected INavigationService NavigationService;

        public ApplicationWindowViewModel(/*INavigationService service*/)
        {
            //NavigationService = service;

            //NavigationService.Navigate<StartPageViewModel>();
        }

        public override string Title => "Equality";

        protected override async Task InitializeAsync() => await base.InitializeAsync();// TODO: subscribe to events here

        protected override async Task CloseAsync() =>
            // TODO: unsubscribe from events here

            await base.CloseAsync();
    }
}
