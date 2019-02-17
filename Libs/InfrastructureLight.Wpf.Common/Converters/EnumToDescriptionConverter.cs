using InfrastructureLight.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

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
