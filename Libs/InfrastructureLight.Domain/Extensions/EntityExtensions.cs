using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InfrastructureLight.Domain.Extensions
{
    using Interfaces;

    public static class EntityExtensions
    {
        /// <summary>
        ///     Copy data from an entity to an entity
        /// </summary>
        public static void CopyDataTo<TEntity>(this TEntity source, TEntity dest) where TEntity : IEntity
        {
            var sourceType = source.GetType();
            var destType = dest.GetType();

            foreach (var info in sourceType.GetProperties())
            {
                if (info.Name != "EntityConflict" && info.Name != "EntityState")
                {
                    bool isReadOnly = info.GetCustomAttributes(false).Any(x => (x is EditableAttribute)
                        && !(x as EditableAttribute).AllowEdit
                        && !(x as EditableAttribute).AllowInitialValue);

                    var secondInfo = destType.GetProperty(info.Name);

                    if (!isReadOnly && secondInfo != null && secondInfo.CanWrite)
                    {
                        secondInfo.SetValue(dest, info.GetValue(source, null), null);
                    }
                }
            }
        }
    }
}
