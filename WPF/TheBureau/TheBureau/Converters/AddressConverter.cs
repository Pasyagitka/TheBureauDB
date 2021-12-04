using System;
using System.Globalization;
using System.Windows.Data;

namespace TheBureau.Converters
{
    public class AddressConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string formattedstring = "";
            if (values[4] == null && values[5] !=null)
            {
                formattedstring = values[0] + ", г." + values[1] + ", ул." + values[2] + ", д." + values[3]
                                  + ", кв." + values[5];
            }
            else if (values[5] == null)
            {
                formattedstring = values[0] + ", г." + values[1] + ", ул." + values[2]+ ", д."  + values[3] +
                                  ", к." + values[4];
            }
            else
            {
                formattedstring = values[0] + ", г." + values[1] + ", ул." + values[2]+ ", д."  + values[3] +
                                  ", к." + values[4] + ", кв." + values[5];
            }

            return formattedstring;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}