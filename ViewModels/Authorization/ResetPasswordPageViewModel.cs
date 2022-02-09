using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

namespace Equality.ViewModels
{
    public class ResetPasswordPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        public ResetPasswordPageViewModel(INavigationService service)
        {
            NavigationService = service;

            OpenForgotPasswordPage = new Command(OnOpenForgotPasswordPageExecute);
            OpenLoginPage = new Command(OnOpenLoginPageExecute);
        }

        public override string Title => "Изменение пароля";

        #region Commands

        public Command OpenForgotPasswordPage { get; private set; }

        private void OnOpenForgotPasswordPageExecute() => NavigationService.Navigate<ForgotPasswordPageViewModel>();

        public Command OpenLoginPage { get; private set; }

        private void OnOpenLoginPageExecute() => NavigationService.Navigate<LoginPageViewModel>();

        #endregion

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
