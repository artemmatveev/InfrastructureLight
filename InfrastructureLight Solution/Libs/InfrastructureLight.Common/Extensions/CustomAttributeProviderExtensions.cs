using System;
using System.Reflection;

namespace InfrastructureLight.Common.Extensions
{
    /// <summary>
    ///     Extensions class for the <see cref="ICustomAttributeProvider"/>
    /// </summary>
    public static class CustomAttributeProviderExtensions
    {
        /// <summary>
        ///     Strongly typed version GetCustomAttributes method
        /// </summary>        
        public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider attributeProvider, bool inherit)
            where T : Attribute
        {
            return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit);
        }
    }
}
