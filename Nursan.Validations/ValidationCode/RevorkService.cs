using Nursan.Domain.Entity;
using Nursan.Persistanse.Repository;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using System.Linq.Expressions;

namespace Nursan.Validations.ValidationCode
{
    public class RevorkService : IRepositoryAmabar<ErRework>
    {
        private readonly IUnitOfWork _repo;
        public RevorkService(IUnitOfWork repo)
        {
            _repo = repo;
        }
        public IDataResult<ErRework> Add(ErRework entity)
        {
            return _repo.GetRepository<ErRework>().Add(entity);
        }

        public IResults Delete(ErRework entity)
        {
            return _repo.GetRepository<ErRework>().Delete(entity);
        }

        public IResults Delete(int id)
        {
            return _repo.GetRepository<ErRework>().Delete(id);
        }

        public IDataResult<ErRework> Get(Expression<Func<ErRework, bool>> predicate)
        {
            return _repo.GetRepository<ErRework>().Get(predicate);
        }

        public IDataResult<List<ErRework>> GetAll(Expression<Func<ErRework, bool>> predicate)
        {
            return _repo.GetRepository<ErRework>().GetAll(predicate);
        }

        public IDataResult<ErRework> GetById(int id)
        {
            return _repo.GetRepository<ErRework>().GetById(id);
        }

        public IDataResult<DateTime> TarihGetir()
        {
            return new DataResult<DateTime>(TarihHesapla.GetSystemDate(), true, "System Date");
        }

        public IDataResult<ErRework> Update(ErRework entity)
        {
            return _repo.GetRepository<ErRework>().Update(entity);
        }


    }
}
