using System.Threading.Tasks;
using System.Net.Http;

using Catel.MVVM;
using Catel.Services;

using Equality.Extensions;
using Equality.MVVM;
using Equality.Models;
using Equality.Data;
using Equality.Services;
using Equality.Http;
using Equality.Helpers;

namespace Equality.ViewModels
{
    public class ProjectPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IProjectService ProjectService;

        protected IBoardService BoardService;

        #region DesignModeConstructor

        public ProjectPageViewModel()
        {
            HandleDesignMode(() =>
            {
                Project = StateManager.SelectedProject;
                Leader = new User { Name = "Marshall" };
            });
        }

        #endregion

        public ProjectPageViewModel(INavigationService navigationService, IProjectService projectService, IBoardService boardService)
        {
            NavigationService = navigationService;
            ProjectService = projectService;
            BoardService = boardService;

            Project = StateManager.SelectedProject;
            SaveRecentProject();

            OpenTeamPage = new TaskCommand(OnOpenTeamPageExecuteAsync, () => Project.Team != null);
        }

        public enum Tab
        {
            Board,
            Leader,
            Settings,
        }

        #region Properties

        public Tab ActiveTab { get; set; }

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Board:
                default:
                    OpenBoardPageAsync();
                    break;
                case Tab.Leader:
                    NavigationService.Navigate<LeaderNominationPageViewModel>(this);
                    break;
                case Tab.Settings:
                    NavigationService.Navigate<ProjectSettingsPageViewModel>(this);
                    break;
            }
        }

        [Model]
        public Project Project { get; set; }

        public User Leader { get; set; }

        #endregion

        #region Commands

        public TaskCommand OpenTeamPage { get; private set; }

        private async Task OnOpenTeamPageExecuteAsync()
        {
            StateManager.SelectedTeam = Project.Team;

            var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
            vm.ActiveTab = ApplicationWindowViewModel.Tab.Team;
        }

        #endregion

        #region Methods

        protected async void OpenBoardPageAsync()
        {
            var board = await LoadActiveBoardAsync();
            if (board != null) {
                StateManager.SelectedBoard = board;
                NavigationService.Navigate<BoardPageViewModel>(this);
            } else {
                NavigationService.Navigate<BoardsPageViewModel>(this);
            }
        }

        private async Task<Board> LoadActiveBoardAsync()
        {
            if (!SettingsManager.FavoriteBoards.ContainsKey(Project.Id)) {
                return null;
            }

            try {
                var response = await BoardService.GetBoardAsync(SettingsManager.FavoriteBoards[Project.Id]);

                return response.Object;
            } catch (NotFoundHttpException) {
                SettingsManager.FavoriteBoards.Remove(Project.Id);
                Properties.Settings.Default.Save();
            }

            return null;
        }

        protected async void LoadProjectLeaderAsync()
        {
            try {
                var response = await ProjectService.GetProjectLeaderAsync(StateManager.SelectedProject);

                Leader = response.Object;
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        protected async Task LoadProjectTeamAsync()
        {
            if (Project.Team != null) {
                return;
            }

            try {
                var response = await ProjectService.GetTeamForProjectAsync(StateManager.SelectedProject);

                Project.Team = response.Object;
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        private void SaveRecentProject()
        {
            SettingsManager.RecentProjects.AddOrReplace(Project.Id);
            Properties.Settings.Default.Save();
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LoadProjectLeaderAsync();
            await LoadProjectTeamAsync();

            OnActiveTabChanged();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
