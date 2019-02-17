using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace InfrastructureLight.BLL.Uow
{
    using DAL.Repositories;

    internal static class EFContextHelper
    {
        /// <summary>
        ///     Изменение контекста репозиториев
        /// </summary>        
        public static void SetContext(IRepository[] repositories, DbContext context)
        {
            // Перебираем репозитории
            // и устанавливаем контекст:
            foreach (var repo in repositories)
            {
                _SetContext(repo, context);
            }
        }

        private static void _SetContext(IRepository repository, DbContext context)
        {
            // Изменение контекста переданного 
            // репозитория:
            var contextField = repository.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(y => y.FieldType == typeof(DbContext));

            if (contextField != null)
            {
                contextField.SetValue(repository, context);
            }

            // Изменение контекста полей, 
            // которые являются репозиториями:
            bool IsSuitable(FieldInfo fi)
            {
                var value = fi.GetValue(repository);
                return value.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                           .FirstOrDefault(y => y.FieldType == typeof(DbContext)) != null;
            }

            var repositoriesInfo = repository.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => typeof(IRepository).IsAssignableFrom(x.FieldType) && IsSuitable(x));

            foreach (FieldInfo repositoryInfo in repositoriesInfo)
            {
                _SetContext(((IRepository)repositoryInfo.GetValue(repository)), context);
            }
        }
    }
}
