using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Media;

using Equality;

using MaterialDesignThemes.Wpf;

using Microsoft.Win32;

namespace Equality.Services
{
    public class ThemeService : IThemeService
    {
        private IThemeService.Theme _currentTheme { get; set; }

        ManagementEventWatcher Watcher;

        private readonly PaletteHelper _paletteHelper = new();

        public ThemeService()
        {
            SetTheme();
        }
        private void SetTheme()
        {
            var helper = new PaletteHelper();
            if (helper.GetThemeManager() is { } themeManager) {
                themeManager.ThemeChanged += (sender, e) =>
                {
                    if (e.NewTheme.GetBaseTheme() == BaseTheme.Light) {
                        SetLightTheme();
                    } else {
                        SetDarkTheme();
                    }
                };
            }

            CreateWatcher();
        }

        private void SetLightTheme()
        {
            ((App)Application.Current).Resources["SecondaryBackgroundColor"] = new SolidColorBrush(Colors.WhiteSmoke);
            ((App)Application.Current).Resources["MaterialDesignPaper"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAFA"));
            ((App)Application.Current).Resources["PrimaryHueMidForegroundBrush"] = new SolidColorBrush(Colors.Black);
            ((App)Application.Current).Resources["GrayColorOnHover"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCCCC"));
        }

        private void SetDarkTheme()
        {
            ((App)Application.Current).Resources["SecondaryBackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
            ((App)Application.Current).Resources["MaterialDesignPaper"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212121"));
            ((App)Application.Current).Resources["PrimaryHueMidForegroundBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#858585"));
            ((App)Application.Current).Resources["GrayColorOnHover"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#373737"));
        }

        private void CreateWatcher()
        {
            string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            var query = new WqlEventQuery(string.Format(
            "SELECT * FROM RegistryValueChangeEvent WHERE Hive='HKEY_USERS' AND KeyPath='{0}\\\\{1}' AND ValueName='{2}'",
            WindowsIdentity.GetCurrent().User.Value, keyPath.Replace("\\", "\\\\"), "AppsUseLightTheme"));
            var _watcher = new ManagementEventWatcher(query);
            _watcher.EventArrived += (sender, args) => LiveThemeChanging();
            Watcher = _watcher;
        }

        public IThemeService.Theme GetCurrentTheme()
        {
            return _currentTheme;
        }

        public void SetColorTheme(IThemeService.Theme theme)
        {
            IBaseTheme baseTheme = theme switch
            {
                IThemeService.Theme.Dark => new MaterialDesignDarkTheme(),
                _ => new MaterialDesignLightTheme()
            };
            switch (theme) {
                case IThemeService.Theme.Light:
                    _currentTheme = IThemeService.Theme.Light;
                    Watcher.Stop();
                    break;
                case IThemeService.Theme.Dark:
                    _currentTheme = IThemeService.Theme.Dark;
                    Watcher.Stop();
                    break;
                case IThemeService.Theme.Sync:
                    _currentTheme = IThemeService.Theme.Sync;
                    Properties.Settings.Default.current_theme = (int)_currentTheme;
                    Properties.Settings.Default.Save();
                    Watcher.Start();
                    LiveThemeChanging();
                    return;
            }
            Properties.Settings.Default.current_theme = (int)_currentTheme;
            Properties.Settings.Default.Save();
            var currentTheme = _paletteHelper.GetTheme();
            currentTheme.SetBaseTheme(baseTheme);
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _paletteHelper.SetTheme(currentTheme);
            });
        }

        private void LiveThemeChanging()
        {
            var currentTheme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = new MaterialDesignLightTheme();
            if (Theme.GetSystemTheme() == BaseTheme.Dark) {
                baseTheme = new MaterialDesignDarkTheme();
            }
            currentTheme.SetBaseTheme(baseTheme);

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _paletteHelper.SetTheme(currentTheme);
            });
        }
    }
}
