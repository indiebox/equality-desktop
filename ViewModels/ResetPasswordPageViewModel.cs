using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.Fody;
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

            GoBack = new Command(OnGoBackExecute, () => !IsSendingRequest);
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

        [NoWeaving]
        [ExcludeFromValidation]
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public string Token { get; set; }

        [ExcludeFromValidation]
        public bool IsSendingRequest { get; set; }

        #endregion

        #region Commands

        public Command GoBack { get; private set; }

        private void OnGoBackExecute() => NavigationService.GoBack();

        public TaskCommand ResetPassword { get; private set; }

        private async Task OnResetPasswordExecute()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            IsSendingRequest = true;

            try {
                var response = await UserServise.ResetPasswordAsync(Email, Password, PasswordConfirmation, Token);

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
                new PredicateRule<string>((password) => password != PasswordConfirmation, "Пароли не совпадают."),
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
