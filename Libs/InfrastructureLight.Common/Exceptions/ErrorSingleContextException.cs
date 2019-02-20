using System;
using System.Runtime.Serialization;

namespace InfrastructureLight.Common.Exceptions
{
    [Serializable]
    public class ErrorSingleContextException : DomainException
    {
        public ErrorSingleContextException()
            : base("Некоторые из репозиториев имеют разные контексты. Можно передавать только репозитории с одним контекстом") { }

        protected ErrorSingleContextException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
