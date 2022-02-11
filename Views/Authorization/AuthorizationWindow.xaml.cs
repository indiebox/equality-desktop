using System.Windows;

namespace Equality.Views
{
    public partial class AuthorizationWindow
    {
        public AuthorizationWindow()
        {
            InitializeComponent();

            Activated += AuthorizationWindow_Activated;
        }

        private void AuthorizationWindow_Activated(object sender, System.EventArgs e)
        {
            if (!ReferenceEquals(App.Current.MainWindow, this)) {
                App.Current.MainWindow.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                App.Current.MainWindow = this;
            }

            if (!IsVisible) {
                SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }
        }
    }
}
