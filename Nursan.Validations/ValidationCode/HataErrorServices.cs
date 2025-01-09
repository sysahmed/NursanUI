using Nursan.Domain.Entity;
using Nursan.Persistanse.Repository;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using System.Linq.Expressions;

namespace Nursan.Validations.ValidationCode
{
    public class HataErrorServices : IRepositoryAmabar<ErTestHatalari>
    {
        private readonly IUnitOfWork _repo;
        public HataErrorServices(IUnitOfWork repo)
        {
            _repo = repo;
        }
        public IDataResult<ErTestHatalari> Add(ErTestHatalari entity)
        {
            return _repo.GetRepository<ErTestHatalari>().Add(entity);
        }

        public IResults Delete(ErTestHatalari entity)
        {
            return _repo.GetRepository<ErTestHatalari>().Add(entity);
        }

        public IResults Delete(int id)
        {
            return _repo.GetRepository<ErTestHatalari>().Delete(id);
        }

        public IDataResult<ErTestHatalari> Get(Expression<Func<ErTestHatalari, bool>> predicate)
        {
            return _repo.GetRepository<ErTestHatalari>().Get(predicate);
        }

        public IDataResult<List<ErTestHatalari>> GetAll(Expression<Func<ErTestHatalari, bool>> predicate)
        {
            return _repo.GetRepository<ErTestHatalari>().GetAll(predicate);
        }

        public IDataResult<ErTestHatalari> GetById(int id)
        {
            return _repo.GetRepository<ErTestHatalari>().GetById(id);
        }

        public IDataResult<DateTime> TarihGetir()
        {
            return new DataResult<DateTime>(TarihHesapla.GetSystemDate(), true, "System Date");
        }

        public IDataResult<ErTestHatalari> Update(ErTestHatalari entity)
        {
            return _repo.GetRepository<ErTestHatalari>().Update(entity);
        }
    }
}
