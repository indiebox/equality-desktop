using System.Threading.Tasks;

using Equality.MVVM;
using Equality.Services;

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
