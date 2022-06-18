using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Security.Principal;
using System.Threading.Tasks;

using Catel.MVVM;

using Catel.Services;

using Equality.Data;
using Equality.MVVM;
using Equality.Services;

using MaterialDesignThemes.Wpf;

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

        protected IThemeService ThemeService;

        public SettingsPageViewModel(IThemeService themeService)
        {
            ThemeService = themeService;

            _activeTheme = ThemeService.GetCurrentTheme();
        }

        #region Methods

        #endregion

        #region Properties

        private IThemeService.Theme _activeTheme;

        public IThemeService.Theme ActiveTheme
        {
            get {
                return _activeTheme;
            }
            set {
                _activeTheme = value;
                ThemeService.SetColorTheme(_activeTheme);
            }
        }

        #endregion

        #region Commands

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
