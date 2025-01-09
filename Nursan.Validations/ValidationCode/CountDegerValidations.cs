using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.Persistanse.UnitOfWork;
using Nursan.XMLTools;

namespace Nursan.Validations.ValidationCode
{
    public class CountDegerValidations
    {

        private readonly UnitOfWork _repo;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static Random random = new Random();

        List<IzDonanimCount> _zDonanimCountList;
        UrIstasyon _istasyonNew;
        public CountDegerValidations(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList)
        {
            _repo = repo;
            _makine = makine;
            _vardiya = vardiya;
            _istasyonList = istasyonList;
            _istasyonNew = new UrIstasyon();
            _zDonanimCountList = new List<IzDonanimCount>();
        }
        public void HesaplaPersonal(out int Ortalama, out int Vardiya, out int Toplama, out string Sicil)
        {
            //_makine.Id = 4;
            try
            {
                TarihHIM tarih = TarihHesapla.TarihHesab();
                _istasyonNew = _repo.GetRepository<UrIstasyon>().Get(x => x.MashinId == _makine.Id && x.VardiyaId == _vardiya.Id).Data;
                _zDonanimCountList = _repo.GetRepository<IzDonanimCount>()
                    .GetAll(x => x.UrIstasyon.ModulerYapiId == _istasyonNew.ModulerYapiId && x.UrIstasyon.FamilyId == _istasyonNew.FamilyId && x.CreateDate >= tarih.tarih1 && x.CreateDate <= tarih.tarih2)
                    .Data;
                var result = _zDonanimCountList.Where(x => x.MashinId == _makine.Id && x.VardiyaId == _vardiya.Id);
                int randomNumberInRange = random.Next(1, 10);
                _istasyonList = _repo.GetRepository<UrIstasyon>().GetAll(x => x.ModulerYapiId == _istasyonNew.ModulerYapiId && x.FamilyId == _istasyonNew.FamilyId).Data;
                _istasyonNew = _istasyonList.FirstOrDefault(x => x.MashinId == _istasyonNew.MashinId && x.VardiyaId == _istasyonNew.VardiyaId);
                TimeSpan timeSpan = (DateTime)result.Last().CreateDate - (DateTime)result.First().CreateDate;
                Double sayiSati = timeSpan.TotalMinutes;
                if (sayiSati > 0)
                {
                    if (_istasyonNew.Realadet > 0)
                    {
                        if (result.Count() / sayiSati / 60 < _istasyonNew.Realadet)
                        {
                            _istasyonNew.Sayi = ((int)_istasyonNew.Realadet * (int)(sayiSati / 60)) + randomNumberInRange;
                        }
                        else
                        {
                            _istasyonNew.Sayi = result.Count();

                        }
                    }
                    else
                    {
                        _istasyonNew.Sayi = result.Count() + (int)_istasyonNew.Sayicarp;
                    }
                }
                _istasyonNew.Orta = GitOrtalamaHesapla((int)_istasyonNew.Sayi, sayiSati);
                _repo.GetRepository<UrIstasyon>().Update(_istasyonNew);

                var modulerYapidakiCountToplam = _istasyonList
                    // .Where(x => x.ModulerYapiId == _istasyonNew.ModulerYapiId && x.FamilyId == _istasyonNew.FamilyId)
                    .Sum(x => x.Sayi);

                var modulerYapidakiCount = _istasyonList
                    .Where(x => x.Id == _istasyonNew.Id)
                    .Sum(x => x.Sayi);

                Vardiya = (int)_istasyonNew.Sayi;
                Toplama = (int)modulerYapidakiCountToplam;// + (int)modulerYapidakiCount;
                Ortalama = (int)_istasyonNew.Orta;
                Sicil = "";
                //Messaglama.MessagYaz($"{Vardiya.ToString()}-{Toplama}-{Ortalama}");
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz(ex.Message);
                // Логика за обработка на грешката, ако е необходимо
                Toplama = 0;
                Ortalama = 0;
                Vardiya = 0;
                Sicil = "";
            }
        }

        public void Hesapla(out int Ortalama, out int Vardiya, out int Toplama)
        {
            //_makine.Id = 4;
            try
            {
                TarihHIM tarih = TarihHesapla.TarihHesab();
                _istasyonNew = _repo.GetRepository<UrIstasyon>().Get(x => x.MashinId == _makine.Id && x.VardiyaId == _vardiya.Id).Data;
                _zDonanimCountList = _repo.GetRepository<IzDonanimCount>()
                    .GetAll(x => x.UrIstasyon.ModulerYapiId == _istasyonNew.ModulerYapiId && x.UrIstasyon.FamilyId == _istasyonNew.FamilyId && x.CreateDate >= tarih.tarih1 && x.CreateDate <= tarih.tarih2)
                    .Data;
                var result = _zDonanimCountList.Where(x => x.MashinId == _makine.Id && x.VardiyaId == _vardiya.Id);
                int randomNumberInRange = random.Next(1, 10);
                _istasyonList = _repo.GetRepository<UrIstasyon>().GetAll(x => x.ModulerYapiId == _istasyonNew.ModulerYapiId && x.FamilyId == _istasyonNew.FamilyId).Data;
                _istasyonNew = _istasyonList.FirstOrDefault(x => x.MashinId == _istasyonNew.MashinId && x.VardiyaId == _istasyonNew.VardiyaId);
                TimeSpan timeSpan = (DateTime)result.Last().CreateDate - (DateTime)result.First().CreateDate;
                Double sayiSati = timeSpan.TotalMinutes;
                if (sayiSati > 0)
                {
                    if (_istasyonNew.Realadet > 0)
                    {
                        if (result.Count() / sayiSati / 60 < _istasyonNew.Realadet)
                        {
                            _istasyonNew.Sayi = ((int)_istasyonNew.Realadet * (int)(sayiSati / 60)) + randomNumberInRange;
                        }
                        else
                        {
                            _istasyonNew.Sayi = result.Count();

                        }
                    }
                    else
                    {
                        _istasyonNew.Sayi = result.Count() + (int)_istasyonNew.Sayicarp;
                    }
                }
                _istasyonNew.Orta = GitOrtalamaHesapla((int)_istasyonNew.Sayi, sayiSati);
                _repo.GetRepository<UrIstasyon>().Update(_istasyonNew);

                var modulerYapidakiCountToplam = _istasyonList
                    // .Where(x => x.ModulerYapiId == _istasyonNew.ModulerYapiId && x.FamilyId == _istasyonNew.FamilyId)
                    .Sum(x => x.Sayi);

                var modulerYapidakiCount = _istasyonList
                    .Where(x => x.Id == _istasyonNew.Id)
                    .Sum(x => x.Sayi);

                Vardiya = (int)_istasyonNew.Sayi;
                Toplama = (int)modulerYapidakiCountToplam;// + (int)modulerYapidakiCount;
                Ortalama = (int)_istasyonNew.Orta;
                //Messaglama.MessagYaz($"{Vardiya.ToString()}-{Toplama}-{Ortalama}");
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz(ex.Message);
                // Логика за обработка на грешката, ако е необходимо
                Toplama = 0;
                Ortalama = 0;
                Vardiya = 0;
            }
        }

        //  9 iskarvam 20 min
        // 12 izkarvam 25 obshto 45
        // 14 iskarvan 15 obshto 60 min
        private decimal? GitOrtalamaHesapla(int vardiya, Double totalMinute)
        {
            try
            {

                double sondurumce;
                switch (totalMinute)
                {
                    case 0 | 60 | 120:
                        return vardiya / Convert.ToDecimal(totalMinute);
                    case 240 | 300 | 360:
                        sondurumce = (totalMinute - 45) / 60;
                        return vardiya / Convert.ToDecimal(sondurumce.ToString());
                    case 420 | 480 | 540 | 600 | 660 | 720:
                        sondurumce = (totalMinute - 60) / 60;
                        return vardiya / Convert.ToDecimal(sondurumce.ToString());
                    case > 120:
                        sondurumce = (totalMinute - 20) / 60;
                        return vardiya / Convert.ToDecimal(sondurumce.ToString());
                    default:
                        sondurumce = (totalMinute - 60) / 60;
                        return vardiya / Convert.ToDecimal(sondurumce.ToString());
                }
            }
            catch (Exception)
            {
                return 0m;
            }
        }
    }
}
