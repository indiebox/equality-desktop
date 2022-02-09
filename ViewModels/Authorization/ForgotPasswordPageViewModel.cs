using System.Threading.Tasks;

using Catel;
using Catel.MVVM;
using Catel.Services;

namespace Equality.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        public ForgotPasswordPageViewModel(INavigationService service)
        {
            NavigationService = service;

            OpenLoginPage = new Command(OnOpenLoginPageExecute);
            OpenResetPasswordPage = new Command(OnOpenResetPasswordPageExecute);
        }

        public override string Title => "Восстановление пароля";

        #region Commands

        public Command OpenResetPasswordPage { get; private set; }

        private void OnOpenResetPasswordPageExecute() => NavigationService.Navigate<ResetPasswordPageViewModel>();

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
