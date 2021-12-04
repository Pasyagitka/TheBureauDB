using System;
using System.Globalization;
using System.Windows.Data;

namespace TheBureau.Converters
{
    public class NameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string formattedstring = "";
            formattedstring = values[0] + " " + values[1] +  " " + values[2] + ", #" + values[3];
            return formattedstring;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}