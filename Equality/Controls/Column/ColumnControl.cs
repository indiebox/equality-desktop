using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Equality.Models;
using Equality.MVVM;

namespace Equality.Controls
{
    public class ColumnControl : Control
    {
        static ColumnControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnControl), new FrameworkPropertyMetadata(typeof(ColumnControl)));
        }

        /// <summary>
        /// Does this column in 'dragging' mode.
        /// </summary>
        public bool IsDragging
        {
            get => (bool)GetValue(IsDraggingProperty);
            set => SetValue(IsDraggingProperty, value);
        }
        public static readonly DependencyProperty IsDraggingProperty =
          DependencyProperty.Register(nameof(IsDragging), typeof(bool), typeof(ColumnControl), new PropertyMetadata(false));

        #region ColumnProperties

        public Column Column
        {
            get => (Column)GetValue(ColumnProperty);
            set => SetValue(ColumnProperty, value);
        }
        public static readonly DependencyProperty ColumnProperty =
          DependencyProperty.Register(nameof(Column), typeof(Column), typeof(ColumnControl), new PropertyMetadata(null));

        public string NewColumnName
        {
            get => (string)GetValue(NewColumnNameProperty);
            set => SetValue(NewColumnNameProperty, value);
        }
        public static readonly DependencyProperty NewColumnNameProperty =
          DependencyProperty.Register(nameof(NewColumnName), typeof(string), typeof(ColumnControl), new PropertyMetadata(string.Empty));

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }
        public static readonly DependencyProperty IsEditableProperty =
          DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(ColumnControl), new PropertyMetadata(false));

        #endregion ColumnProperties

        #region CardProperties

        public Card EditableCard
        {
            get => (Card)GetValue(EditableCardProperty);
            set => SetValue(EditableCardProperty, value);
        }
        public static readonly DependencyProperty EditableCardProperty =
          DependencyProperty.Register(nameof(EditableCard), typeof(Card), typeof(ColumnControl), new PropertyMetadata(null));

        public string NewCardName
        {
            get => (string)GetValue(NewCardNameProperty);
            set => SetValue(NewCardNameProperty, value);
        }
        public static readonly DependencyProperty NewCardNameProperty =
          DependencyProperty.Register(nameof(NewCardName), typeof(string), typeof(ColumnControl), new PropertyMetadata(string.Empty));

        /// <summary>
        /// View model thats represent create card view.
        /// </summary>
        public ViewModel CreateCardVm
        {
            get => (ViewModel)GetValue(CreateCardVmProperty);
            set => SetValue(CreateCardVmProperty, value);
        }
        public static readonly DependencyProperty CreateCardVmProperty =
          DependencyProperty.Register(nameof(CreateCardVm), typeof(ViewModel), typeof(ColumnControl), new PropertyMetadata(OnCreateCardVmChanged));
        private static void OnCreateCardVmChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ColumnControl c = sender as ColumnControl;
            if (c != null && e.NewValue != null) {
                c.ScrollToEnd();
            }
        }
        private void ScrollToEnd()
        {
            var list = GetTemplateChild("CardsList") as ListBox;
            if (list == null) {
                return;
            }

            list.ScrollIntoView(list.Items[^1]);
        }

        #endregion CardProperties

        #region ColumnCommands

        public ICommand EditColumnCommand
        {
            get => (ICommand)GetValue(EditColumnCommandProperty);
            set => SetValue(EditColumnCommandProperty, value);
        }
        public static readonly DependencyProperty EditColumnCommandProperty =
          DependencyProperty.Register(nameof(EditColumnCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        public ICommand SaveNewColumnNameCommand
        {
            get => (ICommand)GetValue(SaveNewColumnNameCommandProperty);
            set => SetValue(SaveNewColumnNameCommandProperty, value);
        }
        public static readonly DependencyProperty SaveNewColumnNameCommandProperty =
          DependencyProperty.Register(nameof(SaveNewColumnNameCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        public ICommand CancelEditColumnCommand
        {
            get => (ICommand)GetValue(CancelEditColumnCommandProperty);
            set => SetValue(CancelEditColumnCommandProperty, value);
        }
        public static readonly DependencyProperty CancelEditColumnCommandProperty =
          DependencyProperty.Register(nameof(CancelEditColumnCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        public ICommand DeleteColumnCommand
        {
            get => (ICommand)GetValue(DeleteColumnCommandProperty);
            set => SetValue(DeleteColumnCommandProperty, value);
        }
        public static readonly DependencyProperty DeleteColumnCommandProperty =
          DependencyProperty.Register(nameof(DeleteColumnCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        #endregion ColumnCommands

        #region CardCommands

        public ICommand DeleteColumnCommand
        {
            get => (ICommand)GetValue(DeleteColumnCommandProperty);
            set => SetValue(DeleteColumnCommandProperty, value);
        }
        public static readonly DependencyProperty DeleteColumnCommandProperty =
          DependencyProperty.Register(nameof(DeleteColumnCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        public ICommand CreateCardCommand
        {
            get => (ICommand)GetValue(CreateCardCommandProperty);
            set => SetValue(CreateCardCommandProperty, value);
        }
        public static readonly DependencyProperty CreateCardCommandProperty =
          DependencyProperty.Register(nameof(CreateCardCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        public ICommand EditCardCommand
        {
            get => (ICommand)GetValue(EditCardCommandProperty);
            set => SetValue(EditCardCommandProperty, value);
        }
        public static readonly DependencyProperty EditCardCommandProperty =
          DependencyProperty.Register(nameof(EditCardCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        public ICommand SaveNewCardNameCommand
        {
            get => (ICommand)GetValue(SaveNewCardNameCommandProperty);
            set => SetValue(SaveNewCardNameCommandProperty, value);
        }
        public static readonly DependencyProperty SaveNewCardNameCommandProperty =
          DependencyProperty.Register(nameof(SaveNewCardNameCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        public ICommand CancelEditCardCommand
        {
            get => (ICommand)GetValue(CancelEditCardCommandProperty);
            set => SetValue(CancelEditCardCommandProperty, value);
        }
        public static readonly DependencyProperty CancelEditCardCommandProperty =
          DependencyProperty.Register(nameof(CancelEditCardCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        public ICommand DeleteCardCommand
        {
            get => (ICommand)GetValue(DeleteCardCommandProperty);
            set => SetValue(DeleteCardCommandProperty, value);
        }
        public static readonly DependencyProperty DeleteCardCommandProperty =
          DependencyProperty.Register(nameof(DeleteCardCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

        #endregion CardCommands
    }
}
