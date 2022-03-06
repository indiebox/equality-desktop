using System.Windows;
using System.Windows.Controls;

namespace Equality.Controls
{
    public class $safeitemname$ : Control
    {
        static $safeitemname$()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof($safeitemname$), new FrameworkPropertyMetadata(typeof($safeitemname$)));
        }

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }
        public static readonly DependencyProperty PropertyNameProperty =
          DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof($safeitemname$), new PropertyMetadata(string.Empty));
    }
}
