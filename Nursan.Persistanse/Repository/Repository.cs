using Microsoft.EntityFrameworkCore;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Result;
using System.Data.Common;
using System.Linq.Expressions;

namespace Nursan.Persistanse.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
        }

        public IDataResult<DateTime> TarihGetir()
        {
            try
            {
                var dateTime = _context.Database.ExecuteSqlRaw("SELECT GETDATE()").ToString();
                return new DataResult<DateTime>(Convert.ToDateTime(dateTime), true, Messages.DataGetAll);
            }
            catch (FormatException ex)
            {
                return new DataResult<DateTime>(DateTime.Now, false, ex.Message);
            }
            catch (Exception ex)
            {
                return new DataResult<DateTime>(DateTime.Now, false, ex.Message);
            }
        }

        public IDataResult<TEntity> Add(TEntity entity)
        {
            try
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
                return new DataResult<TEntity>(entity, true, Messages.DataAdded);
            }
            catch (DbUpdateException ex)
            {
                var sqlEx = ex.InnerException;
                return new DataResult<TEntity>(entity, false, ex.Message);
            }
            catch (Exception ex)
            {
                return new DataResult<TEntity>(entity, false, ex.Message);
            }
        }

        public IDataResult<TEntity> Update(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                _context.SaveChanges();
                return new DataResult<TEntity>(entity, true, Messages.DataUpdated);
            }
            catch (DbException ex)
            {
                return new DataResult<TEntity>(entity, false, ex.Message);
            }
        }

        public IResults Delete(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
                return new Result.Result(true, Messages.DataDeleted);
            }
            catch (DbException ex)
            {
                return new ErrorResult(false, ex.Message);
            }
        }

        public IResults Delete(int id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    _context.SaveChanges();
                    return new Result.Result(true, Messages.DataDeleted);
                }
                return new ErrorResult(false, Messages.DataNotFound);
            }
            catch (DbException ex)
            {
                return new ErrorResult(false, ex.Message);
            }
        }

        public IDataResult<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            try
            {
                var result = predicate == null ? _dbSet.ToList() : _dbSet.Where(predicate).ToList();
                return new DataResult<List<TEntity>>(result, true, Messages.DataGetAll);
            }
            catch (Exception ex)
            {
                return new DataResult<List<TEntity>>(null, false, ex.Message);
            }
        }

        public IDataResult<TEntity> GetById(int id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                return new DataResult<TEntity>(entity, entity != null, entity != null ? Messages.DataGetAll : Messages.DataNotFound);
            }
            catch (Exception ex)
            {
                return new DataResult<TEntity>(null, false, ex.Message);
            }
        }

        public IDataResult<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = _dbSet.FirstOrDefault(predicate);
                return new DataResult<TEntity>(result, result != null, result != null ? Messages.DataGet : Messages.DataNotFound);
            }
            catch (Exception ex)
            {
                return new DataResult<TEntity>(null, false, ex.Message);
            }
        }
    }
}
