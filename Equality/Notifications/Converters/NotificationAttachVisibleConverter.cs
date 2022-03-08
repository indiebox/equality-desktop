using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

using Notification.Wpf;
using Notification.Wpf.Converters;

namespace Equality.Notifications.Converters
{
    // see: https://github.com/Platonenkov/Notification.Wpf/blob/051e0564d172c6fb61de08f5dc439d5ab710773c/Notification.Wpf/Converters/NotificationAttachVisibleConverter.cs

    [ValueConversion(typeof(NotificationContent), typeof(Visibility)), MarkupExtensionReturnType(typeof(NotificationAttachVisibleConverter))]
    internal class NotificationAttachVisibleConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (!(v is NotificationContent content))
                return Visibility.Collapsed;
            return content.Message.Length < 43 * content.RowsCount ? Visibility.Collapsed : Visibility.Visible;

        }
    }
}
