using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Business.Services
{
    public class GenrateIdDonanimManager
    {
        private readonly IUnitOfWork _repo;
        public GenrateIdDonanimManager(IUnitOfWork repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(_repo));
        }
        public SuccessDataResults<IzGenerateId> AddGenbrateId(IzGenerateId id)
        {
            try
            {
                var result = _repo.GetRepository<IzGenerateId>().Add(id);
                return new SuccessDataResults<IzGenerateId>(result.Data, result.Message);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public SuccessDataResults<IzGenerateId> UpdateIDGenerate(IzGenerateId data)
        {
            try
            {
                var result = _repo.GetRepository<IzGenerateId>().Update(data);
                return new SuccessDataResults<IzGenerateId>(result.Data, result.Message);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
