﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        public ColumnControl DragColumn { get; set; }

        public Point DeltaMouse { get; set; }

        public Point ColumnRelativePoint { get; set; }

        private void ColumnControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDragging || e.LeftButton != MouseButtonState.Pressed) {
                return;
            }
            DragColumn = ParseControl(sender);
            Vm.DragColumn = ParseControl(sender);
            DragColumn.SetCurrentValue(ColumnControl.IsDraggingProperty, true);
            Vm.DragColumn.SetCurrentValue(ColumnControl.IsDraggingProperty, true);

            DeltaMouse = Mouse.GetPosition(DraggingCanvas);
            ColumnRelativePoint = DragColumn.TransformToAncestor(this).Transform(new Point(0, 0));
            Canvas.SetLeft(MovingColumn, ColumnRelativePoint.X);
            Canvas.SetTop(MovingColumn, ColumnRelativePoint.Y);
        }

        private async void ColumnControl_MouseEnter(object sender, MouseEventArgs e)
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

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragColumn == null) {
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
            DragColumn.SetCurrentValue(ColumnControl.IsDraggingProperty, false);
            DragColumn = null;
            Vm.DragColumn.SetCurrentValue(ColumnControl.IsDraggingProperty, false);
            Vm.DragColumn = null;
        }

        #endregion
    }
}
