using Nursan.Persistanse.Repository;

namespace Nursan.Persistanse.UnitOfWork
{
    public interface IUnitOfWorkKasa : IDisposable
    {
        INewRepository<T> GetKasaRepository<T>() where T : class;
        int SaveChanges();

    }
}
