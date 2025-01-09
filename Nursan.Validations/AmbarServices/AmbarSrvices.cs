using Nursan.Business.Manager;
using Nursan.Domain.AmbarModels;
using Nursan.Persistanse.Repository;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;
using System.Linq.Expressions;

namespace Nursan.Validations.AmbarServices
{
    public class AmbarSrvices : IRepository<Islemler>
    {
        private readonly IUnitOfWork _repo;

        public AmbarSrvices(IUnitOfWork repo)
        {
            _repo = repo;
        }

        public IDataResult<Islemler> Add(Islemler entity)
        {
            return _repo.GetRepository<Islemler>().Add(entity);
        }

        public IResults Delete(Islemler entity)
        {
            return _repo.GetRepository<Islemler>().Delete(entity);
        }

        public IResults Delete(int id)
        {
            return _repo.GetRepository<Islemler>().Delete(id);
        }

        public IDataResult<Islemler> Get(Expression<Func<Islemler, bool>> predicate)
        {
            try
            {
                return _repo.GetRepository<Islemler>().Get(predicate);
            }
            catch (ErrorExceptionHandller)
            {

                throw;
            }
        }

        public IDataResult<List<Islemler>> GetAll(Expression<Func<Islemler, bool>> predicate)
        {
            return _repo.GetRepository<Islemler>().GetAll(predicate);
        }

        public IDataResult<Islemler> GetById(int id)
        {
            return _repo.GetRepository<Islemler>().Get(x => x.IslemId == id);
        }

        public IDataResult<DateTime> TarihGetir()
        {
            return new DataResult<DateTime>(TarihHesapla.GetSystemDate(), true, "System Date");
        }

        public IDataResult<Islemler> Update(Islemler entity)
        {
            return _repo.GetRepository<Islemler>().Update(entity);
        }
    }
}
