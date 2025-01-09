using Nursan.Persistanse.Result;
using System.Linq.Expressions;

namespace Nursan.Persistanse.Repository
{
    public interface INewRepository<IEntity> where IEntity : class
	{
        Task<IDataResult<DateTime>> GetCurrentDateAsync();
        Task<IDataResult<IEnumerable<IEntity>>> GetAllAsync(Expression<Func<IEntity, bool>> predicate);
        Task<IDataResult<IEntity>> GetByIdAsync(int id);
        Task<IDataResult<IEntity>> GetAsync(Expression<Func<IEntity, bool>> predicate);
        Task<IDataResult<IEntity>> AddAsync(IEntity entity);
        Task<IDataResult<IEntity>> UpdateAsync(IEntity entity);
        Task<IDataResult<IEntity>> DeleteAsync(IEntity entity);
        Task<IDataResult<IEntity>> DeleteByIdAsync(int id);
    }
}
