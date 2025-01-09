using Nursan.Persistanse.Repository;

namespace Nursan.Persistanse.UnitOfWork
{
    public interface IUnitOfWorkAmbar : IDisposable
    {
        IRepositoryAmabar<T> GetRepositoryAmbar<T>() where T : class;
        int SaveChanges();

    }
}
