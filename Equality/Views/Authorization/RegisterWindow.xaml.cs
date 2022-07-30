using CefSharp;
using CefSharp.Wpf;

using Equality.ViewModels;

namespace Equality.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterWindow
    {
        public RegisterWindow()
        {
            // Setup browser language.
            if (!Cef.IsInitialized) {
                CefSettings settingsBrowser = new();
                settingsBrowser.Locale = "ru";
                Cef.Initialize(settingsBrowser);
            }

            InitializeComponent();

            // Disable context menu in browser by click RMB.
            Browser.MenuHandler = new Browser.ContextMenuHandler();

            // Hide loading screen on page loaded.
            Browser.FrameLoadEnd += OnFrameLoadEnd;
        }

        private void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain) {
                App.Current.Dispatcher.Invoke(delegate
                {
                    ((RegisterWindowViewModel)DataContext).IsBrowserLoaded = true;
                });
            }
        }
    }
}
