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

        public Point DeltaMouse { get; set; }

        public Point DraggableRelativePoint { get; set; }

        protected int DragElementInitialPosition { get; set; }

        bool IsDraggingColumn => Vm?.DragColumn != null;

        bool IsDraggingCard => Vm?.DragCard != null;

        Column DragCardInitialColumn { get; set; }

        /// <summary>
        /// Fired when mouse is over through column header(with column name).
        /// </summary>
        private void ColumnHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDraggingColumn) {
                return;
            }

            var control = (FrameworkElement)sender;
            Vm.DragColumn = control.DataContext as Column;

            DragElementInitialPosition = Vm.Columns.IndexOf(Vm.DragColumn);
            StartDragging(control);
        }

        /// <summary>
        /// Fired when mouse is over through column element(full height of the page).
        /// </summary>
        private async void ColumnMouseEnter(object sender, MouseEventArgs e)
        {
            var column = ((FrameworkElement)sender).DataContext as Column;

            // Move draggable card in new column.
            if (IsDraggingCard) {
                Vm.DraggableCardColumn.Cards.Remove(Vm.DragCard);
                Vm.DraggableCardColumn = column;
                column.Cards.Add(Vm.DragCard);

                return;
            }

            if (!IsDraggingColumn) {
                return;
            }

            int oldIndex = Vm.Columns.IndexOf(column);
            int dragColumnIndex = Vm.Columns.IndexOf(Vm.DragColumn);

            Vm.Columns.Move(oldIndex, dragColumnIndex);
        }

        /// <summary>
        /// Fired when mouse down in card element.
        /// </summary>
        private void CardMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDraggingCard) {
                return;
            }

            var control = (ContentControl)sender;
            Vm.DragCard = control.Content as Card;
            Vm.DraggableCardColumn = Vm.Columns.First(column => column.Cards.Contains(Vm.DragCard));

            DragCardInitialColumn = Vm.DraggableCardColumn;
            DragElementInitialPosition = Vm.DraggableCardColumn.Cards.IndexOf(Vm.DragCard);
            StartDragging(control);
        }

        /// <summary>
        /// Fired when mouse move in card element.
        /// </summary>
        private async void CardMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDraggingCard) {
                return;
            }

            var control = (ContentControl)sender;
            var card = control.Content as Card;
            if (Vm.DragCard == card) {
                return;
            }

            double mousePosition = e.GetPosition(control).Y;
            double cardCenter = control.ActualHeight / 2;

            int dragCardIndex = Vm.DraggableCardColumn.Cards.IndexOf(Vm.DragCard);
            int cardIndex = Vm.DraggableCardColumn.Cards.IndexOf(card);

            if ((dragCardIndex > cardIndex && mousePosition < cardCenter)
                || (dragCardIndex < cardIndex && mousePosition > cardCenter)) {
                Vm.DraggableCardColumn.Cards.Move(dragCardIndex, cardIndex);
            }
        }

        private void StartDragging(FrameworkElement control)
        {
            var element = IsDraggingCard
                    ? MovingCard
                    : MovingColumn;

            DeltaMouse = Mouse.GetPosition(DraggingCanvas);
            DraggableRelativePoint = control.TransformToAncestor(this).Transform(new Point(0, 0));

            Canvas.SetLeft(element, DraggableRelativePoint.X);
            Canvas.SetTop(element, DraggableRelativePoint.Y);

            PageGrid.MouseMove += GridMouseMove;
        }

        private void StopDragging()
        {
            if (IsDraggingColumn) {
                if (DragElementInitialPosition != Vm.Columns.IndexOf(Vm.DragColumn)) {
                    Vm.UpdateColumnOrder.Execute();
                }

                Vm.DragColumn = null;
            }

            if (IsDraggingCard) {
                if (DragCardInitialColumn != Vm.DraggableCardColumn) {
                    Vm.MoveCardToColumn.Execute();
                } else if (DragElementInitialPosition != Vm.DraggableCardColumn.Cards.IndexOf(Vm.DragCard)) {
                    Vm.UpdateCardOrder.Execute();
                }

                Vm.DragCard = null;
                Vm.DraggableCardColumn = null;
            }
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

        private void GridMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDraggingCard && !IsDraggingColumn) {
                PageGrid.MouseMove -= GridMouseMove;

                return;
            }

            var cursorPosition = Mouse.GetPosition(DraggingCanvas);
            var element = IsDraggingCard
                    ? MovingCard
                    : MovingColumn;

            Canvas.SetLeft(element, DraggableRelativePoint.X + (cursorPosition.X - DeltaMouse.X));
            Canvas.SetTop(element, DraggableRelativePoint.Y + (cursorPosition.Y - DeltaMouse.Y));
        }

        private void PageMouseUp(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
            StopScrolling();
        }

        private void PageMouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            StopScrolling();
        }

        #endregion
    }
}
