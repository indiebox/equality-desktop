using System;
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

        public enum Themes
        {
            Light,
            Dark,
            Sync,
        }

        protected IThemeService ThemeService;

        public SettingsPageViewModel(IThemeService themeService)
        {
            int currentThemeString = Properties.Settings.Default.current_theme;

            ThemeService = themeService;

            ChangeTheme = new Command<string>(OnChangeThemeExecute);
            var currentTheme = (IThemeService.Theme)currentThemeString;
            ThemeService.SetTheme(currentTheme);
        }

        #region Methods

        #endregion

        #region Properties

        public Themes ActiveTheme { get; set; }

        #endregion

        #region Commands


        public Command<string> ChangeTheme { get; private set; }

        private void OnChangeThemeExecute(string theme)
        {
            // TODO: change enum
            var themeEnum = (IThemeService.Theme)Enum.Parse(typeof(IThemeService.Theme), theme);

            // The foo.ToString().Contains(",") check is necessary for enumerations marked with an [Flags] attribute
            if (!Enum.IsDefined(typeof(IThemeService.Theme), themeEnum) && !themeEnum.ToString().Contains(",")) {
                throw new InvalidOperationException($"{theme} is not an underlying value of the YourEnum enumeration.");
            }

            ThemeService.SetTheme(themeEnum);
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
