using System.Windows;
using System.Windows.Controls;

namespace Equality.Controls
{
    public class Invation : Control
    {
        static Invation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Invation), new FrameworkPropertyMetadata(typeof(Invation)));
        }

        public string Team
        {
            get => (string)GetValue(TeamProperty);
            set => SetValue(TeamProperty, value);
        }
        public static readonly DependencyProperty TeamProperty =
          DependencyProperty.Register(nameof(Team), typeof(string), typeof(Invation), new PropertyMetadata(string.Empty));
        public string NameInvitor
        {
            get => (string)GetValue(NameInvitorProperty);
            set => SetValue(NameInvitorProperty, value);
        }
        public static readonly DependencyProperty NameInvitorProperty =
          DependencyProperty.Register(nameof(NameInvitor), typeof(string), typeof(Invation), new PropertyMetadata(string.Empty));

        public string TeamLogoImagePath
        {
            get => (string)GetValue(TeamLogoImagePathProperty);
            set => SetValue(TeamLogoImagePathProperty, value);
        }
        public static readonly DependencyProperty TeamLogoImagePathProperty =
          DependencyProperty.Register(nameof(TeamLogoImagePath), typeof(string), typeof(Invation), new PropertyMetadata(string.Empty));
    }
}
