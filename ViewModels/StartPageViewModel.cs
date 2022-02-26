using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class StartPageViewModel : ViewModel
    {
        IUIVisualizerService UIVisualizerService;

        public StartPageViewModel(IUIVisualizerService uIVisualizerService)
        {
            UIVisualizerService = uIVisualizerService;

            OpenInvitationsWindow = new TaskCommand(OnInvitationsWindowExecute);
        }

        #region Properties


        #endregion

        #region Commands

        public TaskCommand OpenInvitationsWindow { get; private set; }

        private async Task OnInvitationsWindowExecute()
        {
            await UIVisualizerService.ShowAsync<InvitationsDataWindowViewModel>();
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
