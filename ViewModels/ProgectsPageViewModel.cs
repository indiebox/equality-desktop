using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Catel.MVVM;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class ProgectsPageViewModel : ViewModel
    {
        public ProgectsPageViewModel()
        {
            OpenCreateNewTeamDataWindow = new Command(OnOpenCreateNewTeamDataWindowExecute);
        }

        #region Properties



        #endregion

        #region Commands


        public Command OpenCreateNewTeamDataWindow { get; private set; }


        private void OnOpenCreateNewTeamDataWindowExecute()
        {

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
