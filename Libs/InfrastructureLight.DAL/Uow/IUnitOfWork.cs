using System;
namespace InfrastructureLight.DAL.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        void Rollback();
        bool HasChanges();
    }
}
