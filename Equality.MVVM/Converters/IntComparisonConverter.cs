using System;
using System.Windows.Data;

using Catel.MVVM.Converters;

namespace Equality.Converters
{
    /// <summary>
    /// Converts int to boolean if it greater or equal to converter parameter.
    /// </summary>
    /// <example>
    /// For example:
    /// <code>
    ///     <Button IsEnabled={Binding Votes.Count, Converter={equality:GreaterOrEqualConverter}, ConverterParameter=30>Add</Button>
    /// </code>
    /// </example>
    [ValueConversion(typeof(int), typeof(bool))]
    public class GreaterOrEqualConverter : ValueConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter)
        {
            return (int)value >= int.Parse(parameter.ToString());
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Converts int to boolean if it less or equal to converter parameter.
    /// </summary>
    /// <example>
    /// For example:
    /// <code>
    ///     <Button IsEnabled={Binding Votes.Count, Converter={equality:LessOrEqualConverter}, ConverterParameter=30>Add</Button>
    /// </code>
    /// </example>
    [ValueConversion(typeof(int), typeof(bool))]
    public class LessOrEqualConverter : ValueConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter)
        {
            return (int)value <= int.Parse(parameter.ToString());
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            throw new NotSupportedException();
        }
    }
}
