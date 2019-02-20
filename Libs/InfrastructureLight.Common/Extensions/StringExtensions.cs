using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace InfrastructureLight.Common.Extensions
{
    /// <summary>
    ///     Extensions class for the <see cref="System.String"/>
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Converts the value of objects to strings based on the formats specified 
        ///     and inserts them into another string
        /// </summary>
        public static string f(this string source, params object[] args)
            => string.Format(source, args);

        /// <summary>
        ///     Concatenates the specified elements of a string array, 
        ///     using the specified separator between each element
        /// </summary>
        public static string Join(this IEnumerable<string> source, string separator)
            => string.Join(separator, source);

        /// <summary>
        ///     Concatenates the specified elements of a string array, 
        ///     using the specified separator between each element
        /// </summary>
        public static string Join(this StringCollection source, string separator)
            => string.Join(separator, source.Cast<string>());

        /// <summary>
        ///     Returns a value indicating whether a specified 
        ///     substring occurs within this string
        /// </summary>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            if (!string.IsNullOrEmpty(source))
                return source.IndexOf(value, comparisonType) != -1;

            return false;
        }

        /// <summary>
        ///     Indicates whether a specified string is null, empty, or 
        ///     consists only of white-space characters
        /// </summary>        
        public static bool IsEmpty(this string source)
            => string.IsNullOrWhiteSpace(source);

        /// <summary>
        ///     Converts the string representation of a number to its 32-bit signed integer equivalent. 
        ///     A return value indicates whether the operation succeeded.
        /// </summary>
        public static bool IsEmptyOrNotNaturalNumber(this string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
                return false;

            int value;
            if (!int.TryParse(source, NumberStyles.Integer, CultureInfo.CurrentCulture, out value) || value < 0)
                return false;

            return true;
        }
    }
}
