﻿using System.Collections.Generic;
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
    public class ForgotPasswordPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IUserService UserService;

        public ForgotPasswordPageViewModel(INavigationService navigationService, IUserService userService)
        {
            NavigationService = navigationService;
            UserService = userService;

            GoBack = new Command(OnGoBackExecute, () => !IsSendingRequest);
            OpenResetPasswordPage = new TaskCommand(OnOpenResetPasswordPageExecute, () => !HasErrors);

            ApiFieldsMap = new()
            {
                { nameof(Email), "email" },
            };
        }

        public override string Title => "Восстановление пароля";

        #region Properties

        public string Email { get; set; }

        [ExcludeFromValidation]
        public bool IsSendingRequest { get; set; }

        #endregion

        #region Commands

        public TaskCommand OpenResetPasswordPage { get; private set; }

        private async Task OnOpenResetPasswordPageExecute()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            IsSendingRequest = true;

            try {
                await UserService.SendResetPasswordTokenAsync(Email);
                var parameters = new Dictionary<string, object>
                {
                    { "email", Email }
                };

                SuspendValidations(false);

                NavigationService.Navigate<ResetPasswordPageViewModel>(parameters);
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }

            IsSendingRequest = false;
        }

        public Command GoBack { get; private set; }

        private void OnGoBackExecute()
        {
            SuspendValidations();
            NavigationService.GoBack();
        }

        #endregion

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);

            validator.ValidateField(nameof(Email), Email, new()
            {
                new NotEmptyStringRule(),
                new ValidEmailRule(),
            });
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
