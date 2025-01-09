using Microsoft.EntityFrameworkCore;
using Nursan.Domain.Personal;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Result;
using System.Data.Common;
using System.Linq.Expressions;

namespace Nursan.Persistanse.Repository
{
    public class PersonalRepository<IEntity> : IPersonalRepository<IEntity> where IEntity : class
    {
        protected readonly DbContext _context;
        private readonly DbSet<IEntity> _dbSet;
        private static string msage;
        public PersonalRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<IEntity>();
        }
        IDataResult<DateTime> IPersonalRepository<IEntity>.TarihGetir()
        {
            try
            {
                var veri = Convert.ToDateTime(_context
                    .Set<IEntity>()
                  .FromSqlRaw(@"SELECT getdate()")
                .ToString());
                return new DataResult<DateTime>(veri, true, Messages.DataGetAll);
            }
            catch (Exception)
            {
                return new DataResult<DateTime>(DateTime.Now, false, Messages.DataGetAll);
            }

        }

        /// <summary>
        /// Deleted
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IResults IPersonalRepository<IEntity>.Delete(IEntity entity)
        {
            try
            {
                using (var context = new PersonalContext())
                {
                    var deletedEntity = context.Entry(entity);
                    deletedEntity.State = EntityState.Deleted;
                    context.SaveChanges();
                    msage = Messages.DataDeleted;
                }
                return new DataResult<IEntity>(entity, true, msage);
            }
            catch (DbException ex)
            {
                return new SuccessDataResults<IEntity>(entity, ex.Message);
            }
        }
        /// <summary>
        /// Get All Filtered Predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IDataResult<List<IEntity>> IPersonalRepository<IEntity>.GetAll(Expression<Func<IEntity, bool>> predicate)
        {
            try
            {
                using (var contect = new PersonalContext())
                {
                    var result = predicate == null
                        ? contect.Set<IEntity>().ToList()
                        : contect.Set<IEntity>().Where(predicate).ToList();
                    return new DataResult<List<IEntity>>(result, true, Messages.DataGetAll);
                }
            }
            catch (Exception ex)
            {
                return new DataResult<List<IEntity>>(null, false, ex.Message);
            }
        }
        /// <summary>
        /// Get By ID send ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IDataResult<IEntity> IPersonalRepository<IEntity>.GetById(int id)
        {
            try
            {
                var veri = _dbSet.Find(id);
                return new DataResult<IEntity>(veri, true, Messages.DataGetAll);
            }
            catch (Exception ex)
            {
                return new DataResult<IEntity>(null, false, ex.Message);
            }
        }
        /// <summary>
        /// Get Singelton
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IDataResult<IEntity> IPersonalRepository<IEntity>.Get(Expression<Func<IEntity, bool>> predicate)
        {
            try
            {
                using (var contect = new PersonalContext())
                {

                    var result = contect.Set<IEntity>().FirstOrDefault(predicate);
                    return new DataResult<IEntity>(result, true, Messages.DataGet);
                }
            }
            catch (Exception ex)
            {
                return new DataResult<IEntity>(null, false, ex.Message);
            }
        }
        /// <summary>
        /// Addet send entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// //#R2X6-15K867-AEB00000211
        IDataResult<IEntity> IPersonalRepository<IEntity>.Add(IEntity entity)
        {
            //ModelState.IsValid == ItemTypeReflectionUtility;
            try
            {
                using (var context = new PersonalContext())
                {

                    var addedEntity = context.Entry(entity);
                    addedEntity.State = EntityState.Added;
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        msage = ex.Message;


                    }
                    msage = Messages.DataAdded;
                }
                return new DataResult<IEntity>(entity, true, msage);
            }
            catch (DbException ex)
            {
                return new DataResult<IEntity>(entity, false, ex.Message);
            }

        }
        /// <summary>
        /// Updating senf entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        IDataResult<IEntity> IPersonalRepository<IEntity>.Update(IEntity entity)
        {
            try
            {
                using (var context = new PersonalContext())
                {
                    var updatedEntity = context.Entry(entity);
                    updatedEntity.State = EntityState.Modified;
                    context.SaveChanges();
                    msage = Messages.DataUpdated;
                }
                return new DataResult<IEntity>(entity, true, msage);
            }
            catch (DbException ex)
            {
                return new DataResult<IEntity>(entity, false, ex.Message); // msage = ex.Message;
            }

        }
        /// <summary>
        /// Delete Id send id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResults IPersonalRepository<IEntity>.Delete(int id)
        {
            try
            {
                var result = _dbSet.Find(id);
                _dbSet.Remove(result);
                _context.SaveChanges();
                return new Result.Result(true, Messages.DataDeleted);
            }
            catch (Exception)
            {
                return new ErrorResult(false, Messages.DataDeleted);
            }
        }
    }
}
