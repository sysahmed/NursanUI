using Nursan.ORM.Result;

namespace Nursan.ORM.Interface
{
    public interface IORM<IEntity> where IEntity : class
    {
        Result<List<IEntity>> Select(string whereQuery);
        Result<bool> Insert(IEntity item);
        Result<bool> Update(IEntity item, string whereQuery);
        Result<bool> Delete(IEntity item, string key);

    }
}
