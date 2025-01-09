using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.Interface;
using System.Linq.Expressions;

namespace Nursan.Validations.Opsionlar
{
    public class HarnessConfigManager : IHarnesConfigServices
    {
        private readonly IUnitOfWork _repo;

        public HarnessConfigManager(IUnitOfWork repo)
        {
            _repo = repo;
        }

        public IUnitOfWork Repo => _repo;

        public IDataResult<OrHarnessConfig> Add(OrHarnessConfig entity)
        {
            return Repo.GetRepository<OrHarnessConfig>().Add(entity);
        }

        public IResults Delete(OrHarnessConfig entity)
        {
            return Repo.GetRepository<OrHarnessConfig>().Delete(entity);
        }

        public IResults Delete(int id)
        {
            return Repo.GetRepository<OrHarnessConfig>().Delete(id);
        }

        public IDataResult<OrHarnessConfig> Get(Expression<Func<OrHarnessConfig, bool>> predicate)
        {
            return Repo.GetRepository<OrHarnessConfig>().Get(predicate);
        }

        public IDataResult<List<OrHarnessConfig>> GetAll(Expression<Func<OrHarnessConfig, bool>> predicate)
        {
            return Repo.GetRepository<OrHarnessConfig>().GetAll(predicate);
        }

        public IDataResult<OrHarnessConfig> GetById(int id)
        {
            return Repo.GetRepository<OrHarnessConfig>().Get(x => x.Id == id);
        }

        public IDataResult<DateTime> TarihGetir()
        {
            return Repo.GetRepository<OrHarnessConfig>().TarihGetir();
        }

        public IDataResult<OrHarnessConfig> Update(OrHarnessConfig entity)
        {
            return Repo.GetRepository<OrHarnessConfig>().Update(entity);
        }
    }
}
