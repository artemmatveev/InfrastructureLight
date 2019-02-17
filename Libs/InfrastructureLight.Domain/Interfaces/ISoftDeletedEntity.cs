namespace InfrastructureLight.Domain.Interfaces
{
    public interface ISoftDeletedEntity : IEntity
    {
        bool IsDeleted { get; }
        void Delete();
    }
}
