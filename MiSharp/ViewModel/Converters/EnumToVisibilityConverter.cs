using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MiSharp
{
    public class EnumToVisibilityConverter : IValueConverter
    {
        public EnumToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Hidden;
        }

        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                if (value == null)
                {
                    return FalseValue;
                }
                else
                {
                    var equals = Equals(value, parameter);
                    return equals ? TrueValue : FalseValue;
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}