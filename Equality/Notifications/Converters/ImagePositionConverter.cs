using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

using Notification.Wpf;
using Notification.Wpf.Converters;

namespace Equality.Notifications.Converters
{
    // This is a custom converter that converts image position dirrectly to the visibility(without bool converter).

    [ValueConversion(typeof(ImagePosition), typeof(Visibility)), MarkupExtensionReturnType(typeof(ImagePositionToVisibilityConverter))]
    internal class ImagePositionToVisibilityConverter : ValueConverter
    {
        public ImagePosition Position { get; set; }

        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not ImagePosition pos) {
                return DependencyProperty.UnsetValue;
            }

            if (pos == ImagePosition.None) {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }

    // see: https://github.com/Platonenkov/Notification.Wpf/blob/051e0564d172c6fb61de08f5dc439d5ab710773c/Notification.Wpf/Converters/ImagePositionConverter.cs

    [ValueConversion(typeof(ImagePosition), typeof(int)), MarkupExtensionReturnType(typeof(ImagePositionGridRowConverter))]
    internal class ImagePositionGridRowConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not ImagePosition position)
                return null;
            return position switch
            {
                ImagePosition.Top => 0,
                ImagePosition.Bottom => 2,
                _ => 0
            };
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
    [ValueConversion(typeof(ImagePosition), typeof(Thickness)), MarkupExtensionReturnType(typeof(ImagePositionMarginConverter))]
    internal class ImagePositionMarginConverter : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is not ImagePosition position)
                return null;
            return position switch
            {
                ImagePosition.Top => new Thickness(0, 0, 0, 5),
                ImagePosition.Bottom => new Thickness(0, 5, 0, 0),
                _ => 0
            };
        }

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
}
