using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Core.Validation;
using Equality.Core.ViewModel;
using Equality.Services;

namespace Equality.ViewModels
{
    public class LoginPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IUserService UserService;

        protected IStateManager StateManager;

        public LoginPageViewModel(INavigationService navigationService, IUserService userService, IStateManager stateManager)
        {
            NavigationService = navigationService;
            UserService = userService;
            StateManager = stateManager;

            OpenForgotPassword = new Command(OnOpenForgotPasswordExecute);
            OpenRegisterWindow = new TaskCommand(OnOpenRegisterWindowExecute);
            Login = new TaskCommand(OnLoginExecuteAsync, OnLoginCanExecute);

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.api_token)) {
                NavigationService.Navigate<StartPageViewModel>();
            }
        }

        public override string Title => "Вход";

        #region Properties

        public string Email { get; set; }

        public string Password { get; set; }

        [ExcludeFromValidation]
        public bool RememberMe { get; set; } = false;

        [ExcludeFromValidation]
        public string EmailErrorText { get; set; }

        [ExcludeFromValidation]
        public string CredintialsErrorText { get; set; }

        #endregion

        #region Commands

        public TaskCommand Login { get; private set; }

        private async Task OnLoginExecuteAsync()
        {
            if (EnableValidation()) {
                return;
            }

            try {
                var (user, token) = await UserService.LoginAsync(Email, Password);

                StateManager.CurrentUser = user;
                StateManager.ApiToken = token;

                if (RememberMe) {
                    Properties.Settings.Default.api_token = token;
                    Properties.Settings.Default.Save();
                }

                NavigationService.Navigate<StartPageViewModel>();
            } catch (UnprocessableEntityHttpException e) {
                var errors = e.Errors;

                CredintialsErrorText = errors.ContainsKey("credentials") ? string.Join("", errors["credentials"]) : string.Empty;
                EmailErrorText = errors.ContainsKey("email") ? string.Join("", errors["email"]) : string.Empty;

                DisplayApiErrors();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        private bool OnLoginCanExecute()
        {
            return !HasErrors;
        }

        public TaskCommand OpenRegisterWindow { get; private set; }

        private async Task OnOpenRegisterWindowExecute()
        {
            var uiVisualizerService = this.GetDependencyResolver().Resolve<IUIVisualizerService>();
            var vm = this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<RegisterWindowViewModel>();

            await uiVisualizerService.ShowAsync(vm);
        }

        public Command OpenForgotPassword { get; private set; }

        private void OnOpenForgotPasswordExecute() => NavigationService.Navigate<ForgotPasswordPageViewModel>();

        #endregion

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            Debug.WriteLine("Call");

            var validator = new Validator(validationResults);

            validator.ValidateField(nameof(Email), Email, new()
            {
                new NotEmptyStringRule(),
                new MinStringLengthRule(6)
            });

            validator.ValidateField(nameof(Password), Password, new()
            {
                new NotEmptyStringRule(),
            });
        }

        protected override void DisplayApiErrors(List<IFieldValidationResult> validationResults)
        {
            Debug.WriteLine("Api call");

            if (!string.IsNullOrEmpty(EmailErrorText)) {
                validationResults.Add(FieldValidationResult.CreateError(nameof(Email), EmailErrorText));
            }
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
