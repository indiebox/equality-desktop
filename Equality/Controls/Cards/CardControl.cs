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

        public Card Card
        {
            get => (Card)GetValue(CardProperty);
            set => SetValue(CardProperty, value);
        }
        public static readonly DependencyProperty CardProperty =
          DependencyProperty.Register(nameof(Card), typeof(Card), typeof(CardControl), new PropertyMetadata(null));

        #region Commands

        public ICommand DeleteCardCommand
        {
            get => (ICommand)GetValue(DeleteCardCommandProperty);
            set => SetValue(DeleteCardCommandProperty, value);
        }
        public static readonly DependencyProperty DeleteCardCommandProperty =
          DependencyProperty.Register(nameof(DeleteCardCommand), typeof(ICommand), typeof(CardControl), new PropertyMetadata(null));

        #endregion Commands
    }
}
