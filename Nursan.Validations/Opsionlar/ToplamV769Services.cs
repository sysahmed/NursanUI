using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Validations.Opsionlar
{
    public class ToplamV769Services
    {
        private readonly IUnitOfWork _repo;
        public ToplamV769Services(IUnitOfWork repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(_repo));
        }
        public SuccessDataResults<IzToplamV769> AddToplamV769(IzToplamV769 id)
        {
            try
            {
                var result = _repo.GetRepository<IzToplamV769>().Add(id);
                return new SuccessDataResults<IzToplamV769>(result.Data, result.Message);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SuccessDataResults<IzToplamV769> UpdateToplamV769(IzToplamV769 data)
        {
            try
            {
                var result = _repo.GetRepository<IzToplamV769>().Update(data);
                return new SuccessDataResults<IzToplamV769>(result.Data, result.Message);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
