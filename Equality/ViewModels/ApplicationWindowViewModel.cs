using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;

namespace Equality.ViewModels
{
    public class ApplicationWindowViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected INavigationService NavigationService;

        protected IUserService UserService;

        #region DesignModeConstructor

        public ApplicationWindowViewModel()
        {
            HandleDesignMode();
        }

        #endregion

        public ApplicationWindowViewModel(IUIVisualizerService uIVisualizerService, INavigationService navigationService, IUserService userService)
        {
            UIVisualizerService = uIVisualizerService;
            NavigationService = navigationService;
            UserService = userService;

            Logout = new TaskCommand(OnLogoutExecute);

            StateManager.SelectedTeamChanged += SelectedTeamChangedInStateManager;
            StateManager.SelectedProjectChanged += SelectedProjectChangedInStateManager;
        }

        public override string Title => "Equality";

        public enum Tab
        {
            Main,
            Projects,
            Team,
            Project,
        }

        #region Properties

        public Tab ActiveTab { get; set; }

        public Team SelectedTeam => StateManager.SelectedTeam;

        public Project SelectedProject => StateManager.SelectedProject;

        #endregion

        #region Commands

        public TaskCommand Logout { get; private set; }

        private async Task OnLogoutExecute()
        {
            try {
                await UserService.LogoutAsync();

                StateManager.ApiToken = null;
                StateManager.CurrentUser = null;

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
                case Tab.Team:
                    NavigationService.Navigate<TeamPageViewModel>();
                    break;
                case Tab.Project:
                    NavigationService.Navigate<ProjectPageViewModel>();
                    break;
            }
        }

        private void SelectedTeamChangedInStateManager()
        {
            RaisePropertyChanged(nameof(SelectedTeam));
        }

        private void SelectedProjectChangedInStateManager()
        {
            RaisePropertyChanged(nameof(SelectedProject));
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            OnActiveTabChanged();
        }

        protected override async Task CloseAsync()
        {
            StateManager.SelectedTeamChanged -= SelectedTeamChangedInStateManager;
            StateManager.SelectedProjectChanged -= SelectedProjectChangedInStateManager;

            await base.CloseAsync();
        }
    }
}
