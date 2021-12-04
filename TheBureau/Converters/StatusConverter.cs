using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TheBureau.Converters
{
    [ValueConversion(typeof (int), typeof (string))]
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                switch (value)
                {
                    case 1 : return "В обработке";
                    case 2: return "В процессе";
                    default: return "Готово";
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}