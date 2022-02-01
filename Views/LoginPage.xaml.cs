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
            if (DataContext
                == null) {
                return;
            }
            ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
