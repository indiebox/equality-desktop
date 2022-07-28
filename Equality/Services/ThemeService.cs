using System.Management;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

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
            var app = (App)Application.Current;

            app.Resources["SecondaryBackgroundColor"] = new SolidColorBrush(Colors.WhiteSmoke);
            app.Resources["MaterialDesignPaper"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAFA"));
            app.Resources["PrimaryHueMidForegroundBrush"] = new SolidColorBrush(Colors.Black);
            app.Resources["GrayColorOnHover"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCCCC"));
        }

        private void SetDarkTheme()
        {
            var app = (App)Application.Current;

            app.Resources["SecondaryBackgroundColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626"));
            app.Resources["MaterialDesignPaper"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212121"));
            app.Resources["PrimaryHueMidForegroundBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#858585"));
            app.Resources["GrayColorOnHover"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#373737"));
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

            _currentTheme = theme;
            Properties.Settings.Default.current_theme = (int)_currentTheme;
            Properties.Settings.Default.Save();

            if (_currentTheme == IThemeService.Theme.Sync) {
                Watcher.Start();
                LiveThemeChanging();
            } else {
                Watcher.Stop();
                SetColorThemeInternal(baseTheme);
            }
        }

        private void SetColorThemeInternal(IBaseTheme baseTheme)
        {
            var currentTheme = _paletteHelper.GetTheme();
            currentTheme.SetBaseTheme(baseTheme);
            Application.Current.Dispatcher.Invoke(() =>
            {
                _paletteHelper.SetTheme(currentTheme);
            });
        }

        private void LiveThemeChanging()
        {
            IBaseTheme baseTheme = Theme.GetSystemTheme() switch
            {
                BaseTheme.Dark => new MaterialDesignDarkTheme(),
                _ => new MaterialDesignLightTheme()
            };
            SetColorThemeInternal(baseTheme);
        }
    }
}
