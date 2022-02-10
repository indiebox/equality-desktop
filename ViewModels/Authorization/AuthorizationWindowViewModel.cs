using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.IoC;
using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Core.ViewModel;
using Equality.Services;

namespace Equality.ViewModels
{
    public class AuthorizationWindowViewModel : ViewModel
    {
        protected IUserService UserService;

        protected IStateManager StateManager;

        public AuthorizationWindowViewModel(IUserService userService, IStateManager stateManager)
        {
            UserService = userService;
            StateManager = stateManager;
        }

        public override string Title => "Equality";

        protected async Task HandleAuthenticatedUser()
        {
            string apiToken = Properties.Settings.Default.api_token;

            OpenLoginPage();

            if (await IsValidToken(apiToken)) {
                OpenMainPage();
            }
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

        protected async void OpenMainPage()
        {
            var uiService = this.GetDependencyResolver().Resolve<IUIVisualizerService>();
            await uiService.ShowAsync<MainWindowViewModel>();
        }

        protected void OpenLoginPage()
        {
            var navigationService = this.GetDependencyResolver().Resolve<INavigationService>();
            navigationService.Navigate<LoginPageViewModel>();
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
