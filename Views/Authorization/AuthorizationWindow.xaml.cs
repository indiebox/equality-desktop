namespace Equality.Views
{
    public partial class AuthorizationWindow
    {
        public AuthorizationWindow()
        {
            InitializeComponent();

            // We need setup each dedicated window as MainWindow when we close previous one.
            App.Current.MainWindow = this;
        }
    }
}
