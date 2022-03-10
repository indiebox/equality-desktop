using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Extensions;
using Equality.MVVM;
using Equality.Models;
using Equality.Data;

namespace Equality.ViewModels
{
    public class ProjectPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        public ProjectPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

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

        #endregion

        #region Commands



        #endregion

        #region Methods

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Board:
                default:
                    break;
                case Tab.Leader:
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

            OnActiveTabChanged();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
