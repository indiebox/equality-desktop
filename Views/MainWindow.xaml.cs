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

            // Change MainWindow and close previous one.
            var temp = App.Current.MainWindow;
            App.Current.MainWindow = this;
            temp.Close();
        }
    }
}
