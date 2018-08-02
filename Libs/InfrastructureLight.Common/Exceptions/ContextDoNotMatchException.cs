namespace InfrastructureLight.Common.Exceptions
{
    public class ContextDoNotMatchException : DomainException
    {
        public ContextDoNotMatchException()
            : base("Контексты не совпадают") { }
    }
}
