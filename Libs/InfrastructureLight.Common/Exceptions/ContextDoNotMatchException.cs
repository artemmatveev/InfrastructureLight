using System;
using System.Runtime.Serialization;

namespace InfrastructureLight.Common.Exceptions
{
    [Serializable]
    public class ContextDoNotMatchException : DomainException
    {        
        public ContextDoNotMatchException()
            : base("Контексты не совпадают") { }

        protected ContextDoNotMatchException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
