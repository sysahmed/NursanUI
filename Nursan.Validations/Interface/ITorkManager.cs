using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;

namespace Nursan.Validations.Interface
{
    public interface ITorkManager
    {
        public IDataResult<IzGenerateId> GetTorkConfigId(SyBarcodeInput id);
        public IDataResult<IzGenerateId> AddTorkConfig(IzGenerateId id);
        public IDataResult<IzGenerateId> UpdateTorkConfig(IzGenerateId id);
        public IDataResult<IzGenerateId> DeleteTorkKonfig(IzGenerateId id);
        public IDataResult<IEnumerable<IzGenerateId>> GetAllTorkConfig(IzGenerateId id);


    }
}
