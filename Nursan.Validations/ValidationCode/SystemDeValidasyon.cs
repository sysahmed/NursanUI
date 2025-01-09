using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.SortedList;

namespace Nursan.Validations.ValidationCode
{
    internal class SystemDeValidasyon
    {
        UnitOfWork _repo; OrFamily _orFamily;
        public SystemDeValidasyon(UnitOfWork repo, OrFamily family)
        {
            _repo = repo;
            _orFamily = family;
        }
        public IEnumerable<UrIstasyon> GetByIstasyon()
        {
            return _repo.GetRepository<UrIstasyon>().GetAll(x => x.FamilyId == _orFamily.Id).Data;
        }

        public IEnumerable<IzDonanimCount> GetByOkunan(string? barcodeIcerik)
        {
            var res = StringSpanConverter.SplitWithoutAllocationReturnArray(barcodeIcerik.AsSpan(), '-');
            var idres = StringSpanConverter.GetCharsIsDigit(res[2]);
            return _repo.GetRepository<IzDonanimCount>().GetAll(x => x.IdDonanim == idres).Data;
        }
    }
}
