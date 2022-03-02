using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel;
using Catel.MVVM;
using Catel.Services;

using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Services;

namespace Equality.ViewModels
{
    public class ApplicationWindowViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected INavigationService NavigationService;

        protected IUserService UserService;

        public ApplicationWindowViewModel(IUIVisualizerService uIVisualizerService, INavigationService navigationService, IUserService userService)
        {
            UIVisualizerService = uIVisualizerService;
            NavigationService = navigationService;
            UserService = userService;

            Logout = new TaskCommand(OnLogoutExecute);
        }

        public override string Title => "Equality";

        public enum Tab
        {
            Main,
            Projects,
            Team,
            Project,
            TempProgect,
        }

        #region Properties

        public Tab ActiveTab { get; set; }

        public Team SelectedTeam { get; set; }

        #endregion

        #region Commands

        public TaskCommand Logout { get; private set; }

        private async Task OnLogoutExecute()
        {
            try {
                await UserService.LogoutAsync();

                Properties.Settings.Default.api_token = null;
                Properties.Settings.Default.Save();

                await UIVisualizerService.ShowAsync<AuthorizationWindowViewModel>();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Methods

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Main:
                default:
                    NavigationService.Navigate<StartPageViewModel>();
                    break;
                case Tab.Projects:
                    NavigationService.Navigate<ProjectsPageViewModel>();
                    break;
                case Tab.TempProgect:
                    NavigationService.Navigate<ProjectPageViewModel>();
                    break;
                case Tab.Team:
                    Argument.IsNotNull(nameof(SelectedTeam), SelectedTeam);

                    NavigationService.Navigate<TeamPageViewModel>(new() { { "team", SelectedTeam } });
                    break;
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            OnActiveTabChanged();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
