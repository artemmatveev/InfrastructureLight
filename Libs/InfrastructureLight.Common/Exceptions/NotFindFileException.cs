using System;

namespace InfrastructureLight.Common.Exceptions
{
    [Serializable]
    public class NotFindFileException : DomainException
    {
        public NotFindFileException(string path) : base($"Файлы на сервере не сущесвуют или не удалось найти путь: {path}") { }
    }
}
