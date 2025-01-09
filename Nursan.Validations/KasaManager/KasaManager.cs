using Microsoft.EntityFrameworkCore;
using Nursan.Domain.SystemClass;
using Nursan.Persistanse.Repository;
using Nursan.Persistanse.Result;
using System.Linq.Expressions;

namespace Nursan.Validations.KasaManager
{
    public class KasaManager<T> : INewRepository<T> where T : class, IEntity
    {
        protected readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
       //private static string msage;

        public KasaManager(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IDataResult<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                var result = predicate == null ? await _dbSet.ToListAsync() : await _dbSet.Where(predicate).ToListAsync();
                return new DataResult<IEnumerable<T>>(result, true, "Data retrieved successfully");
            }
            catch (Exception ex)
            {
                return new DataResult<IEnumerable<T>>(null, false, ex.Message);
            }
        }

        public async Task<IDataResult<T>> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new DataResult<T>(entity, true, "Entity added successfully");
            }
            catch (Exception ex)
            {
                return new DataResult<T>(null, false, ex.Message);
            }
        }

        public async Task<IDataResult<T>> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return new DataResult<T>(entity, true, "Entity deleted successfully");
            }
            catch (Exception ex)
            {
                return new DataResult<T>(null, false, ex.Message);
            }
        }

        public async Task<IDataResult<T>> DeleteByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return new DataResult<T>(null, false, "Entity not found");
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return new DataResult<T>(entity, true, "Entity deleted successfully");
            }
            catch (Exception ex)
            {
                return new DataResult<T>(null, false, ex.Message);
            }
        }

        public async Task<IDataResult<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(predicate);
                if (entity == null)
                {
                    return new DataResult<T>(null, false, "Entity not found");
                }

                return new DataResult<T>(entity, true, "Entity retrieved successfully");
            }
            catch (Exception ex)
            {
                return new DataResult<T>(null, false, ex.Message);
            }
        }

        public async Task<IDataResult<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return new DataResult<T>(null, false, "Entity not found");
                }

                return new DataResult<T>(entity, true, "Entity retrieved successfully");
            }
            catch (Exception ex)
            {
                return new DataResult<T>(null, false, ex.Message);
            }
        }

        public async Task<IDataResult<DateTime>> GetCurrentDateAsync()
        {
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync("SELECT getdate()");
                var currentDate = DateTime.Now; // This assumes the SQL Server time aligns with the local server.
                return new DataResult<DateTime>(currentDate, true, "Current date retrieved successfully");
            }
            catch (Exception ex)
            {
                return new DataResult<DateTime>(default, false, ex.Message);
            }
        }

        public async Task<IDataResult<T>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return new DataResult<T>(entity, true, "Entity updated successfully");
            }
            catch (Exception ex)
            {
                return new DataResult<T>(null, false, ex.Message);
            }
        }
    }

}
