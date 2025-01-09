using Nursan.Persistanse.Result;
using System.Linq.Expressions;

namespace Nursan.Persistanse.Repository
{

    public interface IPersonalRepository<IEntity> where IEntity : class
    {
        IDataResult<DateTime> TarihGetir();
        IDataResult<List<IEntity>> GetAll(Expression<Func<IEntity, bool>> predicate);
        IDataResult<IEntity> GetById(int id);
        IDataResult<IEntity> Get(Expression<Func<IEntity, bool>> predicate);
        IDataResult<IEntity> Add(IEntity entity);
        IDataResult<IEntity> Update(IEntity entity);
        IResults Delete(IEntity entity);
        IResults Delete(int id);

    }
}
