using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TheBureau.Converters
{
    [ValueConversion(typeof (int), typeof (string))]
    public class StageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                switch (value)
                {
                    case 1 : return "Чистовая отделка";
                    case 2: return "Черновая отделка";
                    default: return "Чистовая и черновая отделки";
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