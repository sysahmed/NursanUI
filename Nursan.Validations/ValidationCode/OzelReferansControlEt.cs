using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Validations.ValidationCode
{
    public class OzelReferansControlEt
    {
        public readonly UnitOfWork _repo;
        public OzelReferansControlEt(UnitOfWork repo)
        {
            _repo = repo;
        }
        public IDataResult<OrOzelReferans> ControletSystemi(string referans, string id)
        {
            try
            {
                var ozelReferans = _repo.GetRepository<OrOzelReferans>().Get(x => x.referans == referans);
                var ozelId = _repo.GetRepository<OrOzelReferans>().Get(x => x.referans == id);
                if (ozelReferans.Data != null)
                {
                    return new DataResult<OrOzelReferans>(ozelReferans.Data, true, ozelId.Message);
                }
                else if (ozelId.Data != null)
                {
                    return new DataResult<OrOzelReferans>(ozelId.Data, true, ozelId.Message);
                }
                else
                {
                    return new DataResult<OrOzelReferans>(null, false, "Devam");
                }

            }
            catch (Exception ex)
            {
                return new DataResult<OrOzelReferans>(null, false, ex.Message);
            }
        }
    }
}
