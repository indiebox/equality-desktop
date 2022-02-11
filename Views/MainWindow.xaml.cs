namespace Equality.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            // We need setup each dedicated window as MainWindow when we close previous one.
            App.Current.MainWindow = this;
        }
    }
}
