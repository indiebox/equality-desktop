using System.Windows;

namespace Equality.Assists
{
    public static class PasswordBoxAssist
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxAssist), new PropertyMetadata(string.Empty));

        public static void SetPassword(DependencyObject dp, string value)
        {
        }
    }
}
