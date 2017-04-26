using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace InfrastructureLight.Common.Extensions
{
    /// <summary>
    ///     Extensions class for the <see cref="System.Enum"/>
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Get an enum Description value
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            if (value.GetType().IsEnum == false)
                throw new ArgumentOutOfRangeException("value", "value is not enum");

            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
                return null;

            DescriptionAttribute[] attributes
                = fieldInfo.GetCustomAttributes<DescriptionAttribute>(false);

            return attributes.Any() ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        ///     Convert an <see cref="System.Enum"/> to a list
        /// </summary>
        public static IEnumerable<KeyValuePair<int, string>> ToKeyValuePairs<TEnum>() where TEnum : struct, IConvertible
        {
            if (typeof(TEnum).IsEnum == false)
                throw new ArgumentException("TEnum must be an enumerated type");

            List<KeyValuePair<int, string>> items = Enum.GetValues(typeof(TEnum))
                .Cast<Enum>()
                .Select(x => new KeyValuePair<int, string>(int.Parse(x.ToString("D")), x.GetDescription()))
                .ToList();

            return items;
        }

        /// <summary>
        ///     Get an <see cref="System.Enum"/> from Description value
        /// </summary>
        public static object GetEnumFromDescription(string descToDecipher, Type destinationType)
        {
            var type = destinationType;
            if (!type.IsEnum) { throw new InvalidOperationException(); }

            var staticFields = type.GetFields().Where(fld => fld.IsStatic);
            foreach (var field in staticFields)
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null)
                {
                    if (attribute.Description == descToDecipher)
                    {
                        return (Enum.Parse(type, field.Name, true));
                    }
                }
                else
                {
                    if (field.Name == descToDecipher)
                        return field.GetValue(null);
                }
            }

            throw new ArgumentException("Description is not found in enum list.");
        }
    }
}
