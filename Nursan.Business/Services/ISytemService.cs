using Nursan.Domain.SystemClass;
using Nursan.Persistanse.Result;
using System.Linq.Expressions;

namespace Nursan.Business.Services
{
    public interface ISytemService<T> where T : BaseEntity
    {
        IDataResult<T> Get(Expression<Func<T, bool>> predicate);
        IDataResult<bool> Add(T entity);
        IDataResult<bool> Update(T entity);
        IDataResult<bool> Delete(T entity);
        IDataResult<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);
    }
}
