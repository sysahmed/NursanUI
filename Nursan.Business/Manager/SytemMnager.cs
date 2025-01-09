using Nursan.Business.Services;
using Nursan.Domain.SystemClass;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using System.Linq.Expressions;

namespace Nursan.Business.Manager
{
    public class SytemMnager<T> : ISytemService<T> where T : BaseEntity
    {
        private readonly IUnitOfWork _unitOfWork;
        public SytemMnager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IDataResult<bool> Add(T entity)
        {
            var result = _unitOfWork.GetRepository<T>().Add(entity);
            return new DataResult<bool>(true, result.Success, result.Message);
        }

        public IDataResult<bool> Delete(T entity)
        {
            var result = _unitOfWork.GetRepository<T>().Delete(entity);
            return new DataResult<bool>(true, result.Success, result.Message);
        }

        public IDataResult<T> Get(Expression<Func<T, bool>> predicate)
        {
            var result = _unitOfWork.GetRepository<T>().Get(predicate);
            return result;
        }

        public IDataResult<bool> Update(T entity)
        {
            var result = _unitOfWork.GetRepository<T>().Update(entity);
            return new DataResult<bool>(true, result.Success, result.Message);
        }

        IDataResult<IEnumerable<T>> ISytemService<T>.GetAll(Expression<Func<T, bool>> predicate)
        {
            var result = _unitOfWork.GetRepository<T>().GetAll(predicate);
            return new DataResult<IEnumerable<T>>(result.Data, result.Success, result.Message);
        }
    }
}
