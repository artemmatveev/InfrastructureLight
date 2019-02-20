using System;
using System.Runtime.Serialization;

namespace InfrastructureLight.Common.Exceptions
{
    [Serializable]
    public sealed class FieldValueMustBeGreaterException : DomainException
    {
        public FieldValueMustBeGreaterException(string fieldNameOne, string filedNameTwo)
            : base($"Знаечние поля {fieldNameOne} должно быть больше значения поля {filedNameTwo}") { }

        FieldValueMustBeGreaterException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}

