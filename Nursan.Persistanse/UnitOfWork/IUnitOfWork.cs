using Nursan.Persistanse.Repository;

namespace Nursan.Persistanse.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        int SaveChanges();

    }
}
