using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Equality.Models;
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

        private void BoardPage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Vm = (BoardPageViewModel)DataContext;
        }

        #region Drag&Drop

        BoardPageViewModel Vm { get; set; }

        int DragColumnInitialPosition { get; set; }

        bool IsDragging => Vm.DragColumn != null;

        public Point DeltaMouse { get; set; }

        public Point ColumnRelativePoint { get; set; }

        private void ColumnControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDragging || e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            Vm.DragColumn = ((FrameworkElement)sender).DataContext as Column;
            DragColumnInitialPosition = Vm.Columns.IndexOf(Vm.DragColumn);

            DeltaMouse = Mouse.GetPosition(DraggingCanvas);
            ColumnRelativePoint = ((FrameworkElement)sender).TransformToAncestor(this).Transform(new Point(0, 0));
            Canvas.SetLeft(MovingColumn, ColumnRelativePoint.X);
            Canvas.SetTop(MovingColumn, ColumnRelativePoint.Y);
        }

        private async void ColumnControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!IsDragging) {
                return;
            }

            var column = ((FrameworkElement)sender).DataContext as Column;
            int oldIndex = Vm.Columns.IndexOf(column);
            int dragColumnIndex = Vm.Columns.IndexOf(Vm.DragColumn);

            Vm.Columns.Move(oldIndex, dragColumnIndex);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDragging) {
                return;
            }

            var cursorPosition = Mouse.GetPosition(DraggingCanvas);
            Canvas.SetLeft(MovingColumn, ColumnRelativePoint.X + (cursorPosition.X - DeltaMouse.X));
            Canvas.SetTop(MovingColumn, ColumnRelativePoint.Y + (cursorPosition.Y - DeltaMouse.Y));
        }

        #region StopDrag

        private void Page_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsDragging) {
                return;
            }

            StopDragging();
        }

        private void Page_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsDragging) {
                return;
            }

            StopDragging();
        }

        private void StopDragging()
        {
            if (DragColumnInitialPosition != Vm.Columns.IndexOf(Vm.DragColumn)) {
                Vm.UpdateColumnOrder.Execute();
            }

            Vm.DragColumn = null;
        }

        #endregion StopDrag

        #endregion

        #region Horizontal scroll

        protected double ScrollInitialPosition { get; set; } = -1;

        protected double ScrollInitialOffset { get; set; } = 0;

        protected ScrollViewer ColumnsScrollViewer { get; set; }

        protected bool IsScrolling => ScrollInitialPosition != -1;

        private void ListBoxColumns_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.DirectlyOver is not Grid grid || grid.Parent != null) {
                return;
            }

            ScrollInitialPosition = Mouse.GetPosition(this).X;
            Decorator border = VisualTreeHelper.GetChild(ListBoxColumns, 0) as Decorator;
            if (border != null) {
                ColumnsScrollViewer = border.Child as ScrollViewer;
                ScrollInitialOffset = ColumnsScrollViewer.HorizontalOffset;
            }
        }

        private void ListBoxColumns_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ScrollInitialPosition = -1;
            ColumnsScrollViewer = null;
            ScrollInitialOffset = 0;
        }

        private void ListBoxColumns_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsScrolling || ColumnsScrollViewer == null) {
                return;
            }

            ColumnsScrollViewer.ScrollToHorizontalOffset(ScrollInitialOffset + ScrollInitialPosition - Mouse.GetPosition(this).X);
        }

        #endregion
    }
}
