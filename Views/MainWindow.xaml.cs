using System.Windows;

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

            Activated += MainWindow_Activated;
        }

        private void MainWindow_Activated(object sender, System.EventArgs e)
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
