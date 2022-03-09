using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

using Catel.MVVM.Converters;

using Notification.Wpf.Converters;

using Notifications.Wpf.Command;

namespace Equality.Notifications.Converters
{
    // see: https://github.com/Platonenkov/Notification.Wpf/blob/051e0564d172c6fb61de08f5dc439d5ab710773c/Notification.Wpf/Converters/ActionToCommandConverter.cs

    [ValueConversion(typeof(object), typeof(LamdaCommand)), MarkupExtensionReturnType(typeof(BooleanToOppositeBooleanConverter))]
    internal class ActionToCommandConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is not Action act ? null : new LamdaCommand(o => act.Invoke());
    }
}
