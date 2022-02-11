﻿using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Core.ViewModel;
using Equality.Services;

namespace Equality.ViewModels
{
    public class AppWindowViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected IUserService UserService;

        public AppWindowViewModel(IUIVisualizerService uiVisualizerService, IUserService userService)
        {
            UIVisualizerService = uiVisualizerService;
            UserService = userService;
        }

        public override string Title => "Equality";

        protected async Task HandleAuthenticatedUser()
        {
            string apiToken = Properties.Settings.Default.api_token;

            bool result = await IsValidToken(apiToken);

            if (result) {
                OpenMainPage();
            } else {
                OpenAuthorizationPage();
            }

            await CloseViewModelAsync(true);
        }

        protected async Task<bool> IsValidToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) {
                return false;
            }

            try {
                var user = await UserService.GetUserAsync(token);

                StateManager.CurrentUser = user;
                StateManager.ApiToken = token;

                return true;
            } catch (UnauthorizedHttpException) {
                Properties.Settings.Default.api_token = "";
                Properties.Settings.Default.Save();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }

            return false;
        }

        protected void OpenMainPage()
        {
            UIVisualizerService.ShowAsync<MainWindowViewModel>();
        }

        protected void OpenAuthorizationPage()
        {
            UIVisualizerService.ShowAsync<AuthorizationWindowViewModel>();
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await HandleAuthenticatedUser();
        }

        protected override async Task CloseAsync()
        {
            // TODO: Add uninitialization logic like unsubscribing from events

            await base.CloseAsync();
        }
    }
}