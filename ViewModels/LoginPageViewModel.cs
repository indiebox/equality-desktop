﻿using System.Diagnostics;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Models;

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

        public string Email { get; set; }
        public string Password { get; set; }


        public TaskCommand Login { get; private set; }

        private async Task OnLoginExecuteAsync()
        {
            int statusCode = await User.Login(Email, Password, "");
            Debug.WriteLine(statusCode);
        }

        public Command OpenForgotPassword { get; private set; }

        private void OnOpenForgotPasswordExecute()
        {
            NavigationService.Navigate<ForgotPasswordPageViewModel>();
        }

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
