using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Security.Principal;
using System.Text;

using MaterialDesignThemes.Wpf;

namespace Equality.Services
{
    public class ThemeService : IThemeService
    {
        private IThemeService.Theme _currentTheme { get; set; }

        public IThemeService.Theme GetCurrentTheme()
        {
            return _currentTheme;
        }

        private readonly PaletteHelper _paletteHelper = new();

        public void SetTheme(IThemeService.Theme theme)
        {
            var currentTheme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = new MaterialDesignLightTheme();
            switch (theme) {
                case IThemeService.Theme.Light:
                    Properties.Settings.Default.current_theme = (int)IThemeService.Theme.Light;

                    baseTheme = new MaterialDesignLightTheme();

                    break;
                case IThemeService.Theme.Dark:
                    Properties.Settings.Default.current_theme = (int)IThemeService.Theme.Dark;

                    baseTheme = new MaterialDesignDarkTheme();

                    break;
                case IThemeService.Theme.Sync:
                    string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
                    var query = new WqlEventQuery(string.Format(
                    "SELECT * FROM RegistryValueChangeEvent WHERE Hive='HKEY_USERS' AND KeyPath='{0}\\\\{1}' AND ValueName='{2}'",
                    WindowsIdentity.GetCurrent().User.Value, keyPath.Replace("\\", "\\\\"), "AppsUseLightTheme"));
                    var _watcher = new ManagementEventWatcher(query);
                    _watcher.EventArrived += (sender, args) => LiveThemeChanging();
                    _watcher.Start();

                    Properties.Settings.Default.current_theme = (int)IThemeService.Theme.Sync;
                    break;
            }
            currentTheme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(currentTheme);
            Properties.Settings.Default.Save();
        }

        private void LiveThemeChanging()
        {
            Debug.Write("Change");
        }
    }
}
