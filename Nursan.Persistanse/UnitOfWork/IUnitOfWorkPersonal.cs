using Nursan.Persistanse.Repository;

namespace Nursan.Persistanse.UnitOfWork
{
    public interface IUnitOfWorkPersonal : IDisposable
    {
        IPersonalRepository<T> GetPersonalRepository<T>() where T : class;
        int SaveChanges();

    }
}
