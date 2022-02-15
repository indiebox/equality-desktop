using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class ProjectsPageViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        public ProjectsPageViewModel(IUIVisualizerService uIVisualizerService)
        {
            UIVisualizerService = uIVisualizerService;

            OpenCreateTeamWindow = new TaskCommand(OnOpenCreateTeamWindowExecute);
        }

        #region Properties



        #endregion

        #region Commands

        public TaskCommand OpenCreateTeamWindow { get; private set; }

        private async Task OnOpenCreateTeamWindowExecute()
        {
            await UIVisualizerService.ShowAsync<CreateTeamDataWindowViewModel>();
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
