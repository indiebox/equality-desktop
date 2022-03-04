using System;
using System.Windows;
using System.Windows.Data;

using Catel.MVVM.Converters;

namespace Equality.Converters
{
    [ValueConversion(typeof(int), typeof(object))]
    public class ZeroToEmptyStringConverter : ValueConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value == null) {
                return DependencyProperty.UnsetValue;
            }

            if (value is not int valueInt) {
                return DependencyProperty.UnsetValue;
            }

            return (valueInt == 0) ? string.Empty : value;
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            string valueStr = value as string;

            if (valueStr == string.Empty) {
                return 0;
            }

            int result = 0;
            int? num = null;
            if (int.TryParse(valueStr, out result)) {
                num = result;
            }

            return num;
        }
    }
}
