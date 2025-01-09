using Nursan.Domain.Entity;
using Nursan.Domain.SystemClass;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Business.Services
{
    public class SayiIzlemeSIcilBagizliService
    {
        static private UrIstasyon _istasyon;
        static UrModulerYapi _model;

        static Tarih tarihHesapla;

        private static UnitOfWork _repo;
        private static OpMashin _makine;
        //private static UrVardiya _vardiya;
        //private static List<UrIstasyon> _istasyonList;

        public SayiIzlemeSIcilBagizliService(UrIstasyon istasyon)
        {
            _repo = new UnitOfWork(new UretimOtomasyonContext());
            Initialize(istasyon);
        }

        private static void Initialize(UrIstasyon istasyon)
        {
            _repo = new UnitOfWork(new UretimOtomasyonContext());
            //_istasyonList = _repo.GetRepository<UrIstasyon>().GetAll(null).Data;
            _makine = _repo.GetRepository<OpMashin>().Get(x => x.MasineName == Environment.MachineName).Data;
            _istasyon = istasyon ?? _repo.GetRepository<UrIstasyon>().Get(x => x.MashinId == _makine.Id).Data;
            //  _vardiya = _repo.GetRepository<UrVardiya>().Get(x => x.Id == _istasyon.VardiyaId).Data;
            //_countDegerValidations = new CountDegerValidations(_repo, _makine, _vardiya, _istasyonList);
            //_countDegerValidations.Hesapla(out ortalamaCount, out vardiyaCount, out toplamCount);
            _model = _repo.GetRepository<UrModulerYapi>().Get(x => x.Id == _istasyon.ModulerYapiId).Data;
        }

        public static int SayiHesapla(string sicil, string vardiya)
        {
            Initialize(null);
            return GitModelBak(sicil, vardiya);
        }

        private static int GitModelBak(string sicil, string vardiya)
        {
            string sicilVr = $"*{sicil}";
            string pcName = $"{Environment.MachineName}{vardiya}";
            tarihHesapla = OtherTools.TarihHesab();
            switch (_model.Etap)
            {
                case "Sigorta":
                    return _repo.GetRepository<IzToplamV769>().GetAll(x => x.Sigortavar.Contains(sicilVr) && x.Sigortadate > tarihHesapla.tarih1).Data.Count();
                case "AntenKablo":
                    return _repo.GetRepository<IzToplamV769>().GetAll(x => x.Antenvar.Contains(sicilVr) && x.Antendate > tarihHesapla.tarih1).Data.Count();
                case "Konveyor":
                    return _repo.GetRepository<IzToplamV769>().GetAll(x => x.Konvar.Contains(sicilVr) && x.Kondata > tarihHesapla.tarih1).Data.Count();
                case "Gromet":
                    pcName = pcName + sicilVr;
                    var ews = _repo.GetRepository<IzToplamV769>().GetAll(x => x.Grometvar.Contains(pcName) && x.Grometdata > tarihHesapla.tarih1).Data.Count();
                    return ews;
                case "KlipTest":
                    return _repo.GetRepository<IzToplamV769>().GetAll(x => x.Klipvar.Contains(sicilVr) && x.Kliptestdata > tarihHesapla.tarih1).Data.Count();
                case "ElTest":
                    return _repo.GetRepository<IzToplamV769>().GetAll(x => x.Elvar.Contains(sicilVr) && x.Eltestdata > tarihHesapla.tarih1).Data.Count();
                case "GozKontrol":
                    return _repo.GetRepository<IzToplamV769>().GetAll(x => x.Gozvar.Contains(sicilVr) && x.Gozdata > tarihHesapla.tarih1).Data.Count();
                case "Tork":
                    return _repo.GetRepository<IzToplamV769>().GetAll(x => x.Torkvar.Contains(sicilVr) && x.Torkdate > tarihHesapla.tarih1).Data.Count();
                case "Paket":
                    return _repo.GetRepository<IzToplamV769>().GetAll(x => x.Paketvar.Contains(sicilVr) && x.Paketdata > tarihHesapla.tarih1).Data.Count();
                default:
                    return 0;
            }

        }
    }

}

