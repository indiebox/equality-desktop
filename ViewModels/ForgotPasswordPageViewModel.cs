using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Services;

namespace Equality.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        protected IUserService UserService;

        public ForgotPasswordPageViewModel(INavigationService navigationService, IUserService userService)
        {
            NavigationService = navigationService;
            UserService = userService;

            GoBack = new Command(OnGoBackExecute);
            OpenResetPasswordPage = new TaskCommand(OnOpenResetPasswordPageExecute);
        }

        public override string Title => "Восстановление пароля";

        #region Properties

        public string Error { get; set; }

        public string Email { get; set; }

        #endregion

        #region Commands

        public TaskCommand OpenResetPasswordPage { get; private set; }

        private async Task OnOpenResetPasswordPageExecute()
        {
            try {
                var response = await UserService.ForgotPasswordEmailSendAsync(Email);
                var parameters = new Dictionary<string, object>
                {
                    { "email", Email }
                };

                NavigationService.Navigate<ResetPasswordPageViewModel>(parameters);
            } catch (UnprocessableEntityHttpException e) {
                var errors = e.Errors;

                Error = errors.ContainsKey("email") ? string.Join("", errors["email"]) : string.Empty;
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public Command GoBack { get; private set; }

        private void OnGoBackExecute() => NavigationService.GoBack();

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
