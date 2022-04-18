using System.Diagnostics;
using System.Linq;
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
            ListBoxColumns.Loaded += ListBoxColumns_Loaded;
        }

        protected BoardPageViewModel Vm { get; set; }

        private void BoardPage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Vm = (BoardPageViewModel)DataContext;
        }

        private void ListBoxColumns_Loaded(object sender, RoutedEventArgs e)
        {
            ColumnsScrollViewer = (VisualTreeHelper.GetChild(ListBoxColumns, 0) as Decorator).Child as ScrollViewer;
        }

        #region Drag&Drop

        protected int DragColumnInitialPosition { get; set; }

        int DragCardInitialPosition { get; set; }

        Column DragCardInitialColumn { get; set; }

        bool IsDraggingColumn => Vm?.DragColumn != null;

        bool IsDraggingCard => Vm?.DragCard != null;

        public Point DeltaMouse { get; set; }

        public Point ColumnRelativePoint { get; set; }

        public Point CardRelativePoint { get; set; }

        private void ColumnControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDraggingColumn || e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            Vm.DragColumn = ((FrameworkElement)sender).DataContext as Column;
            DragColumnInitialPosition = Vm.Columns.IndexOf(Vm.DragColumn);

            DeltaMouse = Mouse.GetPosition(DraggingCanvas);
            ColumnRelativePoint = ((FrameworkElement)sender).TransformToAncestor(this).Transform(new Point(0, 0));

            Canvas.SetLeft(MovingColumn, ColumnRelativePoint.X);
            Canvas.SetTop(MovingColumn, ColumnRelativePoint.Y);
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDraggingCard || e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            Vm.DragCard = ((ContentControl)sender).Content as Card;
            Vm.DraggableCardColumn = Vm.Columns.First(column => column.Cards.Contains(Vm.DragCard));
            DragCardInitialColumn = Vm.DraggableCardColumn;
            DragCardInitialPosition = Vm.DraggableCardColumn.Cards.IndexOf(Vm.DragCard);

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
            var column = ((FrameworkElement)sender).DataContext as Column;
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
            Point MousePosition = Mouse.GetPosition(this);
            double CardPositionY = ((ContentControl)sender).TransformToAncestor(this)
                                .Transform(new Point(0, 0)).Y + ((ContentControl)sender).ActualHeight / 2;

            int oldIndex = Vm.DraggableCardColumn.Cards.IndexOf(card);
            if (Vm.DraggableCardColumn.Cards.IndexOf(Vm.DragCard) > oldIndex) {
                if (CardPositionY >= MousePosition.Y) {
                    int dragCardIndex = Vm.DraggableCardColumn.Cards.IndexOf(Vm.DragCard);

                    if (oldIndex != -1) {
                        Vm.DraggableCardColumn.Cards.Move(oldIndex, dragCardIndex);
                    } else {
                        Vm.DraggableCardColumn.Cards.Remove(Vm.DragCard);
                        Vm.DraggableCardColumn = Vm.Columns.First(column => column.Cards.Contains(card));
                        int index = Vm.DraggableCardColumn.Cards.IndexOf(card);
                        Vm.DraggableCardColumn.Cards.Insert(index, Vm.DragCard);
                    }
                }
            } else {
                if (CardPositionY < MousePosition.Y) {
                    int dragCardIndex = Vm.DraggableCardColumn.Cards.IndexOf(Vm.DragCard);

                    if (oldIndex != -1) {
                        Vm.DraggableCardColumn.Cards.Move(oldIndex, dragCardIndex);
                    } else {
                        Vm.DraggableCardColumn.Cards.Remove(Vm.DragCard);
                        Vm.DraggableCardColumn = Vm.Columns.First(column => column.Cards.Contains(card));
                        int index = Vm.DraggableCardColumn.Cards.IndexOf(card);
                        Vm.DraggableCardColumn.Cards.Insert(index, Vm.DragCard);
                    }
                }
            }
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

        private void StopDragging()
        {
            if (!IsDraggingColumn && !IsDraggingCard) {
                return;
            }

            if (IsDraggingColumn && DragColumnInitialPosition != Vm.Columns.IndexOf(Vm.DragColumn)) {
                Vm.UpdateColumnOrder.Execute();
            } else if (DragCardInitialColumn != Vm.DraggableCardColumn) {
                Vm.MoveCardToColumn.Execute();
            } else if (IsDraggingCard && DragCardInitialPosition != Vm.DraggableCardColumn.Cards.IndexOf(Vm.DragCard)) {
                Vm.UpdateCardOrder.Execute();
            }

            Vm.DraggableCardColumn = null;
            Vm.DragColumn = null;
            Vm.DragCard = null;
        }

        #endregion

        #region Horizontal scroll

        protected double ScrollInitialPosition { get; set; }

        protected ScrollViewer ColumnsScrollViewer { get; set; }

        protected bool IsScrolling { get; set; }

        private void StartScrollColumns(object sender, MouseButtonEventArgs e)
        {
            // We check that the mouse is pointed at the main parent element of the ListBox, and not at the inner elements.
            if (Mouse.DirectlyOver is not Grid grid || grid.Parent != null) {
                return;
            }

            IsScrolling = true;
            ScrollInitialPosition = Mouse.GetPosition(this).X + ColumnsScrollViewer.HorizontalOffset;

            ListBoxColumns.MouseMove += ScrollColumns;
        }

        private void ScrollColumns(object sender, MouseEventArgs e)
        {
            ColumnsScrollViewer.ScrollToHorizontalOffset(ScrollInitialPosition - Mouse.GetPosition(this).X);
        }

        private void StopScrolling()
        {
            if (!IsScrolling) {
                return;
            }

            IsScrolling = false;
            ListBoxColumns.MouseMove -= ScrollColumns;
        }

        #endregion

        #region Common handlers

        private void Page_MouseUp(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
            StopScrolling();
        }

        private void Page_MouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            StopScrolling();
        }

        #endregion
    }
}
