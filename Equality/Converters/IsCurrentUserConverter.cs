using System;
using System.Windows.Data;

using Catel.MVVM.Converters;

using Equality.Extensions;
using Equality.Models;

namespace Equality.Converters
{
    /// <summary>
    /// Returns <see langword="true"/> if <c>User</c> model is currently authenticated user.
    /// </summary>
    /// <example>
    /// For example:
    /// <code>
    ///     
    /// </code>
    /// </example>
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsCurrentUserConverter : ValueConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value == null) {
                return false;
            }

            if (value is not User user) {
                return false;
            }

            return user.IsCurrentUser();
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotSupportedException();
        }
    }
}
