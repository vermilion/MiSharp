using System;
using System.Globalization;
using System.Windows.Data;
using NAudio.CoreAudioApi;

namespace MiSharp.ViewModel.Converters
{
    public class ShareModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mode = (AudioClientShareMode) Enum.Parse(typeof (AudioClientShareMode), value.ToString());
            if (mode == AudioClientShareMode.Exclusive) return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var exclusive = System.Convert.ToBoolean(value);
            if (exclusive) return AudioClientShareMode.Exclusive;
            return AudioClientShareMode.Shared;
        }
    }
}