using System.Windows;
using System.Windows.Controls;

namespace Equality.Controls
{
    public class Invitation : Control
    {
        static Invitation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Invitation), new FrameworkPropertyMetadata(typeof(Invitation)));
        }

        public string Team
        {
            get => (string)GetValue(TeamProperty);
            set => SetValue(TeamProperty, value);
        }
        public static readonly DependencyProperty TeamProperty =
          DependencyProperty.Register(nameof(Team), typeof(string), typeof(Invitation), new PropertyMetadata(string.Empty));
        public string NameInvitor
        {
            get => (string)GetValue(NameInvitorProperty);
            set => SetValue(NameInvitorProperty, value);
        }
        public static readonly DependencyProperty NameInvitorProperty =
          DependencyProperty.Register(nameof(NameInvitor), typeof(string), typeof(Invitation), new PropertyMetadata(string.Empty));

        public string TeamLogoImagePath
        {
            get => (string)GetValue(TeamLogoImagePathProperty);
            set => SetValue(TeamLogoImagePathProperty, value);
        }
        public static readonly DependencyProperty TeamLogoImagePathProperty =
          DependencyProperty.Register(nameof(TeamLogoImagePath), typeof(string), typeof(Invitation), new PropertyMetadata(string.Empty));
    }
}
