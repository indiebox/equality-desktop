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

            ApiFieldsMap = new()
            {
                { nameof(Email), "email" },
            };
        }

        public override string Title => "Вход";

        #region Properties

        public string Email { get; set; }

        public string Password { get; set; }

        [ExcludeFromValidation]
        public bool RememberMe { get; set; } = false;

        [ExcludeFromValidation]
        public string CredentialsErrorMessage { get; set; }

        #endregion

        #region Commands

        public TaskCommand Login { get; private set; }

        private async Task OnLoginExecuteAsync()
        {
            if (FirstValidationHasErrors()) {
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
                HandleApiErrors(e.Errors);

                CredentialsErrorMessage = ApiErrors.GetValueOrDefault("credentials", string.Empty);
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
            var validator = new Validator(validationResults);

            validator.ValidateField(nameof(Email), Email, new()
            {
                new NotEmptyStringRule(),
                new ValidEmailRule(),
            });

            validator.ValidateField(nameof(Password), Password, new()
            {
                new NotEmptyStringRule(),
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
