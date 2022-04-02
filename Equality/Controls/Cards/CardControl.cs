using System.Windows;
using System.Windows.Controls;

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
    }
}
