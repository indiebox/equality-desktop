using System.Windows.Controls;

namespace Equality.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Password_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext
                == null) {
                return;
            }
            ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword;
        }
    }
}
