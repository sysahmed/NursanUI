using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Validations.ValidationCode
{

    public class MasaPanoValidasyonlari
    {
        private readonly UnitOfWork _repo;
        public MasaPanoValidasyonlari(UnitOfWork repo)
        {
            _repo = repo;
        }
        public UrKonveyorNumara MasaGet(string masaBarcode)
        {
            try
            {
                return _repo.GetRepository<UrKonveyorNumara>().Get(x => x.MasaNo == masaBarcode).Data;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
    }
}
