using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        BoardPageViewModel Vm { get; set; }

        int DragColumnInitialPosition { get; set; }

        int DragCardInitialPosition { get; set; }

        bool IsDraggingColumn => Vm.DragColumn != null;

        bool IsDraggingCard => Vm.DragCard != null;

        public Point DeltaMouse { get; set; }

        public Point ColumnRelativePoint { get; set; }

        public Point CardRelativePoint { get; set; }

        private void ColumnControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDraggingColumn || e.LeftButton != MouseButtonState.Pressed) {
                return;
            }
            Vm.DragColumn = ((ContentControl)sender).Content as Column;
            DragColumnInitialPosition = Vm.Columns.IndexOf(Vm.DragColumn);

            DeltaMouse = Mouse.GetPosition(DraggingCanvas);
            ColumnRelativePoint = ((ContentControl)sender).TransformToAncestor(this).Transform(new Point(0, 0));
            Canvas.SetLeft(MovingColumn, ColumnRelativePoint.X);
            Canvas.SetTop(MovingColumn, ColumnRelativePoint.Y);
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDraggingCard || e.LeftButton != MouseButtonState.Pressed) {
                return;
            }
            Vm.DragCard = ((ContentControl)sender).Content as Card;
            DragCardInitialPosition = (from column in Vm.Columns
                                       where column.Cards.Contains(Vm.DragCard)
                                       select column.Cards.IndexOf(Vm.DragCard))
                                      .First();

            DeltaMouse = Mouse.GetPosition(DraggingCanvas);
            CardRelativePoint = ((ContentControl)sender).TransformToAncestor(this).Transform(new Point(0, 0));
            Canvas.SetLeft(MovingCard, CardRelativePoint.X);
            Canvas.SetTop(MovingCard, CardRelativePoint.Y);
        }

        private async void ColumnControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!IsDraggingColumn) {
                return;
            }
            var column = ((ContentControl)sender).Content as Column;

            int oldIndex = Vm.Columns.IndexOf(column);
            int dragColumnIndex = Vm.Columns.IndexOf(Vm.DragColumn);

            Vm.Columns.Move(oldIndex, dragColumnIndex);
        }

        private async void CardControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!IsDraggingCard) {
                return;
            }
            var card = ((ContentControl)sender).Content as Card;

            int oldIndex = (from column in Vm.Columns
                            where column.Cards.Contains(card)
                            select column.Cards.IndexOf(card))
                                      .First();
            int dragColumnIndex = (from column in Vm.Columns
                                   where column.Cards.Contains(Vm.DragCard)
                                   select column.Cards.IndexOf(Vm.DragCard))
                                      .First();

            (from column in Vm.Columns
             where column.Cards.Contains(Vm.DragCard)
             select column.Cards).First().Move(oldIndex, dragColumnIndex);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDraggingCard) {
                var cursorPosition = Mouse.GetPosition(DraggingCanvas);
                Canvas.SetLeft(MovingCard, CardRelativePoint.X + (cursorPosition.X - DeltaMouse.X));
                Canvas.SetTop(MovingCard, CardRelativePoint.Y + (cursorPosition.Y - DeltaMouse.Y));
            } else if (IsDraggingColumn) {
                var cursorPosition = Mouse.GetPosition(DraggingCanvas);
                Canvas.SetLeft(MovingColumn, ColumnRelativePoint.X + (cursorPosition.X - DeltaMouse.X));
                Canvas.SetTop(MovingColumn, ColumnRelativePoint.Y + (cursorPosition.Y - DeltaMouse.Y));
            }
        }

        #region StopDrag

        private void Page_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsDraggingColumn && !IsDraggingCard) {
                return;
            }

            StopDragging();
        }

        private void Page_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!IsDraggingColumn && !IsDraggingCard) {
                return;
            }

            StopDragging();
        }

        private void StopDragging()
        {
            if (DragColumnInitialPosition != Vm.Columns.IndexOf(Vm.DragColumn)) {
                Vm.UpdateColumnOrder.Execute();
            } else if (DragCardInitialPosition != (from column in Vm.Columns
                                                   where column.Cards.Contains(Vm.DragCard)
                                                   select column.Cards).First().IndexOf(Vm.DragCard)) {
                Vm.UpdateCardOrder.Execute();
            }

            Vm.DragColumn = null;
            Vm.DragCard = null;
        }

        #endregion
    }
}
