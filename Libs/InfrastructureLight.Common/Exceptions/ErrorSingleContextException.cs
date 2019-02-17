using System;

namespace InfrastructureLight.Common.Exceptions
{
    [Serializable]
    public class ErrorSingleContextException : DomainException
    {
        public ErrorSingleContextException()
            : base("Некоторые из репозиториев имеют разные контексты. Можно передавать только репозитории с одним контекстом") { }
    }
}
