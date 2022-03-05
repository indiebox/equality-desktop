using System.Windows;
using System.Windows.Controls;

namespace Equality.Controls
{
    public class TeamLogo : Control
    {
        static TeamLogo()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TeamLogo), new FrameworkPropertyMetadata(typeof(TeamLogo)));
        }

        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }
        public static readonly DependencyProperty ImagePathProperty =
          DependencyProperty.Register(nameof(ImagePath), typeof(string), typeof(TeamLogo), new PropertyMetadata(string.Empty));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly DependencyProperty CornerRadiusProperty =
          DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(TeamLogo), new PropertyMetadata(null));
    }
}
