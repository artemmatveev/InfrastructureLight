using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLight.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> Sort<T>(this IEnumerable<T> collection, Func<T, DateTime> expression, bool byDescending = false)
        {
            if (collection.IsNull())
            {
                return collection.ToObservable();
            }
            else
            {
                return byDescending ? collection.OrderByDescending(expression).ToObservable()
                                    : collection.OrderBy(expression).ToObservable();
            }
        }
    }
}
