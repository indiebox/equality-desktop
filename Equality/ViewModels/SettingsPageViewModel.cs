﻿using System.Threading.Tasks;

using Catel.Services;

using Equality.MVVM;

namespace Equality.ViewModels
{
    public class SettingsPageViewModel : ViewModel
    {
        #region DesignModeConstructor

        public SettingsPageViewModel()
        {
            HandleDesignMode();
        }

        #endregion

        public SettingsPageViewModel(INavigationService navigationService)
        {
        }

        #region Properties



        #endregion

        #region Commands



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
