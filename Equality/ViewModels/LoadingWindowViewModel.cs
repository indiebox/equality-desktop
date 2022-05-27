using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Catel.IoC;
using Catel.Services;

using Equality.Data;
using Equality.Http;
using Equality.MVVM;
using Equality.Services;

using MaterialDesignThemes.Wpf;

using PusherClient;

namespace Equality.ViewModels
{
    public class LoadingWindowViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected IUserService UserService;

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public LoadingWindowViewModel(IUIVisualizerService uiVisualizerService, IUserService userService)
        {
            var theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = new MaterialDesignLightTheme();
            string currentThemeString = Properties.Settings.Default.current_theme;

            switch (currentThemeString) {
                case "Light":
                    Properties.Settings.Default.current_theme = "Light";
                    baseTheme = new MaterialDesignLightTheme();

                    break;
                case "Dark":
                    Properties.Settings.Default.current_theme = "Dark";
                    baseTheme = new MaterialDesignDarkTheme();

                    break;
                case "Sync":
                    Properties.Settings.Default.current_theme = "Sync";
                    baseTheme = StateManager.GetColorTheme() == "Light" ? new MaterialDesignLightTheme() : new MaterialDesignDarkTheme();
                    break;
            }
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);

            UIVisualizerService = uiVisualizerService;
            UserService = userService;
        }

        public override string Title => "Equality";

        protected async Task HandleAuthenticatedUser()
        {
            string apiToken = Properties.Settings.Default.api_token;

            try {
                bool result = await IsValidToken(apiToken);

                if (result) {
                    App.RegisterPusher();
                    OpenMainPage();
                } else {
                    OpenAuthorizationPage();
                }

                await CloseViewModelAsync(true);
            } catch (Exception e) {
                ExceptionHandler.Handle(e);
            }
        }

        protected async Task<bool> IsValidToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) {
                return false;
            }

            StateManager.ApiToken = token;

            try {
                var response = await UserService.LoadAuthUserAsync();
                StateManager.CurrentUser = response.Object;

                return true;
            } catch (UnauthorizedHttpException) {
                StateManager.ApiToken = null;

                Properties.Settings.Default.api_token = "";
                Properties.Settings.Default.Save();

                return false;
            } catch {
                StateManager.ApiToken = null;

                throw;
            }
        }

        protected void OpenMainPage()
        {
            UIVisualizerService.ShowAsync<ApplicationWindowViewModel>();
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
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
