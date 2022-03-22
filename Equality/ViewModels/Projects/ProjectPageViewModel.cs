using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Extensions;
using Equality.MVVM;
using Equality.Models;
using Equality.Data;
using Equality.Services;
using System.Net.Http;
using System.Diagnostics;

namespace Equality.ViewModels
{
    public class ProjectPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IProjectService ProjectService;

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

        public ProjectPageViewModel(INavigationService navigationService, IProjectService projectService)
        {
            NavigationService = navigationService;
            ProjectService = projectService;

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

        protected async Task LoadLeaderNominationsAsync()
        {
            try {
                var response = await ProjectService.GetProjectLeaderAsync(StateManager.SelectedProject);

                Leader = response.Object;
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Methods

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Board:
                default:
                    NavigationService.Navigate<BoardPageViewModel>(this);
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

            await LoadLeaderNominationsAsync();

            OnActiveTabChanged();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
