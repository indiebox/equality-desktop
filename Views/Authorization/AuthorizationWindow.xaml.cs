namespace Equality.Views
{
    public partial class AuthorizationWindow
    {
        public AuthorizationWindow()
        {
            InitializeComponent();

            // Change MainWindow and close previous one.
            var temp = App.Current.MainWindow;
            App.Current.MainWindow = this;
            temp.Close();
        }
    }
}
