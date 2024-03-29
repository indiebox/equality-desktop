﻿using System;

using Catel.MVVM.Converters;

namespace Equality.Converters
{
    [System.Windows.Data.ValueConversion(typeof(int), typeof(string))]
    public class PluralizeConverter : ValueConverterBase
    {
        public string One { get; set; }
        public string Two { get; set; }
        public string Five { get; set; }

        protected override object Convert(object value, Type targetType, object parameter)
        {
            int numberPositive = Math.Abs((int)value);

            int[] cases = { 2, 0, 1, 1, 1, 2 };
            int key = numberPositive % 100 > 4 && numberPositive % 100 < 20 ? 2 : cases[Math.Min(numberPositive % 10, 5)];

            return value + " " + key switch
            {
                0 => One,
                1 => Two,
                2 => Five,
                _ => One,
            };
        }
    }
}
