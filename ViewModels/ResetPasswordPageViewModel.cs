using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Services;

namespace Equality.ViewModels
{
    public class ResetPasswordPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        protected IUserService UserServise;

        public ResetPasswordPageViewModel(INavigationService navigationService, IUserService userService)
        {
            NavigationService = navigationService;
            UserServise = userService;

            GoBack = new Command(OnGoBackExecute, () => !IsSendingRequest);
            ResetPassword = new TaskCommand(OnResetPasswordExecute);

            NavigationCompleted += OnNavigationCompleted;
        }

        public override string Title => "Изменение пароля";

        #region Properties

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public string Token { get; set; }

        public bool IsSendingRequest { get; set; }

        #endregion

        #region Commands

        public Command GoBack { get; private set; }

        private void OnGoBackExecute() => NavigationService.GoBack();

        public TaskCommand ResetPassword { get; private set; }

        private async Task OnResetPasswordExecute()
        {
            IsSendingRequest = true;

            try {
                var response = await UserServise.ResetPasswordAsync(Email, Password, PasswordConfirmation, Token);

                NavigationService.Navigate<LoginPageViewModel>();
            } catch (UnprocessableEntityHttpException e) {
                var errors = e.Errors;
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }

            IsSendingRequest = false;
        }

        #endregion

        private void OnNavigationCompleted(object sender, System.EventArgs e)
        {
            Email = (string)NavigationContext.Values["email"];
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
