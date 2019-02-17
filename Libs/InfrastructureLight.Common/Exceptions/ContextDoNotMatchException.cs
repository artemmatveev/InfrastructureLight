using System;

namespace InfrastructureLight.Common.Exceptions
{
    [Serializable]
    public class ContextDoNotMatchException : DomainException
    {
        public ContextDoNotMatchException()
            : base("Контексты не совпадают") { }
    }
}
