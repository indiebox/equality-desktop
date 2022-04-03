using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Equality.Models;
using Equality.MVVM;

namespace Equality.Controls
{
    public sealed class ColumnControl : Control
    {
        static ColumnControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnControl), new FrameworkPropertyMetadata(typeof(ColumnControl)));
        }

        public Column Column
        {
            get => (Column)GetValue(ColumnProperty);
            set => SetValue(ColumnProperty, value);
        }
        public static readonly DependencyProperty ColumnProperty =
          DependencyProperty.Register(nameof(Column), typeof(Column), typeof(ColumnControl), new PropertyMetadata(null));

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

        public ICommand CreateCardCommand
        {
            get => (ICommand)GetValue(CreateCardCommandProperty);
            set => SetValue(CreateCardCommandProperty, value);
        }
        public static readonly DependencyProperty CreateCardCommandProperty =
          DependencyProperty.Register(nameof(CreateCardCommand), typeof(ICommand), typeof(ColumnControl), new PropertyMetadata(null));

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
    }
}
