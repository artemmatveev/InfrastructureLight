using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace InfrastructureLight.Wpf.Common.Converters
{
    public class InverseNullToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
