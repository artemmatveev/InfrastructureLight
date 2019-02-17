using System;
namespace InfrastructureLight.BLL.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        void Rollback();
        bool HasChanges();
    }
}
