using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Equality.Models;

namespace Equality.Controls
{
    public class CardControl : Control
    {
        static CardControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardControl), new FrameworkPropertyMetadata(typeof(CardControl)));
        }

        #region Properties

        public Card Card
        {
            get => (Card)GetValue(CardProperty);
            set => SetValue(CardProperty, value);
        }
        public static readonly DependencyProperty CardProperty =
          DependencyProperty.Register(nameof(Card), typeof(Card), typeof(CardControl), new PropertyMetadata(null));

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }
        public static readonly DependencyProperty IsEditableProperty =
          DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(CardControl), new PropertyMetadata(false));

        public string NewCardName
        {
            get => (string)GetValue(NewCardNameProperty);
            set => SetValue(NewCardNameProperty, value);
        }
        public static readonly DependencyProperty NewCardNameProperty =
          DependencyProperty.Register(nameof(NewCardName), typeof(string), typeof(CardControl), new PropertyMetadata(string.Empty));

        #endregion

        #region Commands

        public ICommand EditCardCommand
        {
            get => (ICommand)GetValue(EditCardCommandProperty);
            set => SetValue(EditCardCommandProperty, value);
        }
        public static readonly DependencyProperty EditCardCommandProperty =
          DependencyProperty.Register(nameof(EditCardCommand), typeof(ICommand), typeof(CardControl), new PropertyMetadata(null));

        public ICommand SaveNewCardNameCommand
        {
            get => (ICommand)GetValue(SaveNewCardNameCommandProperty);
            set => SetValue(SaveNewCardNameCommandProperty, value);
        }
        public static readonly DependencyProperty SaveNewCardNameCommandProperty =
          DependencyProperty.Register(nameof(SaveNewCardNameCommand), typeof(ICommand), typeof(CardControl), new PropertyMetadata(null));

        public ICommand CancelEditCardCommand
        {
            get => (ICommand)GetValue(CancelEditCardCommandProperty);
            set => SetValue(CancelEditCardCommandProperty, value);
        }
        public static readonly DependencyProperty CancelEditCardCommandProperty =
          DependencyProperty.Register(nameof(CancelEditCardCommand), typeof(ICommand), typeof(CardControl), new PropertyMetadata(null));

        public ICommand DeleteCardCommand
        {
            get => (ICommand)GetValue(DeleteCardCommandProperty);
            set => SetValue(DeleteCardCommandProperty, value);
        }
        public static readonly DependencyProperty DeleteCardCommandProperty =
          DependencyProperty.Register(nameof(DeleteCardCommand), typeof(ICommand), typeof(CardControl), new PropertyMetadata(null));


        #endregion
    }
}
