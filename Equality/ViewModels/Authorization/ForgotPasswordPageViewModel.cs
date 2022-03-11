using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;
using Catel.Services;

using Equality.Http;
using Equality.Validation;
using Equality.MVVM;
using Equality.Services;

namespace Equality.ViewModels
{
    public class ForgotPasswordPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IUserService UserService;

        #region DesignModeConstructor

        public ForgotPasswordPageViewModel()
        {
            HandleDesignMode();
        }

        #endregion

        public ForgotPasswordPageViewModel(INavigationService navigationService, IUserService userService)
        {
            NavigationService = navigationService;
            UserService = userService;

            GoBack = new Command(OnGoBackExecute, () => !IsSendingRequest);
            OpenResetPasswordPage = new TaskCommand(OnOpenResetPasswordPageExecute, () => !HasErrors);
        }

        public override string Title => "Восстановление пароля";

        #region Properties

        [Validatable]
        public string Email { get; set; }

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

                NavigationService.Navigate<ResetPasswordPageViewModel>(new() { { "email", Email } });
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }

            IsSendingRequest = false;
        }

        public Command GoBack { get; private set; }

        private void OnGoBackExecute() => NavigationService.GoBack();

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
