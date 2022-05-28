using System.Diagnostics;
using System.Management;
using System.Security.Principal;
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

        public enum Themes
        {
            Light,
            Dark,
            Sync,
        }

        public SettingsPageViewModel(INavigationService navigationService)
        {
            int currentThemeString = Properties.Settings.Default.current_theme;
            ChangeTheme = new Command<string>(OnChangeThemeExecute);

            switch (currentThemeString) {
                case (int)Themes.Light:
                    ActiveTheme = Themes.Light;
                    break;
                case (int)Themes.Dark:
                    ActiveTheme = Themes.Dark;
                    break;
                case (int)Themes.Sync:
                    ActiveTheme = Themes.Sync;
                    break;
                default:
                    currentThemeString = (int)Themes.Light;
                    break;
            }
        }

        #region Methods

        private void OnActiveThemeChanged(string newTheme)
        {
            var theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = new MaterialDesignLightTheme();
            switch (newTheme) {
                case "Light":
                    ActiveTheme = Themes.Light;
                    Properties.Settings.Default.current_theme = (int)Themes.Light;

                    baseTheme = new MaterialDesignLightTheme();

                    break;
                case "Dark":
                    ActiveTheme = Themes.Dark;
                    Properties.Settings.Default.current_theme = (int)Themes.Dark;

                    baseTheme = new MaterialDesignDarkTheme();

                    break;
                case "Sync":
                    var currentUser = WindowsIdentity.GetCurrent();
                    var query = new WqlEventQuery("SELECT * FROM RegistryTreeChangeEvent WHERE " +
                                    "Hive = 'HKEY_USERS' " +
                                     @"AND RootPath = '" + currentUser.User.Value + @"\\Software'");
                    var _watcher = new ManagementEventWatcher(query);
                    _watcher.EventArrived += (sender, args) => LiveThemeChanging();
                    _watcher.Start();

                    ActiveTheme = Themes.Sync;
                    Properties.Settings.Default.current_theme = (int)Themes.Sync;
                    //baseTheme = StateManager.GetColorTheme() == (int)Themes.Light ? new MaterialDesignLightTheme() : new MaterialDesignDarkTheme();
                    baseTheme = StateManager.GetColorTheme() == (int)Themes.Light ? new MaterialDesignLightTheme() : new MaterialDesignDarkTheme();
                    break;
            }
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
            Properties.Settings.Default.Save();
        }

        private void LiveThemeChanging()
        {
            Debug.Write("Change");
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
