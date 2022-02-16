using System.Threading.Tasks;

using Catel.Services;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class AuthorizationWindowViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        public AuthorizationWindowViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public override string Title => "Equality";

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            NavigationService.Navigate<LoginPageViewModel>();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
