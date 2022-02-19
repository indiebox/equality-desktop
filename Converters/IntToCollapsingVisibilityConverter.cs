using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;

using Catel.MVVM.Converters;

namespace Equality.Converters
{
    /// <summary>
    /// Converts int to hidding visibility.
    /// Less or equal 0 -> hidden/collapsed
    /// Greater than 0  -> visible
    /// </summary>
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class IntToCollapsingVisibilityConverter : VisibilityConverterBase
    {
        public IntToCollapsingVisibilityConverter() : base(Visibility.Collapsed)
        {
        }

        public IntToCollapsingVisibilityConverter(Visibility visibility) : base(visibility)
        {
        }

        protected override bool IsVisible(object value, Type targetType, object parameter)
        {
            if (value is int valueInt) {
                return valueInt > 0;
            }

            // Note: base class will invert if needed

            return false;
        }
    }

    public class IntToHidingVisibilityConverter : IntToCollapsingVisibilityConverter
    {
        public IntToHidingVisibilityConverter() : base(Visibility.Hidden)
        {
        }
    }
}
