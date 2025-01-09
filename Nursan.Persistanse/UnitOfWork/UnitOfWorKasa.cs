using Microsoft.EntityFrameworkCore;
using Nursan.Persistanse.Repository;

namespace Nursan.Persistanse.UnitOfWork
{

    public class UnitOfWorKasa : IUnitOfWorkKasa,IDisposable
    {
        public UnitOfWorKasa(DbContext dbContext)
        {
            _context = dbContext;
        }
        private readonly DbContext _context;


        public INewRepository<T> GetKasaRepository<T>() where T : class
        {
            return new NewRepository<T>(_context);
        }
        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception) { throw; }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _context.Dispose();
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
