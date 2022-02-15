using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class ProgectsPageViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        public ProgectsPageViewModel(IUIVisualizerService uIVisualizerService)
        {
            UIVisualizerService = uIVisualizerService;

            OpenCreateNewTeamDataWindow = new TaskCommand(OnOpenCreateNewTeamDataWindowExecute);
        }

        #region Properties



        #endregion

        #region Commands


        public TaskCommand OpenCreateNewTeamDataWindow { get; private set; }


        private async Task OnOpenCreateNewTeamDataWindowExecute()
        {
            await UIVisualizerService.ShowAsync<CreateNewTeamDataWindowViewModel>();
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
