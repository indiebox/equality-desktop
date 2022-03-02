using System;
using System.Windows;
using System.Windows.Data;

using Catel.MVVM.Converters;

namespace Equality.Converters
{
    /// <summary>
    /// Converts boolean to enum and back using ConverterParameter.
    /// </summary>
    /// <example>
    /// Very good for state detections, for elements like TabControl, RadioButton, etc.
    /// For example:
    /// <code>
    ///     <TabControl>
    ///         <!--
    ///         ActiveTab is Enum (EnumName) with values EnumValue1 and EnumValue2. When TabItem is selected, ActiveTab will be set to value that represented in ConverterParameter.
    ///         {x:Static viewmodels:ViewModel+Tab.EnumValue1} -> this defenition allows you to search for this enum in your viewmodel, 
    ///         because most often you want to make the enum local to a specific VM, and not put it in the global scope. In short, it is a search for static members in the specified class
    ///         -->
    ///         <TabItem IsSelected="{Binding ActiveTab, Converter={converters:BooleanToEnumConverter}, ConverterParameter={x:Static viewmodels:ViewModel+EnumName.EnumValue1}}" />
    ///         <TabItem IsSelected="{Binding ActiveTab, Converter={converters:BooleanToEnumConverter}, ConverterParameter={x:Static viewmodels:ViewModel+EnumName.EnumValue2}}" />
    ///     </TabControl>
    ///     
    ///     <!-- Same for RadioButton -->
    ///     <RadioButton IsChecked="..." />
    /// </code>
    /// </example>
    [ValueConversion(typeof(object), typeof(object))]
    public class BooleanToEnumConverter : ValueConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value == null) {
                return false;
            }

            if (parameter is not Enum parameterEnum) {
                return DependencyProperty.UnsetValue;
            }

            if (Enum.IsDefined(value.GetType(), parameterEnum) == false) {
                return DependencyProperty.UnsetValue;
            }

            return value.Equals(parameter);
        }

        protected override object ConvertBack(object value, Type targetType, object parameter)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }

    [ValueConversion(typeof(object), typeof(object))]
    public class BooleanToEnumFlagsConverter : BooleanToEnumConverter
    {
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (value == null) {
                return false;
            }

            if (parameter is not Enum parameterEnum) {
                return DependencyProperty.UnsetValue;
            }

            if (Enum.IsDefined(value.GetType(), parameterEnum) == false) {
                return DependencyProperty.UnsetValue;
            }

            return ((Enum)value).HasFlag(parameterEnum);
        }
    }
}
