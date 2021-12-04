using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TheBureau.Converters
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.StartPoint = new Point(0,0.5);
                brush.EndPoint = new Point(1,0.5);
                switch (value)
                {
                    case 1:
                    {
                        brush.GradientStops.Add(new GradientStop(Color.FromRgb(255, 197, 110), 0.0));
                        brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));
                        return brush;
                    }
                    case 2:
                    {
                        brush.GradientStops.Add(new GradientStop(Color.FromRgb(255, 239, 97), 0.0));
                        brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));
                        return brush;
                    }
                    default:
                    {
                        brush.GradientStops.Add(new GradientStop(Color.FromRgb(159, 230, 133), 0.0));
                        brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));
                        return brush;
                    }
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