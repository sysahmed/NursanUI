using Microsoft.EntityFrameworkCore;
using Nursan.Persistanse.Repository;

namespace Nursan.Persistanse.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private bool _disposed;

        public UnitOfWork(DbContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Логика за обработка на DbUpdateException, ако е необходимо
                throw new InvalidOperationException("Unable to save changes to the database.", ex);
            }
            catch (Exception ex)
            {
                // Логика за обработка на общи изключения
                throw new InvalidOperationException("An error occurred while saving changes.", ex);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
