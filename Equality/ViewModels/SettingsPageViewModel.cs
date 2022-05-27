using System.Diagnostics;
using System.Threading.Tasks;

using Catel.MVVM;

using Catel.Services;

using Equality.Data;
using Equality.MVVM;

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

        public SettingsPageViewModel(INavigationService navigationService)
        {
            string currentThemeString = Properties.Settings.Default.current_theme;
            ChangeTheme = new Command<string>(OnChangeThemeExecute);

            if (currentThemeString == null) {
                currentThemeString = "Light";
            }
            switch (currentThemeString) {
                case "Light":
                    ActiveTheme = Themes.Light;
                    break;
                case "Dark":
                    ActiveTheme = Themes.Dark;
                    break;
                case "Sync":
                    ActiveTheme = Themes.Sync;
                    break;
            }
        }

        public enum Themes
        {
            Light,
            Dark,
            Sync,
        }

        #region Methods

        private void OnActiveThemeChanged(string newTheme)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = new MaterialDesignLightTheme();
            switch (newTheme) {
                case "Light":
                    ActiveTheme = Themes.Light;
                    Properties.Settings.Default.current_theme = "Light";

                    baseTheme = new MaterialDesignLightTheme();

                    break;
                case "Dark":
                    ActiveTheme = Themes.Dark;
                    Properties.Settings.Default.current_theme = "Dark";

                    baseTheme = new MaterialDesignDarkTheme();

                    break;
                case "Sync":
                    ActiveTheme = Themes.Sync;
                    Properties.Settings.Default.current_theme = "Sync";
                    baseTheme = StateManager.GetColorTheme() == "Light" ? new MaterialDesignLightTheme() : new MaterialDesignDarkTheme();
                    break;
            }
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
            Properties.Settings.Default.Save();
        }

        #endregion

        #region Properties

        public Themes ActiveTheme { get; set; }

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        #endregion

        #region Commands


        public Command<string> ChangeTheme { get; private set; }

        private void OnChangeThemeExecute(string theme)
        {
            OnActiveThemeChanged(theme);
        }

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
