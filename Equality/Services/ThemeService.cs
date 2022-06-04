﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;

using MaterialDesignThemes.Wpf;

using Microsoft.Win32;

namespace Equality.Services
{
    public class ThemeService : IThemeService
    {
        private IThemeService.Theme _currentTheme { get; set; }

        delegate void SyncTheme();

        ManagementEventWatcher Watcher;

        private readonly PaletteHelper _paletteHelper = new();

        public ThemeService()
        {
            string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            var query = new WqlEventQuery(string.Format(
            "SELECT * FROM RegistryValueChangeEvent WHERE Hive='HKEY_USERS' AND KeyPath='{0}\\\\{1}' AND ValueName='{2}'",
            WindowsIdentity.GetCurrent().User.Value, keyPath.Replace("\\", "\\\\"), "AppsUseLightTheme"));
            var _watcher = new ManagementEventWatcher(query);
            _watcher.EventArrived += (sender, args) => LiveThemeChanging();
            Watcher = _watcher;

            _currentTheme = (IThemeService.Theme)Properties.Settings.Default.current_theme;
        }

        public IThemeService.Theme GetCurrentTheme()
        {
            return _currentTheme;
        }

        public void SetColorTheme(IThemeService.Theme theme)
        {
            IBaseTheme baseTheme = new MaterialDesignLightTheme();
            switch (theme) {
                case IThemeService.Theme.Light:
                    _currentTheme = IThemeService.Theme.Light;
                    Properties.Settings.Default.current_theme = (int)IThemeService.Theme.Light;
                    Watcher.Stop();

                    break;
                case IThemeService.Theme.Dark:
                    _currentTheme = IThemeService.Theme.Light;
                    Properties.Settings.Default.current_theme = (int)IThemeService.Theme.Dark;
                    baseTheme = new MaterialDesignDarkTheme();
                    Watcher.Stop();
                    break;
                case IThemeService.Theme.Sync:
                    _currentTheme = IThemeService.Theme.Sync;
                    Properties.Settings.Default.current_theme = (int)IThemeService.Theme.Sync;
                    Properties.Settings.Default.Save();
                    Watcher.Start();
                    LiveThemeChanging();
                    return;
            }
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
            string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            int theme = (int)Registry.GetValue(RegistryKey, "AppsUseLightTheme", string.Empty);
            if (theme == 0) {
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