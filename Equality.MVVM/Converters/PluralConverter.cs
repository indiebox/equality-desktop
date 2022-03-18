using System;
using System.Collections.Generic;
using System.Text;

using Catel.MVVM.Converters;

namespace Equality.MVVM.Converters
{
    public class PluralConverter : ValueConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter)
        {
            string[] options = { "голос", "голоса", "голосов" };
            int numberPositive = Math.Abs(int.Parse((string)value));
            int[] cases = { 2, 0, 1, 1, 1, 2 };
            int key = numberPositive % 100 > 4 && numberPositive % 100 < 20 ? 2 : cases[Math.Min(numberPositive % 10, 5)];
            return (value + " " + options[key]);
        }
    }
}
