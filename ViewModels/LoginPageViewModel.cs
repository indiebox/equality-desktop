using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

namespace Equality.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        public LoginPageViewModel(INavigationService service)
        {
            NavigationService = service;

            OpenForgotPassword = new Command(OnOpenForgotPasswordExecute);
        }

        public override string Title => "Вход";

        public Command OpenForgotPassword { get; private set; }

        private void OnOpenForgotPasswordExecute()
        {
            NavigationService.Navigate<ForgotPasswordPageViewModel>();
        }

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
