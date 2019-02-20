using System;
using System.Runtime.Serialization;

namespace InfrastructureLight.Common.Exceptions
{
    [Serializable]
    public sealed class EmptyException : DomainException
    {
        public EmptyException(string fieldName) 
            : base($"Знаечние поля {fieldName} должно быть задано") { }

        EmptyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
