using System;
using System.Globalization;
using System.Windows.Data;

namespace MiSharp
{
    public class FirstLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (!string.IsNullOrEmpty(s))
                return s.Substring(0, 1).ToUpper();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}