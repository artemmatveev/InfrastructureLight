namespace InfrastructureLight.BLL.Uow
{
    using DAL.Repositories;

    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(params IRepository[] repositories);
    }
}
