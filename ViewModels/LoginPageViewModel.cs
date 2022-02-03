using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;

using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Services;


namespace Equality.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        protected IUserService UserService;

        protected IStateManager StateManager;

        protected IDisposable ValidationToken;

        public LoginPageViewModel(INavigationService navigationService, IUserService userService, IStateManager stateManager)
        {
            NavigationService = navigationService;
            UserService = userService;
            StateManager = stateManager;

            OpenForgotPassword = new Command(OnOpenForgotPasswordExecute);
            OpenRegisterWindow = new TaskCommand(OnOpenRegisterWindowExecute);
            Login = new TaskCommand<object>(OnLoginExecuteAsync, OnLoginCanExecute);

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.api_token)) {
                NavigationService.Navigate<StartPageViewModel>();
            }
        }

        public override string Title => "Вход";

        protected bool CheckApiErrors { get; set; } = false;

        #region Properties

        public string Email { get; set; }

        public bool RememberMe { get; set; } = false;

        public string EmailErrorText { get; set; }

        public string PasswordErrorText { get; set; }

        public string CredintialsErrorText { get; set; }

        public bool CredentialsVisibility { get; set; }

        public IDataErrorInfo MyObject;

        #endregion

        #region Commands

        public TaskCommand<object> Login { get; private set; }

        private async Task OnLoginExecuteAsync(object parameter)
        {
            if (!await SaveViewModelAsync()) {
                return;
            }

            try {
                var (user, token) = await UserService.LoginAsync(Email, ((PasswordBox)parameter).Password);

                StateManager.CurrentUser = user;
                StateManager.ApiToken = token;

                if (RememberMe) {
                    Properties.Settings.Default.api_token = token;
                    Properties.Settings.Default.Save();
                }

                NavigationService.Navigate<StartPageViewModel>();
            } catch (UnprocessableEntityHttpException e) {
                var errors = e.Errors;

                CheckApiErrors = true;

                CredintialsErrorText = errors.ContainsKey("credentials") ? string.Join("", errors["credentials"]) : string.Empty;
                CredentialsVisibility = errors.ContainsKey("credentials") ? true : false;
                EmailErrorText = errors.ContainsKey("email") ? string.Join("", errors["email"]) : string.Empty;
                PasswordErrorText = errors.ContainsKey("password") ? string.Join("", errors["password"]) : string.Empty;

                Validate(true);

                CheckApiErrors = false;
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        private bool OnLoginCanExecute(object parameter)
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
            Debug.WriteLine("Called");

            if (CheckApiErrors) {
                if (!string.IsNullOrWhiteSpace(EmailErrorText)) {
                    validationResults.Add(FieldValidationResult.CreateError(nameof(Email), EmailErrorText));
                }

                return;
            }

            if (string.IsNullOrWhiteSpace(Email)) {
                validationResults.Add(FieldValidationResult.CreateError(nameof(Email), "Не может быть пустым."));
            }

            if (!string.IsNullOrEmpty(Email) && Email.Length < 6) {
                validationResults.Add(FieldValidationResult.CreateError(nameof(Email), "Не может быть меньше 6."));
            }

            //Debug.WriteLine(GetFieldErrors(nameof(Email)));
            //Debug.WriteLine(HasErrors);
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
