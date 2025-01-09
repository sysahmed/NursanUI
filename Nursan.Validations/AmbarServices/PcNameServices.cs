using Nursan.Domain.AmbarModels;
using Nursan.Persistanse.Repository;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;
using System.Linq.Expressions;

namespace Nursan.Validations.AmbarServices
{
    public class PcNameServices : IRepository<PcName>
    {
        private readonly IUnitOfWork _repo;

        public PcNameServices(IUnitOfWork repo)
        {
            _repo = repo;
        }

        public IDataResult<PcName> Add(PcName entity)
        {
            return _repo.GetRepository<PcName>().Add(entity);
        }

        public IResults Delete(PcName entity)
        {
            return _repo.GetRepository<PcName>().Delete(entity);
        }

        public IResults Delete(int id)
        {
            return _repo.GetRepository<PcName>().Delete(id);

        }


        public IDataResult<PcName> Get(Expression<Func<PcName, bool>> predicate)
        {
            try
            {

                var result = _repo.GetRepository<PcName>().Get(predicate);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IDataResult<List<PcName>> GetAll(Expression<Func<PcName, bool>> predicate)
        {
            return _repo.GetRepository<PcName>().GetAll(predicate);
        }

        public IDataResult<PcName> GetById(int id)
        {
            return _repo.GetRepository<PcName>().Get(x => x.Pcid == id);
        }

        public IDataResult<DateTime> TarihGetir()
        {
            return new DataResult<DateTime>(TarihHesapla.GetSystemDate(), true, "System Date");
        }

        public IDataResult<PcName> Update(PcName entity)
        {
            return _repo.GetRepository<PcName>().Update(entity);
        }
    }
}
