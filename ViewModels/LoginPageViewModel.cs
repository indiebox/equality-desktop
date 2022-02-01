using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Core.ApiClient.Exceptions;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        public LoginPageViewModel(INavigationService service)
        {
            NavigationService = service;

            OpenForgotPassword = new Command(OnOpenForgotPasswordExecute);
            Login = new TaskCommand(OnLoginExecuteAsync);
        }

        public override string Title => "Вход";

        public string Email { private get; set; }
        public string Password { private get; set; }
        public string EmailErrorText { private get; set; }
        public string PasswordErrorText { private get; set; }
        public string CredintialsErrorText { private get; set; }

        public TaskCommand Login { get; private set; }

        private async Task OnLoginExecuteAsync()
        {
            Dictionary<string, object> data = new()
            {
                { "email", Email },
                { "password", Password },
                { "device_name", Environment.MachineName },
            };

            try {
                ApiClient apiClient = new();
                ApiResponseMessage p = await apiClient.PostAsync("http://equality/api/v1/login", data);
                Debug.WriteLine(p.Content.ToString());

            } catch (UnprocessableEntityHttpException e) {
                Dictionary<string, string[]> errors = e.Errors;
                if (errors.ContainsKey("credentials")) {

                } else if (errors.ContainsKey("email")) {
                    EmailErrorText = errors["email"][0];
                } else if (errors.ContainsKey("password")) {
                    EmailErrorText = errors["password"][0];
                }
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
