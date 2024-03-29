﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;
using Catel.Services;

using Equality.Http;
using Equality.Validation;
using Equality.MVVM;
using Equality.Services;
using Equality.Data;

namespace Equality.ViewModels
{
    /*
     * NavigationContext: 
     * email - the user email
     */
    public class ResetPasswordPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IUserService UserServise;

        #region DesignModeConstructor

        public ResetPasswordPageViewModel()
        {
            HandleDesignMode();
        }

        #endregion

        public ResetPasswordPageViewModel(INavigationService navigationService, IUserService userService)
        {
            NavigationService = navigationService;
            UserServise = userService;

            GoHome = new Command(OnGoHomeExecute, () => !IsSendingRequest);
            ResendToken = new TaskCommand(OnResendTokenExecute, () => !IsSendingRequest);
            ResetPassword = new TaskCommand(OnResetPasswordExecute, () => !IsSendingRequest && !HasErrors);
        }

        public override string Title => "Изменение пароля";

        #region Properties

        public string Email { get; set; }

        [Validatable]
        public string Password { get; set; }

        [Validatable]
        public string PasswordConfirmation { get; set; }

        [Validatable]
        public string Token { get; set; }

        public string ErrorMessage { get; set; }

        public bool ShowSuccessMessage { get; set; } = true;

        public bool IsSendingRequest { get; set; }

        #endregion

        #region Commands

        public Command GoHome { get; private set; }

        private void OnGoHomeExecute() => NavigationService.Navigate<LoginPageViewModel>();

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
                ExceptionHandler.Handle(e);
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
                ExceptionHandler.Handle(e);
            }

            IsSendingRequest = false;
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

        #region Methods

        protected override void OnNavigationCompleted()
        {
            Email = (string)NavigationContext.Values["email"];
        }

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
