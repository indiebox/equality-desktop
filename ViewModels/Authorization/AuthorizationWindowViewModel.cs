using System.Threading.Tasks;

using Catel.Services;

using Equality.Core.ViewModel;
using Equality.Services;

namespace Equality.ViewModels
{
    public class AuthorizationWindowViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IUserService UserService;

        public AuthorizationWindowViewModel(INavigationService navigationService, IUserService userService)
        {
            NavigationService = navigationService;
            UserService = userService;
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
