using Equality.Core.Extensions;

namespace Equality.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ApplicationWindow
    {
        public ApplicationWindow()
        {
            InitializeComponent();

            this.SetAsMainWindow();
        }
    }
}
