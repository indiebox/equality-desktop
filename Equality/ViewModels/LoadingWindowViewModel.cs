using System;
using System.Threading.Tasks;

using Catel.Services;

using Equality.Data;
using Equality.Http;
using Equality.MVVM;
using Equality.Services;

namespace Equality.ViewModels
{
    public class LoadingWindowViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected IUserService UserService;

        public LoadingWindowViewModel(IUIVisualizerService uiVisualizerService, IUserService userService)
        {
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
                    await App.LoadAsync();
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
