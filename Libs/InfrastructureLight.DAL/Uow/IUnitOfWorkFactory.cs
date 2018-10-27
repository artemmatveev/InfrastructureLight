namespace InfrastructureLight.DAL.Uow
{
    using Repositories;
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(params IRepository[] repositories);
        void ReloadContext();
    }
}
