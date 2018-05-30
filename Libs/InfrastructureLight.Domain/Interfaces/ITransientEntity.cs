namespace InfrastructureLight.Domain.Interfaces {
    public interface ITransientEntity : IEntity {
        bool IsTransient { get; }
    }
}
