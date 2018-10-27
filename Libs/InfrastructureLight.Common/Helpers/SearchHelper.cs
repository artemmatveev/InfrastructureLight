using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;

namespace InfrastructureLight.Common.Helpers
{
    public static class SearchHelper
    {
        /// <summary>
        ///     Filters the <paramref name="source" /> collection and returns the elements, where property 
        ///     value contains <paramref name="searchText" />
        /// </summary>         
        public static IEnumerable<T> Search<T>(IEnumerable<T> source, string searchText, List<string> searchFields = null) where T : class
        {
            if (string.IsNullOrEmpty(searchText)) { return source; }

            Expression<Func<T, bool>> query = null, exp = null;
            if (source.Any())
            {
                var fields = searchFields == null
                        ? source.FirstOrDefault().GetType().GetProperties()
                        : source.FirstOrDefault().GetType().GetProperties().Where(p => searchFields.Contains(p.Name));

                foreach (var field in fields)
                {
                    if (string.Empty.GetType() == field.PropertyType)
                    {
                        exp = p => !string.IsNullOrEmpty((string)field.GetValue(p)) && ((string)field.GetValue(p))
                                          .ToLower().Contains(searchText.ToLower());

                        query = query != null ? query.Or(exp) : exp;
                    }
                }
            }

            query = query.Expand();
            if (query == null) { return source; }

            return source.Where(query.Compile()).ToList();
        }

        /// <summary>
        ///     Filters the <paramref name="source" /> tree, and returns the elements, where
        ///     <paramref name="condition" /> is true.
        /// </summary>
        public static IEnumerable<T> SearchTree<T, TKey>(this IEnumerable<T> source, Predicate<T> condition,
            Func<T, TKey> keySelector, Func<T, TKey> parentKeySelector) where T : class
        {
            var result = new List<T>();

            List<T> elements = source.Where(e => condition(e)).ToList();
            foreach (T element in elements)
            {
                T current = element;
                while (current != null)
                {
                    if (result.Contains(current))
                    {
                        break;
                    }

                    result.Add(current);

                    current = source.FirstOrDefault(arg => keySelector(arg)
                        .Equals(parentKeySelector(current)));
                }
            }

            return result;
        }
    }
}
