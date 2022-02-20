﻿using System;

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
    }
}
