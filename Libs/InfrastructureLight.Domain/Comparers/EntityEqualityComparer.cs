using System.Collections.Generic;

namespace InfrastructureLight.Domain.Comparers
{
    /// <summary>
    ///     Entity Equality Comparer
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class EntityEqualityComparer<TEntity, T> : IEqualityComparer<TEntity>
        where TEntity : class, IEntity<T>
    {
        public bool Equals(TEntity x, TEntity y)
        {
            if (x == null || y == null)
                return false;

            // TODO: fixed x.Id = y.Id
            return x == y;
        }

        public int GetHashCode(TEntity obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
