using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Core.Validation;
using Equality.Core.ViewModel;
using Equality.Services;

namespace Equality.ViewModels
{
    public class ResetPasswordPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IUserService UserServise;

        public ResetPasswordPageViewModel(INavigationService navigationService, IUserService userService)
        {
            NavigationService = navigationService;
            UserServise = userService;

            GoHome = new Command(OnGoHomeExecute, () => !IsSendingRequest);
            ResendToken = new TaskCommand(OnResendTokenExecute);
            ResetPassword = new TaskCommand(OnResetPasswordExecute, OnResetPasswordCanExecute);

            NavigationCompleted += OnNavigationCompleted;

            ApiFieldsMap = new()
            {
                { nameof(Token), "token" },
                { nameof(Password), "password" },
                { nameof(PasswordConfirmation), "password_confirmation" },
            };
        }

        public override string Title => "Изменение пароля";

        #region Properties

        [ExcludeFromValidation]
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public string Token { get; set; }

        [ExcludeFromValidation]
        public string ErrorMessage { get; set; }

        [ExcludeFromValidation]
        public bool ShowSuccessMessage { get; set; } = true;

        [ExcludeFromValidation]
        public bool IsSendingRequest { get; set; }

        #endregion

        #region Commands

        public Command GoHome { get; private set; }

        private void OnGoHomeExecute()
        {
            SuspendValidations();
            NavigationService.Navigate<LoginPageViewModel>();
        }

        public TaskCommand ResendToken { get; private set; }

        private async Task OnResendTokenExecute()
        {
            IsSendingRequest = true;
            ShowSuccessMessage = false;
            ErrorMessage = null;

            try {
                await UserServise.SendResetPasswordTokenAsync(Email);
                var parameters = new Dictionary<string, object>
                {
                    { "email", Email }
                };

                ShowSuccessMessage = true;
            } catch (UnprocessableEntityHttpException e) {
                var errors = e.Errors;

                if (errors.ContainsKey("email")) {
                    ErrorMessage = errors["email"][0];
                }
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }

            IsSendingRequest = false;
        }

        public TaskCommand ResetPassword { get; private set; }

        private async Task OnResetPasswordExecute()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            IsSendingRequest = true;

            try {
                await UserServise.ResetPasswordAsync(Email, Password, PasswordConfirmation, Token);

                NavigationService.Navigate<LoginPageViewModel>();
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }

            IsSendingRequest = false;
        }

        private bool OnResetPasswordCanExecute()
        {
            return !HasErrors;
        }

        #endregion

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);

            validator.ValidateField(nameof(Token), Token, new()
            {
                new NotEmptyStringRule(),
            });
            validator.ValidateField(nameof(Password), Password, new()
            {
                new NotEmptyStringRule(),
                new MinStringLengthRule(6),
#if !DEBUG
                new ValidPasswordRule(),
#endif
            });
            validator.ValidateField(nameof(PasswordConfirmation), PasswordConfirmation, new()
            {
                new NotEmptyStringRule(),
                new PredicateRule<string>((password) => password == Password, "Пароли не совпадают."),
            });
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
