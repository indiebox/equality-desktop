using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Fody;
using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient.Exceptions;
using Equality.Core.ApiClient.Interfaces;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;
        protected IApiClient ApiClient;

        public User User { get; set; }

        public LoginPageViewModel(INavigationService service, IApiClient apiClient)
        {
            NavigationService = service;
            ApiClient = apiClient;
            User = new User(string.Empty, string.Empty, string.Empty);
            OpenForgotPassword = new Command(OnOpenForgotPasswordExecute);
            Login = new TaskCommand(OnLoginExecuteAsync);
        }

        public override string Title => "Вход";
        public string EmailErrorText { get; set; }
        public string PasswordErrorText { get; set; }
        public string CredintialsErrorText { get; set; }

        public TaskCommand Login { get; private set; }

        private async Task OnLoginExecuteAsync()
        {
            Dictionary<string, object> data = new()
            {
                { "email", User.Email },
                { "password", User.Password },
                { "device_name", Environment.MachineName },
            };

            try {
                var response = await ApiClient.PostAsync("login", data);

                var token = response.Content["token"].ToString();

                Properties.Settings.Default.Properties["api_token"].DefaultValue = token;
                Debug.WriteLine(Properties.Settings.Default.Properties["api_token"].DefaultValue);


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
