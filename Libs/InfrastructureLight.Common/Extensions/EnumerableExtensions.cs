using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InfrastructureLight.Common.Extensions
{
    /// <summary>
    ///     Extensions class for the <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Converte IEnumerable to ObservableCollection
        /// </summary>
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> enumerable)
            => new ObservableCollection<T>(enumerable);

        /// <summary>
        ///     Determines whether an element is in the List<T>   
        /// </summary>
        public static bool In<T>(this T source, params T[] list)
        {
            if (null == source) { throw new ArgumentNullException(nameof(source)); }
            return list.Contains(source);
        }

        /// <summary>
        ///     Indicates whether the specified <see cref="IEnumerable{T}"/> 
        ///     is null or an Empty colection
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
            => enumerable == null || !enumerable.Any();

        public static bool IsIn<T>(this T item, IEnumerable<T> collection)
            => collection.Any(m => m.Equals(item));
    }
}
