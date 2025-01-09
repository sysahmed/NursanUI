using Microsoft.EntityFrameworkCore;
using Nursan.Domain.SystemClass;
using Nursan.Persistanse.Result;
using System.Linq.Expressions;

namespace Nursan.Persistanse.Repository
{
    public class NewRepository<TEntity> : INewRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public NewRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
        }
        public Task<IDataResult<DateTime>> GetCurrentDateAsync()
        {
            try
            {
                // Изпълнение на SQL заявка за получаване на текущата дата и час
                var veri = _context
                    .Set<IEntity>()
                    .FromSqlRaw(@"SELECT getdate() AS CurrentDate")
                    .AsEnumerable()
                    .Select(e => (DateTime)e.GetType().GetProperty("CurrentDate").GetValue(e))
                    .FirstOrDefault();

                // Проверка дали резултатът е валиден
                if (veri != default(DateTime))
                {
                    var dataResult = new DataResult<DateTime>(veri, true, "Success");
                    return Task.FromResult<IDataResult<DateTime>>(dataResult);
                }
                else
                {
                    return Task.FromResult<IDataResult<DateTime>>(new DataResult<DateTime>(default, false, "Date could not be retrieved"));
                }
            }
            catch (Exception ex)
            {
                var errorResult = new DataResult<DateTime>(default, false, ex.Message);
                return Task.FromResult<IDataResult<DateTime>>(errorResult);
            }
        }

        public Task<IDataResult<IEnumerable<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = predicate == null ? _dbSet : _dbSet.Where(predicate);
                var dataResult = new DataResult<IEnumerable<TEntity>>(result, true, "Success");
                return Task.FromResult<IDataResult<IEnumerable<TEntity>>>(dataResult);
            }
            catch (Exception ex)
            {
                var errorResult = new DataResult<IEnumerable<TEntity>>(null, false, ex.Message);
                return Task.FromResult<IDataResult<IEnumerable<TEntity>>>(errorResult);
            }
        }
 
        public Task<IDataResult<TEntity>> GetByIdAsync(int id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(entity, true, "Success"));
                }
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(null, false, "Entity not found"));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(null, false, ex.Message));
            }
        }

        public Task<IDataResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entity = _dbSet.FirstOrDefault(predicate);
                if (entity != null)
                {
                    return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(entity, true, "Success"));
                }
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(null, false, "Entity not found"));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(null, false, ex.Message));
            }
        }

        // Добавя нов запис
        public Task<IDataResult<TEntity>> AddAsync(TEntity entity)
        {
            try
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(entity, true, "Entity added successfully"));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(null, false, ex.Message));
            }
        }

        // Обновява съществуващ запис
        public Task<IDataResult<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                _context.SaveChanges();
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(entity, true, "Entity updated successfully"));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(null, false, ex.Message));
            }
        }

        // Изтрива запис
        public Task<IDataResult<TEntity>> DeleteAsync(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(entity, true, "Entity deleted successfully"));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(null, false, ex.Message));
            }
        }

        // Изтрива запис по ID
        public Task<IDataResult<TEntity>> DeleteByIdAsync(int id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                if (entity == null)
                {
                    return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(null, false, "Entity not found"));
                }

                _dbSet.Remove(entity);
                _context.SaveChanges();
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(entity, true, "Entity deleted successfully"));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IDataResult<TEntity>>(new DataResult<TEntity>(null, false, ex.Message));
            }
        }
    }
}
