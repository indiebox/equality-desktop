using System.Threading.Tasks;

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

        public enum Themes
        {
            Light,
            Dark,
            Sync,
        }

        #region Methods

        private void OnActiveThemeChanged()
        {
            switch (ActiveTheme) {
                case Themes.Light:
                    break;
                case Themes.Dark:
                    break;
                case Themes.Sync:
                    break;
            }
        }

        #endregion

        #region Properties

        public Themes ActiveTheme { get; set; }

        #endregion

        #region Commands



        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            OnActiveThemeChanged();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
