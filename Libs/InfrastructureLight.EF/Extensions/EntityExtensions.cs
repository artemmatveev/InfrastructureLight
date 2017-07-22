using System.Linq;
using System.ComponentModel.DataAnnotations;

using InfrastructureLight.Domain;

namespace InfrastructureLight.EF.Extensions
{
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
                        && (x as EditableAttribute).AllowEdit == false
                        && (x as EditableAttribute).AllowInitialValue == false);

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
