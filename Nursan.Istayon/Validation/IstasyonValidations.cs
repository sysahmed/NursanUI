using Nursan.Domain.Entity;
using Nursan.Persistanse.OrmDto;

namespace Nursan.Istayon.Validation
{
    public class IstasyonValidations
    {
        private IEnumerable<UrIstasyon> _istasyon;
        private IEnumerable<UrVardiya> _vardiya;
        private IEnumerable<OpMashin> _makine;

        public IEnumerable<UrIstasyon> GetIstasyons(string vardiya)
        {
            try
            {
                var makine = GetMakine().First();
                var vardiam = GeGetVardiya(vardiya).First();
                var verimIstasyon = UrIstasyonORM.Current.Select($" Where MashinId ='{makine.Id}' and VardiyaId = '{vardiam.Id}'").Data;
                return verimIstasyon;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<UrIstasyon> GetIstasyonsFromFamily(string familyId)
        {
            //var makine = GetMakine().First();
            //var vardiam = GeGetVardiya(vardiya).First();
            var verimIstasyon = UrIstasyonORM.Current.Select($" Where familyId = '{familyId}'").Data;
            return verimIstasyon;
        }
        public IEnumerable<OpMashin> GetMakine()
        {
            return OpMashinORM.Current.Select($" where MasineName ='{Environment.MachineName}'").Data;
        }
        public IEnumerable<UrVardiya> GeGetVardiya(string vardiya)
        {
            return UrVardiyaORM.Current.Select($" where Name='{vardiya}'").Data;
        }
    }
}
