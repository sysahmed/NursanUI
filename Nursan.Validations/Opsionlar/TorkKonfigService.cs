using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.Interface;
using Nursan.Validations.SortedList;

namespace Nursan.Validations.Opsionlar
{
    public class TorkKonfigService : ITorkManager
    {
        private readonly IUnitOfWork _repo;

        public TorkKonfigService(IUnitOfWork repo)
        {
            _repo = repo;
        }

        public IDataResult<IzGenerateId> AddTorkConfig(IzGenerateId id)
        {
            var resulr = _repo.GetRepository<IzGenerateId>().Add(id);
            return new SuccessDataResults<IzGenerateId>(resulr.Data, resulr.Message);
        }

        public IDataResult<IzGenerateId> DeleteTorkKonfig(IzGenerateId id)
        {
            var result = _repo.GetRepository<IzGenerateId>().Delete(id);
            return new SuccessDataResults<IzGenerateId>(result.Message);
        }

        public IDataResult<IEnumerable<IzGenerateId>> GetAllTorkConfig(IzGenerateId id)
        {
            var resulr = _repo.GetRepository<IzGenerateId>().GetAll(null);
            return new DataResult<IEnumerable<IzGenerateId>>(resulr.Data, resulr.Success, resulr.Message);
        }

        public IDataResult<IzGenerateId> GetTorkConfigId(SyBarcodeInput id)
        {
            var res = StringSpanConverter.SplitWithoutAllocationReturnArray(id.BarcodeIcerik.AsSpan(), '-');
            var idres = StringSpanConverter.GetCharsIsDigit(res[2]);
            var resulr = _repo.GetRepository<IzGenerateId>().Get(x => x.Id == idres);
            return new SuccessDataResults<IzGenerateId>(resulr.Data, resulr.Message);
        }

        public IDataResult<IzGenerateId> UpdateTorkConfig(IzGenerateId id)
        {
            var resultIn = _repo.GetRepository<IzGenerateId>().Get(x => x.Id == id.Id);
            var resultOut = _repo.GetRepository<IzGenerateId>().Update(id);
            return new SuccessDataResults<IzGenerateId>(resultOut.Message);
        }


    }
}
