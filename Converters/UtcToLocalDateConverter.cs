using System;
using System.Globalization;

using Catel;
using Catel.MVVM.Converters;

namespace Equality.Converters
{
    [System.Windows.Data.ValueConversion(typeof(DateTime), typeof(string))]
    public class UtcToLocalDateConverter : ShortDateFormattingConverter
    {
        public const string DefaultFormat = "dd.MM.yyyy HH:mm";

        protected override object Convert(object value, Type targetType, object parameter)
        {
            Argument.IsOfType(nameof(value), value, typeof(DateTime));

            return ((DateTime)value).ToLocalTime().ToString(parameter as string ?? DefaultFormat);
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <returns>The value to be passed to the source object.</returns>
        /// <remarks>
        /// By default, this method returns <see cref="ConverterHelper.UnsetValue"/>. This method only has
        /// to be overridden when it is actually used.
        /// </remarks>
        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            bool parsed = DateTime.TryParse(value as string, CurrentCulture, DateTimeStyles.None, out var dateTimeValue);

            return parsed ? dateTimeValue : ConverterHelper.UnsetValue;
        }
    }
}
