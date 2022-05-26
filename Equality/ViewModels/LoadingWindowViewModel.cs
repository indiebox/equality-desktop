using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Catel.IoC;
using Catel.Services;

using Equality.Data;
using Equality.Http;
using Equality.MVVM;
using Equality.Services;

using PusherClient;

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
                    await LoadApplicationAsync();
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

        protected async Task LoadApplicationAsync()
        {
            App.RegisterPusher();

            await LoadSavedDataAsync();

            _ = UIVisualizerService.ShowAsync<ApplicationWindowViewModel>();
        }

        protected async Task LoadSavedDataAsync()
        {
            if (Properties.Settings.Default.menu_selected_team != 0) {
                try {
                    var response = await ServiceLocator.Default.ResolveType<ITeamService>().GetTeamAsync(Properties.Settings.Default.menu_selected_team);

                    StateManager.SelectedTeam = response.Object;
                } catch (NotFoundHttpException) {
                    Properties.Settings.Default.menu_selected_team = 0;
                }
            }

            if (Properties.Settings.Default.menu_selected_project != 0) {
                try {
                    var response = await ServiceLocator.Default.ResolveType<IProjectService>().GetProjectAsync(Properties.Settings.Default.menu_selected_project);

                    StateManager.SelectedProject = response.Object;
                } catch (NotFoundHttpException) {
                    Properties.Settings.Default.menu_selected_project = 0;
                }
            }

            Properties.Settings.Default.Save();
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
