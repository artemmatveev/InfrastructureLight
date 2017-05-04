using System;
using System.Windows.Data;
using System.Globalization;
using InfrastructureLight.Common.Extensions;

namespace InfrastructureLight.Wpf.Common.Converters
{
    public class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;
            if (value is Enum)
            {
                result = (value as Enum).GetDescription();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value;
            if (result != null)
            {
                result = EnumExtensions.GetEnumFromDescription(value.ToString(), targetType);
            }

            return result;
        }
    }
}
