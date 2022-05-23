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
        }

        public enum Tab
        {
            Board,
            Leader,
            Settings,
        }

        #region Properties

        public Tab ActiveTab { get; set; }

        [Model]
        public Project Project { get; set; }

        public User Leader { get; set; }

        #endregion

        #region Commands

        protected async Task LoadProjectLeaderAsync()
        {
            try {
                var response = await ProjectService.GetProjectLeaderAsync(StateManager.SelectedProject);

                Leader = response.Object;
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        #region Methods

        protected async void OpenBoardPageAsync()
        {
            var board = await LoadActiveBoard();
            if (board != null) {
                StateManager.SelectedBoard = board;
                NavigationService.Navigate<BoardPageViewModel, ProjectPageViewModel>();
            } else {
                NavigationService.Navigate<BoardsPageViewModel>(this);
            }
        }

        private async Task<Board> LoadActiveBoard()
        {
            string projectId = Project.Id.ToString();

            if (!SettingsManager.ActiveBoards.ContainsKey(projectId)) {
                return null;
            }

            try {
                var response = await BoardService.GetBoardAsync(SettingsManager.ActiveBoards[projectId]);

                return response.Object;
            } catch (NotFoundHttpException) {
                SettingsManager.ActiveBoards.Remove(projectId);
                Properties.Settings.Default.Save();
            }

            return null;
        }

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

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadProjectLeaderAsync();

            OnActiveTabChanged();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
