using System.Collections.Generic;
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
    public class LoginPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IUIVisualizerService UIVisualizerService;

        protected IUserService UserService;

        #region DesignModeConstructor

        public LoginPageViewModel()
        {
            HandleDesignMode(() =>
            {
                CredentialsErrorMessage = "Ошибка данных";
            });
        }

        #endregion

        public LoginPageViewModel(INavigationService navigationService, IUIVisualizerService uIVisualizerService, IUserService userService)
        {
            NavigationService = navigationService;
            UIVisualizerService = uIVisualizerService;
            UserService = userService;

            OpenForgotPassword = new Command(OnOpenForgotPasswordExecute, () => !IsSendingRequest);
            OpenRegisterWindow = new TaskCommand(OnOpenRegisterWindowExecute);
            Login = new TaskCommand(OnLoginExecuteAsync, () => !HasErrors);
        }

        public override string Title => "Вход";

        #region Properties

        [Validatable]
        public string Email { get; set; }

        [Validatable]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;

        public string CredentialsErrorMessage { get; set; }

        public bool IsSendingRequest { get; set; }

        #endregion

        #region Commands

        public TaskCommand Login { get; private set; }

        private async Task OnLoginExecuteAsync()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            IsSendingRequest = true;

            try {
                var response = await UserService.LoginAsync(Email, Password);

                StateManager.ApiToken = response.Content["token"].ToString();
                StateManager.CurrentUser = response.Object;

                if (RememberMe) {
                    Properties.Settings.Default.api_token = StateManager.ApiToken;
                    Properties.Settings.Default.Save();
                }

                await App.LoadAsync();
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);

                CredentialsErrorMessage = ApiErrors.GetValueOrDefault("credentials", string.Empty);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }

            IsSendingRequest = false;
        }

        public TaskCommand OpenRegisterWindow { get; private set; }

        private async Task OnOpenRegisterWindowExecute()
        {
            await UIVisualizerService.ShowOrActivateAsync<RegisterWindowViewModel>(null, null);
        }

        public Command OpenForgotPassword { get; private set; }

        private void OnOpenForgotPasswordExecute() => NavigationService.Navigate<ForgotPasswordPageViewModel>();

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

            validator.ValidateField(nameof(Password), Password, new()
            {
                new NotEmptyStringRule(),
                new MinStringLengthRule(6),
#if !DEBUG
                new ValidPasswordRule(),
#endif
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
