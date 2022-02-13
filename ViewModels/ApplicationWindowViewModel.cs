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
        protected INavigationService NavigationService;

        public ApplicationWindowViewModel(INavigationService service)
        {
            NavigationService = service;

            NavigationService.Navigate<TeamsPageViewModel>();
        }

        public override string Title => "Equality";

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
