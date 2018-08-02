namespace InfrastructureLight.Common.Exceptions
{
    public class ErrorSingleContextException : DomainException
    {
        public ErrorSingleContextException()
            : base("Некоторые из репозиториев имеют разные контексты. Можно передавать только репозитории с одним контекстом") { }
    }
}
