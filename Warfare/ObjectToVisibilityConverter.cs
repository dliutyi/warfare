using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Warfare
{
    class ObjectToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = parameter as string;
            if (param == "Invert")
            {
                return value == null ? Visibility.Visible : Visibility.Collapsed;
            }
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
