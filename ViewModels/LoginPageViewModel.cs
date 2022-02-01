﻿using System;
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

        public string Email { private get; set; } = string.Empty;
        public string Password { private get; set; } = string.Empty;
        public string EmailErrorText { private get; set; } = string.Empty;
        public string PasswordErrorText { private get; set; } = string.Empty;
        public string CredintialsErrorText { private get; set; } = string.Empty;

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
                var errors = e.Errors;
                CredintialsErrorText = errors.ContainsKey("credentials") ? errors["credentials"][0] : string.Empty;
                EmailErrorText = errors.ContainsKey("email") ? errors["email"][0] : string.Empty;
                PasswordErrorText = errors.ContainsKey("password") ? errors["password"][0] : string.Empty;
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
