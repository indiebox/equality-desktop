using System.Windows;
using System.Windows.Controls;

namespace Equality.Controls
{
    public enum CardType
    {
        Error,
        Success,
    }

    public sealed class InfoCard : Control
    {
        static InfoCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InfoCard), new FrameworkPropertyMetadata(typeof(InfoCard)));
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
          DependencyProperty.Register(nameof(Text), typeof(string), typeof(InfoCard), new PropertyMetadata(string.Empty));

        public CardType Type
        {
            get => (CardType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
        public static readonly DependencyProperty TypeProperty =
          DependencyProperty.Register(nameof(Type), typeof(CardType), typeof(InfoCard), new PropertyMetadata(CardType.Error));
    }
}
