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

        public ResetPasswordPageViewModel(INavigationService service, IUserService userServise)
        {
            NavigationService = service;
            UserServise = userServise;

            OpenForgotPasswordPage = new Command(OnOpenForgotPasswordPageExecute);
            OpenLoginPage = new TaskCommand(OnOpenLoginPageExecute);

            NavigationCompleted += ResetPasswordPageViewModel_NavigationCompleted;
        }

        public override string Title => "Изменение пароля";


        #region Properties

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public string Token { get; set; }

        public string EmailErrorText { get; set; }

        public string PasswordErrorText { get; set; }

        public string PasswordConfirmationErrorText { get; set; }

        public string TokenErrorText { get; set; }

        public string CredintialsErrorText { get; set; }

        public bool CredentialsVisibility { get; set; }

        #endregion

        #region Commands

        public Command OpenForgotPasswordPage { get; private set; }

        private void OnOpenForgotPasswordPageExecute() => NavigationService.Navigate<ForgotPasswordPageViewModel>();

        public TaskCommand OpenLoginPage { get; private set; }

        private async Task OnOpenLoginPageExecute()
        {
            try {
                var response = await UserServise.ResetPasswordAsync(Email, Password, PasswordConfirmation, Token);
                NavigationService.Navigate<LoginPageViewModel>();
            } catch (UnprocessableEntityHttpException e) {
                var errors = e.Errors;
                EmailErrorText = errors.ContainsKey("email") ? string.Join("", errors["email"]) : string.Empty;
                PasswordErrorText = errors.ContainsKey("password") ? string.Join("", errors["password"]) : string.Empty;
                PasswordConfirmationErrorText = errors.ContainsKey("password_confirmation") ? string.Join("", errors["password_confirmation"]) : string.Empty;
                TokenErrorText = errors.ContainsKey("token") ? string.Join("", errors["token"]) : string.Empty;
                CredintialsErrorText = errors.ContainsKey("credentials") ? string.Join("", errors["credentials"]) : string.Empty;
                CredentialsVisibility = errors.ContainsKey("credentials");
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        private void ResetPasswordPageViewModel_NavigationCompleted(object sender, System.EventArgs e)
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
