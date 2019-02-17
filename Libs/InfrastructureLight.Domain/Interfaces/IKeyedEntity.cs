namespace InfrastructureLight.Domain.Interfaces
{
    public interface IKeyedEntity<T> : ITransientEntity
    {
        T Id { get; set; }
    }
}
