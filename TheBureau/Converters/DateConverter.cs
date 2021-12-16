using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TheBureau.Converters
{
    [ValueConversion(typeof (DateTime), typeof (string))]
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                if (value.Equals(DateTime.MinValue)) return "";
               else return value;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}