using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient.Exceptions;
using Equality.Core.ApiClient.Interfaces;
using Equality.Core.StateManager;
using Equality.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Equality.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;
        protected IApiClient ApiClient;
        protected IStateManager StateManager;

        public LoginPageViewModel(INavigationService service, IApiClient apiClient, IStateManager stateManager)
        {
            NavigationService = service;
            ApiClient = apiClient;
            StateManager = stateManager;

            OpenForgotPassword = new Command(OnOpenForgotPasswordExecute);
            Login = new TaskCommand<object>(OnLoginExecuteAsync);

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.api_token)) {
                NavigationService.Navigate<StartPageViewModel>();
            }
        }

        public override string Title => "Вход";

        #region Properties

        public string Email { get; set; }

        public bool RememberMe { get; set; } = false;

        public string EmailErrorText { get; set; }

        public string PasswordErrorText { get; set; }

        public string CredintialsErrorText { get; set; }

        #endregion

        #region Commands

        public TaskCommand<object> Login { get; private set; }

        private async Task OnLoginExecuteAsync(object parameter)
        {
            Dictionary<string, object> data = new()
            {
                { "email", Email },
                { "password", ((PasswordBox)parameter).Password },
                { "device_name", Environment.MachineName },
            };

            try {
                var response = await ApiClient.PostAsync("login", data);

                var user = JsonConvert.DeserializeObject<User>(response.Content["data"].ToString(), new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                });
                string token = response.Content["token"].ToString();

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
                PasswordErrorText = errors.ContainsKey("password") ? string.Join("", errors["password"]) : string.Empty;
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public Command OpenForgotPassword { get; private set; }

        private void OnOpenForgotPasswordExecute() => NavigationService.Navigate<ForgotPasswordPageViewModel>();

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
