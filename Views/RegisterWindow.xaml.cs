using CefSharp;
using CefSharp.Wpf;

using Equality.Core.CefSharp;

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
            CefSettings settingsBrowser = new();
            settingsBrowser.Locale = "ru";
            Cef.Initialize(settingsBrowser);

            InitializeComponent();

            // Disable context menu in browser by click RMB.
            Browser.MenuHandler = new CustomMenuHandler();
        }
    }
}
