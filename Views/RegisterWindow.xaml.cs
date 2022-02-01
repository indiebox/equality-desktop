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
            InitializeComponent();

            // Disable context menu in browser by click RMB.
            Browser.MenuHandler = new CustomMenuHandler();
        }
    }
}
