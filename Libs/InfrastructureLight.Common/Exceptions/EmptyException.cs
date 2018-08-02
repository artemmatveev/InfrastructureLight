using System;

namespace InfrastructureLight.Common.Exceptions
{
    [Serializable]
    public sealed class EmptyException : DomainException {
        public EmptyException(string fieldName) : base($"Знаечние поля {fieldName} должно быть задано") {}
    }
}
