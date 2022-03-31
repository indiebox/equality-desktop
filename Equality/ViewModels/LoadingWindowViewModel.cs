using System;
using System.Threading.Tasks;

using Catel.ExceptionHandling;
using Catel.IoC;
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

            StateManager.ApiToken = token;

            try {
                var response = await UserService.LoadAuthUserAsync();
                StateManager.CurrentUser = response.Object;

                return true;
            } catch (UnauthorizedHttpException) {
                Properties.Settings.Default.api_token = "";
                Properties.Settings.Default.Save();
            } catch (Exception e) {
                // We handle any exception manually, because AppDomain.UnhandledException
                // is not working here for some reason.
                var s = ServiceLocator.Default.ResolveType<IExceptionService>();
                s.HandleException(e);
                throw;
            } finally {
                StateManager.ApiToken = null;
            }

            return true;
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
            // TODO: Add uninitialization logic like unsubscribing from events

            await base.CloseAsync();
        }
    }
}
