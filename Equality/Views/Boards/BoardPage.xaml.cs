using System.Windows.Input;

using Equality.Controls;
using Equality.ViewModels;

namespace Equality.Views
{
    public partial class BoardPage
    {
        public BoardPage()
        {
            InitializeComponent();

            DataContextChanged += BoardPage_DataContextChanged;
        }

        private void BoardPage_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Vm = (BoardPageViewModel)DataContext;
        }

        BoardPageViewModel Vm { get; set; }

        bool IsDragging => DragColumn != null;

        ColumnControl DragColumn { get; set; }

        private void ColumnControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDragging || e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            DragColumn = ParseControl(sender);
            DragColumn.SetCurrentValue(ColumnControl.IsDraggingProperty, true);
        }

        private void ColumnControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!IsDragging) {
                return;
            }

            var column = ParseControl(sender).Column;

            int oldIndex = Vm.Columns.IndexOf(column);
            int dragColumnIndex = Vm.Columns.IndexOf(DragColumn.Column);

            Vm.Columns.Move(oldIndex, dragColumnIndex);
        }

        private ColumnControl ParseControl(object sender)
        {
            return sender as ColumnControl;
        }

        private void Page_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsDragging) {
                return;
            }

            DragColumn.SetCurrentValue(ColumnControl.IsDraggingProperty, false);
            DragColumn = null;
        }
    }
}
