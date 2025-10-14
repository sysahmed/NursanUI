using Nursan.Business.Manager;
using Nursan.Core.Printing;
using Nursan.Domain.AmbarModels;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Personal.Valadation;
using Nursan.Validations.Opsionlar;
using Nursan.Validations.SortedList;
using System.Text.RegularExpressions;

namespace Nursan.Validations.ValidationCode
{
    public partial class TorkService : ValidationCode
    {
        private IUnitOfWork _repo;
        public UrVardiya vardiya;
        public IzGenerateId izGeneraciq;
        public List<IzDonanimCount> _donanimServiceEnumerable;
        public OrHarnessModel _orHarnessModel;
        public OrHarnessConfig _orHarnessConfig;
        public IzDonanimCount _donanimCount;
        public UrKonveyorNumara _konveyorNumara;
        private IzCoaxCableCount izCoaxCableCount;
        private List<IzCoaxCableCount> ListizCoaxCableCount;
        private UretimOtomasyonContext _context;
        private MasaPanoValidasyonlari _MasaTara;
        private DonanimService _donanimService;
        private List<SyBarcodeInCrossIstasyon> lisCrossIstasyonBarcode;
        private List<SyBarcodeInput> syBarcodeInput;
        private SyBarcodeOut barcodeOut;
        private UrFabrika fabrika;
        public Result result;
        public OpMashin makine;
        public UrIstasyon istayon;
        public OrFamily family;
        public IEnumerable<UrModulerYapi> modulerList;
        private ToplamV769Services toplamV769Services;
        private string _gelenTorkPFBSerial;
        private int sayisal, sayitakip;
        private List<UrPersonalTakib> personalTakip;
        private Domain.Personal.Personal personal;
        private OzelReferansControlEt ozel;
        //IstasyonValidations istasyonValidasyon = new IstasyonValidations();
        private PersonalValidasyonu perosnalTakibValidasyonu;

        private string vardiyaTPersonal;

        public TorkService(UnitOfWork repo, UrVardiya vardiya) : base(repo)
        {
            _repo = repo;
            makine = GetMakine();
            vardiya = GetVardiya(vardiya.Name);
            istayon = GetIstasyon();
            family = GetFamyly();
            modulerList = GetModulerYapi();
            _orHarnessConfig = new OrHarnessConfig();
            _orHarnessModel = new OrHarnessModel();
            _donanimCount = new IzDonanimCount();
            _donanimService = new DonanimService(repo);
            _donanimServiceEnumerable = new();
            izGeneraciq = new();
            _MasaTara = new(repo);
            _konveyorNumara = new();
            lisCrossIstasyonBarcode = new();
            syBarcodeInput = new List<SyBarcodeInput>();
            barcodeOut = new SyBarcodeOut();
            _context = new();
            toplamV769Services = new ToplamV769Services(_repo);
            perosnalTakibValidasyonu = new PersonalValidasyonu(new UnitOfWorPersonal(new PersonalContext()), _repo);
            fabrika = _repo.GetRepository<UrFabrika>().Get(x => x.Id == istayon.FabrikaId).Data;
            ozel = new OzelReferansControlEt(repo);
        }

        private Result BarcodeDegerle(SyBarcodeInput _bar)
        {
            try
            {
                sayitakip++;
                if (_bar.OzelChar != "")
                {
                    UpdateBarcodeIcerik(_bar);
                }
                var methodDictionary = new Dictionary<string, Func<SyBarcodeInput, Result>>
                    {
                       { "Masa", MasaGitBak },
                       { "First", GetFirstBarcodeBak },
                       { "PFBRef", GitBarcodePFBRefDenetle },
                       { "PFBRefSerial", GitBarcodePFBRefSerialBak },
                       { "Final", syBarcodeInput.Count > 1 ? GitFinalBarcodeBak : GetFirstBarcodeBak },
                       { "FinalGK", syBarcodeInput.Count > 1 ? GitFinalBarcodeBak : GetFirstBarcodeBak },
                       { "Tork", GitBarcodeTork},
                       { "AntenKablo", GitBarcodeAntenKablosu },
                       { "Sicil", SicilBakDegerle },
                    };

                return methodDictionary.TryGetValue(_bar.Name, out Func<SyBarcodeInput, Result> method)
                    ? method(_bar)
                    : new Result(true, "\"Hata Olustu!\"");
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }

        }

        private Result SicilBakDegerle(SyBarcodeInput input)
        {
            var result = perosnalTakibValidasyonu.GetPersonal(input.BarcodeIcerik);
            if (result != null)
            {
                personal = result.Data;
                return new DataResult<Domain.Personal.Personal>(result.Data, true, "Sicil Olutunuz!");
            }
            return new Result(false);
        }
        public Result GetElTestDonanimBarcode(string[] barcode)
        {
            // Проверка за валидност на входните данни
            if (barcode == null || barcode.Length < 2)
            {
                return new Result(false, "Invalid barcode array.");
            }

            try
            {
                // Получаване на броя на баркодите
                var barcodeCount = GitBarcodeVeIstayonBak();

                // Комбиниране на баркодовете
                var combinedBarcode = $"{barcode[0]}{barcode[1]}";

                // Условна логика с минимизирани извиквания
                return barcodeCount?.Count() == 0 ? ConfigCek(combinedBarcode) : ProcessBarcodes(combinedBarcode);
            }
            catch (ErrorExceptionHandller ex)
            {
                return new Result(false, ex.Message);
            }
        }

        private Result ProcessBarcodes(string combinedBarcode)
        {
            GetInputBarcode(combinedBarcode);
            GetTorkDonanimBarcode(syBarcodeInput);

            return result; // Върни резултата от последната операция
        }

        private SuccessDataResults<IEnumerable<SyBarcodeInput>> GetInputBarcode(string bar)
        {
            try
            {
                foreach (var item in lisCrossIstasyonBarcode)
                {
                    var veri = _repo.GetRepository<SyBarcodeInput>().Get(c => c.Id == item.SysBarcodeInId).Data;
                    veri.BarcodeIcerik = bar;
                    syBarcodeInput.Add(veri);
                }
                return new SuccessDataResults<IEnumerable<SyBarcodeInput>>(syBarcodeInput, "System Dogrulandi");
            }
            catch (ErrorExceptionHandller ex)
            {
                return new SuccessDataResults<IEnumerable<SyBarcodeInput>>(ex.Message);
            }
        }

        private IEnumerable<SyBarcodeInCrossIstasyon> GitBarcodeVeIstayonBak()
        {
            try
            {
                lisCrossIstasyonBarcode = _repo.GetRepository<SyBarcodeInCrossIstasyon>().GetAll(x => x.UrIstasyonId == istayon.Id).Data;
                return lisCrossIstasyonBarcode;
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }
        }
        public Result GetTorkDonanimBarcode(List<SyBarcodeInput> barcode)
        {
            sayitakip = 0;
            syBarcodeInput = barcode;

            foreach (var item in barcode)
            {
                try
                {

                    result = BarcodeDegerle(item);

                    if (!result.Success)
                    {
                        return result;
                    }

                    switch (result.Message)
                    {
                        case "SeriNo Okundu!":
                        case "OK":
                            if (istayon.ModulerYapiId == 1)
                            {
                                sayitakip = 0;
                                return new Result(true, $"{_orHarnessConfig.ConfigTork}{_donanimCount.IdDonanim}");
                            }
                            else
                            {
                                if (barcode.Count > 1)
                                {
                                    break;
                                }
                                else
                                {
                                    return new Result(true, "OK");
                                }

                            }

                        case "Donanim Yazdirildi!":
                            return new Result(true, $"Donanim Sisteme Kayit Oldu! {_donanimCount.IdDonanim}");

                        case "Coax Kablo Tamam!":
                            var sysMejdinenBarcod = syBarcodeInput.Find(x => x.Name == "First");
                            return GitICoaxCountTabloUpdate(_donanimCount.IdDonanim, sysMejdinenBarcod);

                        case "Taktiginiz Kutu Dogru!":
                        case "Pano Okuttunuz!":
                        case "Kutu Okuttunuz!":
                        case "Donanim Cektiniz!":
                        case "NOK":
                        case "Donanim Tork Gecmis!":
                        case "Sicil Olutunuz!":
                            continue;

                        case "Coax Kablo Uyusmuyor!":
                            return result;

                        default:
                            return result; // Връщаме резултата за неочаквани съобщения
                    }
                }
                catch (ErrorExceptionHandller ex) // Хващане на всички изключения
                {
                    return new Result(false, $"Yanlis Barcode Okuttunuz!{ex.Message}");
                }
            }

            // Ако не се намери подходящ резултат
            return new Result(false, "No valid barcode processed.");
        }
        private Result GitICoaxCountTabloUpdate(int? barcodeIcerik, SyBarcodeInput? sysMejdinenBarcod)
        {
            try
            {
                ListizCoaxCableCount = _repo.GetRepository<IzCoaxCableCount>().GetAll(x => x.CoaxTutulanId == izCoaxCableCount.CoaxTutulanId && x.DonanimRederansId == null).Data;
                if (ListizCoaxCableCount.Count is not 0)
                {
                    foreach (var item in ListizCoaxCableCount)
                    {
                        item.DonanimRederansId = barcodeIcerik;
                        _repo.GetRepository<IzCoaxCableCount>().Update(item);
                    }
                    var veriler = GitSytemdeBirinciVeYaSonBak(sysMejdinenBarcod);
                    return new Result(true, $"Donannim Sisteme Kayit Oldu! {barcodeIcerik}");
                }
                else
                {
                    return new Result(false, $"Coax Cablo ID Okutulmus! {barcodeIcerik}");
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                return new Result(false, $"Hata Olstu! {barcodeIcerik}");
            }
        }

        private Result UpdateBarcodeIcerik(SyBarcodeInput _bar)
        {
            try
            {
                if (_bar.BarcodeIcerik.StartsWith(_bar.OzelChar))
                {
                    _bar.BarcodeIcerik = _bar.BarcodeIcerik.Remove(0, 1);
                }
                else if (_bar.BarcodeIcerik.EndsWith(_bar.OzelChar) || _bar.BarcodeIcerik.Contains(_bar.OzelChar))
                {
                    string[] parca = _bar.BarcodeIcerik.Split(_bar.OzelChar);
                    _bar.BarcodeIcerik = parca[0];
                }
                return result;
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }
        }

        private Result GitBarcodePFBRefBak()
        {
            try
            {
                _orHarnessConfig = _repo.GetRepository<OrHarnessConfig>().Get(x => x.OrHarnessModelId == _orHarnessModel.Id).Data;
                if (_orHarnessConfig == null)
                {
                    return new DataResult<OrHarnessConfig>(_orHarnessConfig, false, "Boyle Bir Kutu Tanitik Degil!");
                }
                return new DataResult<OrHarnessConfig>(_orHarnessConfig, true, "Kutu Okuttunuz!");
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }
        }

        private Result GitBarcodeAntenKablosu(SyBarcodeInput arg)
        {
            long? gelenVeri = long.Parse(arg.BarcodeIcerik);
            izCoaxCableCount = _repo.GetRepository<IzCoaxCableCount>().Get(x => x.CoaxTutulanId == gelenVeri).Data;
            var result1 = _repo.GetRepository<OrHarnessModel>().Get(x => x.Id == izCoaxCableCount.HarnessModelId).Data;

            if (result1.HarnessModelName == _orHarnessModel.HarnessModelName)
                return new Result(true, "Coax Kablo Tamam!");
            return new Result(false, "Coax Kablo Uyusmuyor!");
        }

        private Result GitFinalBarcodeBak(SyBarcodeInput arg)
        {
            try
            {
                foreach (var item in syBarcodeInput)
                {
                    if (item.BarcodeIcerik.Contains(arg.BarcodeIcerik)) { sayisal++; }
                    else { return new Result(false, "Okutugunuz Barcode Etiketler Uyusmuyor!"); }
                }
                if (sayisal == syBarcodeInput.Count)
                {
                    sayisal = 0;
                    return GitSytemdeBirinciVeYaSonBak(arg);
                }
                return new Result(false, "Hata");
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }
        }

        private Result GitBarcodePFBRefDenetle(SyBarcodeInput arg)
        {
            try
            {
                if (arg.BarcodeIcerik == _orHarnessConfig.PFBSocket)
                {
                    return new Result(true, "Taktiginiz Kutu Dogru!");
                }
                return new Result(false, "Taktiginiz Kutu Yanlis!");
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Torq Barcode Bak
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Result GitBarcodeTork(SyBarcodeInput arg)
        {
            try
            {
                var tork = GenerateIdBak(arg);
                if (tork.Data != null)
                    return new Result(true, "Donanim Tork Gecmis!");
                return new Result(false, "Donanim Totk Gecmemis!");
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }
        }

        private Result GitBarcodePFBRefSerialBak(SyBarcodeInput bar)
        {
            try
            {
                _gelenTorkPFBSerial = bar.BarcodeIcerik;
                return new Result(true, "SeriNo Okundu!");
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }
        }
        private Result GetFirstBarcodeBak(SyBarcodeInput bar)
        {
            var idGenerateResult = GenerateIdBak(bar);
            if (!idGenerateResult.Success)
            {
                return idGenerateResult;
            }

            if (idGenerateResult.Data == null)
            {
                return new Result(false, "Boyle Bir Donanim Yok!");
            }

            var harnessResult = GitHarnesBak(bar);
            if (harnessResult.Data == null)
            {
                return new Result(false, "Boyle Bir Referans Yok!");
            }

            var etap = modulerList.FirstOrDefault(x => x.Id == istayon.ModulerYapiId)?.Etap.ToUpper() ?? "";

            var okunmusResult = GitBakOkunukmu(bar);
            if (!okunmusResult.Success)
            {
                if (etap == EtapConstants.ELTEST || etap == EtapConstants.KLIPTEST || etap == EtapConstants.GROMET)
                {
                    GitElTestBarcodeBas(bar);
                    return new Result(false, "Donanim Okunmus!"); // Завършваме тук, няма нужда да спираме Stopwatch.
                }
                else if (sayitakip == syBarcodeInput.Count)
                {
                    sayitakip = 0;
                    return new Result(false, "Donanim Okunmus!");
                }
            }

            var geldi = GitSytemeSayiBac(bar);
            if (geldi.Message != "OK")
            {
                return new Result(false, geldi.Message);
            }
            else if (geldi.Message != "Donanim Okunmus!")
            {
                HandleErrorMessage(etap, bar);
            }
            return result; // Тук е необходимо да добавите логика за какво точно трябва да се върне result.
        }
        private void HandleErrorMessage(string etap, SyBarcodeInput bar)
        {
            var gelenbarkod = GitHarnesBak(bar);

            switch (etap)
            {
                case EtapConstants.TORK:
                    HandleTorkEtap();
                    break;

                case EtapConstants.KONVEYOR:
                    GitDegerleHerseySToplamV769(_donanimCount.IdDonanim.ToString());
                    sayitakip = 0;
                    if (family.FamilyName != "14401")
                    {
                        GitSytemdeBirinciVeYaSonBak(bar);
                    }
                    break;
                case EtapConstants.GOZKONTROL:
                case EtapConstants.PAKET:
                    sayitakip = 0;
                    GitDegerleHerseySToplamV769(_donanimCount.IdDonanim.ToString());
                    GitSytemdeBirinciVeYaSonBak(bar);
                    break;

                case EtapConstants.KLIPTEST:
                case EtapConstants.ELTEST:
                    GitDegerleHerseySToplamV769(_donanimCount.IdDonanim.ToString());
                    GitElTestBarcodeBas(bar);
                    break;

                case EtapConstants.GROMET:
                    GitDegerleHerseySToplamV769(_donanimCount.IdDonanim.ToString());
                    GitElTestBarcodeBas(bar);
                    break;

                default:
                    break;
            }

        }

        private void HandleTorkEtap()
        {
            GitDegerleHerseySToplamV769(_donanimCount.IdDonanim.ToString());
            var harnessConfigResult = GitBarcodePFBRefBak();
            if (!harnessConfigResult.Success)
            {
                // Handle error or return resultBuyuk
            }
        }
        /// BUrda dopilnitelno First Sigorta Ve Last Paketleme ya da son istasyon </summary> <param
        /// name="FirstAndLast"></param> <returns></returns>
        private Result GitSytemdeBirinciVeYaSonBak(SyBarcodeInput bar)
        {
            //CustomSortedList<int, string> list = new();
            try
            {
                if (GenerateIdBak(bar).Success)
                {
                    var instance = GetByIstasyon().Where(x => x.Toplam != null).GroupBy(x => x.Toplam).OrderBy(x => x.Key);
                    var donanimlar = GetByOkunan(bar.BarcodeIcerik);
                    var start = instance.First().Key;
                    var stop = instance.Last().Key;
                    var fgt = AddDonanimCount(bar, izGeneraciq);
                    // list.Add(instance.Key, instance.First().);
                    if (instance.First().Key == istayon.Toplam)
                    {
                        AddIzGeneraciq(izGeneraciq);
                    }
                    if (instance.Last().Key == istayon.Toplam)
                    {
                        if (PaketlemeBak(fgt.Data.DonanimReferans))
                        {
                            AddDPaketlemeId(fgt.Data);
                        }
                    }
                    return new Result(true, "Donanim Yazdirildi!");
                }
                else
                {
                    return GenerateIdBak(bar);
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }
        }

        private SuccessDataResults<IzGenerateId> AddIzGeneraciq(IzGenerateId izGeneraciq)
        {
            try
            {
                var result = _repo.GetRepository<IzGenerateId>().Add(izGeneraciq);
                _repo.SaveChanges();
                return new SuccessDataResults<IzGenerateId>(result.Data, result.Message);
            }
            catch (ErrorExceptionHandller ex)
            {
                return null;
            }
        }
        public Result AddDPaketlemeId(IzDonanimCount izDonanimCount)
        {
            IzPaketCount izPacketCount = new IzPaketCount();
            if (PaketlemeBak($"{izDonanimCount.DonanimReferans}"))
            {
                izPacketCount.DonanimReferans = izDonanimCount.DonanimReferans;
                izPacketCount.AlertNumber = izDonanimCount.AlertNumber;
                izPacketCount.VardiyaId = izDonanimCount.VardiyaId;
                izPacketCount.MachinId = izDonanimCount.MashinId;
                izPacketCount.Id = 0;
                izPacketCount.UrIstasyonId = izDonanimCount.UrIstasyonId;
                izPacketCount.CreateDate = TarihHesapla.GetSystemDate();
                izPacketCount.UpdateDate = TarihHesapla.GetSystemDate();

                izDonanimCount.UrIstasyonId = istayon.Id;
                try
                {
                    var result = _repo.GetRepository<IzPaketCount>().Add(izPacketCount).Success;
                    return new Result(true, izPacketCount.DonanimReferans + "Yazdirildi");
                }
                catch (ErrorExceptionHandller ex)
                {
                    return new Result(false, izPacketCount.DonanimReferans + "Yazdirilamadi Hata!");
                }
            }
            else
            {
                return new Result(true, izPacketCount.DonanimReferans + "Yazdirildi");
            }
        }
        /// <summary>
        /// tuk trqbva da se naprawi speciala systema za da
        /// </summary>
        /// <param name="GitBakOkunukmu"></param>
        /// <returns></returns>
        private Result GitBakOkunukmu(SyBarcodeInput bar)
        {
            try
            {
                var res = StringSpanConverter.SplitWithoutAllocationReturnArray(bar.BarcodeIcerik.AsSpan(), bar.ParcalamaChar.Value);
                var idres = StringSpanConverter.GetCharsIsDigit(res[2]);
                var resultants = _repo.GetRepository<IzDonanimCount>()
                    .GetAll(x => x.IdDonanim == idres);
                var mYapiResulr = _repo.GetRepository<UrIstasyon>().GetAll(x => x.ModulerYapiId == istayon.ModulerYapiId);
                var intro = mYapiResulr.Data.Where(x => x.ModulerYapiId == istayon.ModulerYapiId && x.FamilyId == family.Id);
                foreach (var j in resultants.Data)
                {
                    foreach (var i in intro)
                    {
                        if (i.Id == j.UrIstasyonId)
                        {
                            return new Result(false);
                        }
                        else
                        {
                            result = new Result(true);
                        }
                    }
                }
                return result;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return new Result(false);
            }
        }
        public Result GitSytemeSayiElTestBack(SyBarcodeInput barkode)
        {
            try
            {
                var res = StringSpanConverter.SplitWithoutAllocationReturnArray(barkode.BarcodeIcerik.AsSpan(), '-');
                var idres = StringSpanConverter.GetCharsIsDigit(res[2]);
                var resultants = _repo.GetRepository<IzDonanimCount>().Get(x => x.IdDonanim == idres & x.UrIstasyonId == istayon.Id);
                var gelenreg = _repo.GetRepository<UrIstasyon>().GetAll(x => x.Toplam ==
                istayon.Toplam && x.FamilyId == istayon.FamilyId);
                foreach (var item in gelenreg.Data)
                {
                    if (resultants.Data.UrIstasyonId == item.Id && resultants.Data.Activ == true)
                    {
                        resultants.Data.Activ = true;
                        resultants.Data.UpdateDate = TarihHesapla.GetSystemDate();
                        _repo.GetRepository<IzDonanimCount>().Update(resultants.Data);
                        return new Result(true, "OK");
                    }
                    else
                    {
                        return new Result(false, "Donanimi Bi oceki Istasyona Yonlendirin!");
                    }
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return new Result(false, ex.Message);
            }
            return new Result(true, "OK");
        }
        public Result GitSytemeSayiBac(SyBarcodeInput barkode)
        {
            try
            {
                var instance = GetByIstasyonNew();
                var donanimlar = GetByOkunan(barkode.BarcodeIcerik);
                var modulerYapu = instance.Where(x => x.ModulerYapiId == istayon.ModulerYapiId).OrderBy(x => x.Toplam);
                var gelenModuleraYapi = modulerList.Where(x => x.Id == istayon.ModulerYapiId);
                var retrult = gelenModuleraYapi.Select(x => x.Etap).First();
                foreach (var item in modulerYapu)
                {
                    var d0namin = donanimlar.Where(x => x.UrIstasyonId == item.Id);
                    bool verce = d0namin.Count() > 0;

                    if (verce)
                    {
                        if (retrult != "KlipTest" & retrult != "ElTest")
                        {
                            return result = new Result(false, "Donanim Okunmus!");
                        }
                        else
                        {
                            var barcodece = _repo.GetRepository<SyBarcodeOut>().Get(x => x.Id == istayon.SyBarcodeOutId);
                            barcodece.Data.BarcodeIcerik = barkode.BarcodeIcerik;
                            var gelenBarcode = GitYaziciDegiskenParcalama(barcodece.Data);
                            var printer = _repo.GetRepository<SyPrinter>().Get(x => x.Id == barcodece.Data.PrinetrId);
                        }
                    }
                    else
                    {
                        result = new Result(true, "OK");
                    }
                }
                // GitBakbarcodeBasaymi(izGeneraciq, _donanimCount, gelenBarcode, printer.Data);
                //Burada system degisecek Harnes 060 referasnina bakacak
                var modulerYapu2 = instance.OrderBy(x => x.Toplam);
                var modulerYapi1 = modulerYapu2.Where(x => x.Toplam == istayon.Toplam - 1);
                foreach (var item in modulerYapi1)
                {
                    var donanim1 = donanimlar.Where(x => x.UrIstasyonId == item.Id);

                    if (donanim1.Count() > 0)
                    {
                        return result = new Result(true, "OK");
                    }
                    else
                    {
                        result = new Result(false, "Donanimi Bi oceki Istasyona Yonlendirin!");
                    }
                }
                return result;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }
        private SuccessDataResults<OrHarnessModel> GitHarnesBak(SyBarcodeInput bar)
        {
            try
            {
                ReadOnlySpan<char> spanDegisken = bar.BarcodeIcerik.AsSpan();
                string[] parcala = StringSpanConverter.SplitWithoutAllocationReturnArray(bar.BarcodeIcerik.AsSpan(), bar.ParcalamaChar.Value);
                string suffix = Regex.Replace(parcala[2], bar.RegexString, "");
                _donanimCount.DonanimReferans = bar.BarcodeIcerik;
                _donanimCount.IdDonanim = StringSpanConverter.GetCharsIsDigitPadingLeft(parcala[2].AsSpan(), (int)bar.PadLeft);
                _donanimCount.VardiyaId = vardiya.Id;
                _donanimCount.MashinId = makine.Id;
                _donanimCount.UrIstasyonId = istayon.Id;
                string veri = $"{parcala[0]}-{parcala[1]}-{suffix}";
                _orHarnessModel = _repo.GetRepository<OrHarnessModel>().Get(x => x.HarnessModelName == veri).Data;
                _donanimCount.OrHarnessModel = _orHarnessModel.HarnessModelName;
                _donanimCount.AlertNumber = _orHarnessModel.AlertNumber;
                return new SuccessDataResults<OrHarnessModel>(_orHarnessModel, "Harness Cektiniz!");
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }
        public DataResult<IzGenerateId> GenerateIdBak(SyBarcodeInput barkode)
        {
            try
            {
                var res = StringSpanConverter.SplitWithoutAllocationReturnArray(barkode.BarcodeIcerik.AsSpan(), barkode.ParcalamaChar.Value);
                var idres = StringSpanConverter.GetCharsIsDigit(res[2]);
                izGeneraciq = _repo.GetRepository<IzGenerateId>()
                    .Get(x => x.Id == idres)
                    .Data;
                var donanim = StringSpanConverter.GetCharsIsDigitPadingLeft(barkode.BarcodeIcerik.AsSpan(), (int)barkode.PadLeft);

                if (izGeneraciq.Revork != true)
                {
                    return new DataResult<IzGenerateId>(izGeneraciq, true, Message.DoanimIDOkutunuz);
                }
                else
                {
                    return new DataResult<IzGenerateId>
                                (izGeneraciq, false, Message.DoanimIDRevorkta);
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return new DataResult<IzGenerateId>(izGeneraciq, true, Message.DoanimIDOkutunuz);
            }

        }

        //029559415 INFORMCIONNA SISTEMA ZA OTPADACI
        public SuccessDataResults<IzToplamV769> GitDegerleHerseySToplamV769IsBaypass(string barcode)
        {
            try
            {
                vardiyaTPersonal = string.Empty;
                var istasyonListce = GetByIstasyon();
                if (perosnalTakibValidasyonu.GetPersonalTakib(istayon).Data != null)
                {
                    personalTakip = perosnalTakibValidasyonu.GetPersonalTakib(istayon).Data;
                    foreach (var item in personalTakip)
                    {
                        vardiyaTPersonal += $"*{item.Sicil}";
                    }
                }
                var izToplam = _repo.GetRepository<IzToplamV769>().Get(x => x.IdDonanim == int.Parse(barcode).ToString()).Data;
                DateTime tarih = TarihHesaplama.GetSystemDate();
                var istas = modulerList.FirstOrDefault(x => x.Id == istayon.Toplam);
                var istasNewPaketVarmi = istasyonListce.FirstOrDefault(x => x.Id > istayon.ModulerYapiId);
                var result = modulerList.FirstOrDefault(x => x.Id > istayon.Toplam);
                string barkodAndFull = $"{izToplam.Referans}{barcode.PadLeft(8, '0')}";
                if (personal != null)
                {
                    vardiyaTPersonal = vardiyaTPersonal == null ? "*" + personal.USER_CODE : vardiyaTPersonal;
                }
                string vardiyaFull = $"{Environment.MachineName}{vardiya.Name}{vardiyaTPersonal}";
                if (izToplam != null)
                {
                    switch (istas.Etap)
                    {
                        case "Konveyor":
                            izToplam.Konveyor = vardiyaFull;
                            izToplam.Konvar = vardiyaFull;
                            izToplam.Kondata = tarih;
                            izToplam.Konveyorb = false;
                            izToplam.Konveyorgec = true;
                            if (istas.Etap == "KlipTest")
                            {
                                izToplam.Kliptestb = true;
                            }
                            else if (istas.Etap == "ElTest")
                            {
                                izToplam.Eltestb = true;
                            }
                            break;
                        //return true;
                        case "KlipTest":
                            izToplam.Kliptest = vardiyaFull;
                            izToplam.Klipvar = vardiyaFull;
                            izToplam.Kliptestdata = tarih;
                            izToplam.Kliptestb = false;
                            izToplam.Klipgec = true;
                            izToplam.Eltestb = true;
                            break;
                        // return true;
                        case "ElTest":
                            if (istasNewPaketVarmi != null)
                            {
                                izToplam.Eltest = vardiyaFull;
                                izToplam.Elvar = vardiyaFull;
                                izToplam.Eltestdata = tarih;
                                izToplam.Eltestb = false;
                                izToplam.Eltestgec = true;
                                if (modulerList.FirstOrDefault(x => x.Id == result.Id).Etap == "GozKontrol")
                                {
                                    izToplam.Gozb = true;
                                }
                                else if (modulerList.FirstOrDefault(x => x.Id == result.Id).Etap == "Paket")
                                {
                                    izToplam.Paketlemeb = true;
                                }
                                else
                                {
                                    izToplam.Paketleme = vardiyaFull;
                                    izToplam.Paketvar = vardiyaFull;
                                    izToplam.Paketdata = tarih;
                                    izToplam.Paketlemeb = false;
                                    izToplam.Paketgec = true;
                                    izToplam.Kolib = true;
                                    if (PaketlemeBak($"{izToplam.Referans}{barcode}"))
                                        AddDPaketlemeId(new IzDonanimCount
                                        {
                                            DonanimReferans = $"{izToplam.Referans}{barcode}",
                                            AlertNumber = izToplam.Alert,
                                            UrIstasyonId = istayon.Id,
                                            MashinId = istayon.MashinId,
                                            VardiyaId = vardiya.Id,
                                            CreateDate = tarih,
                                            UpdateDate = tarih
                                        });
                                }
                            }
                            else
                            {
                                izToplam.Eltest = vardiyaFull;
                                izToplam.Elvar = vardiyaFull;
                                izToplam.Eltestdata = tarih;
                                izToplam.Eltestb = false;
                                izToplam.Eltestgec = true;
                                izToplam.Paketleme = vardiyaFull;
                                izToplam.Paketvar = vardiyaFull;
                                izToplam.Paketdata = tarih;
                                izToplam.Paketlemeb = false;
                                izToplam.Paketgec = true;
                                izToplam.Kolib = true;
                                if (PaketlemeBak($"{izToplam.Referans}{barcode}"))
                                    AddDPaketlemeId(new IzDonanimCount
                                    {
                                        DonanimReferans = $"{izToplam.Referans}{barcode}",
                                        AlertNumber = izToplam.Alert,
                                        UrIstasyonId = istayon.Id,
                                        MashinId = istayon.MashinId,
                                        VardiyaId = vardiya.Id,
                                        CreateDate = tarih,
                                        UpdateDate = tarih
                                    });
                            }
                            break;
                        // return true;
                        case "GozKontrol":
                            izToplam.Goz = vardiyaFull;
                            izToplam.Gozvar = vardiyaFull;
                            izToplam.Gozdata = tarih;
                            izToplam.Gozb = false;
                            izToplam.Gozgec = true;
                            if (modulerList.FirstOrDefault(x => x.Id == result.Id).Etap == "Tork")
                            {
                                izToplam.Torkb = true;
                            }
                            else if (modulerList.FirstOrDefault(x => x.Id == result.Id).Etap == "Paket")
                            {
                                izToplam.Paketlemeb = true;
                            }
                            break;

                        case "Tork":
                            izToplam.Tork = vardiyaFull;
                            izToplam.Torkvar = vardiyaFull;
                            izToplam.Torkdate = tarih;
                            izToplam.Torkb = false;
                            izToplam.Torkgec = true;
                            izToplam.Paketlemeb = true;
                            break;
                        // return true;
                        case "Paket":
                            izToplam.Paketleme = vardiyaFull;
                            izToplam.Paketvar = vardiyaFull;
                            izToplam.Paketdata = tarih;
                            izToplam.Paketlemeb = false;
                            izToplam.Paketgec = true;
                            izToplam.Kolib = true;

                            break;
                        // return true;
                        case "AntenKablo":
                        // return true;
                        default: break;
                            //return;
                    }
                    return toplamV769Services.UpdateToplamV769(izToplam);
                }
                else if (izToplam == null && (istas.Etap is "KlipTest" | istas.Etap is "ElTest"))
                {
                    var issona = _repo.GetRepository<IzGenerateId>().Get(x => x.Id == int.Parse(barcode)).Data;
                    Messaglama.MessagYaz($"GitDegerleHerseySToplamV769IsBaypass {issona.UrIstasyonId}");
                    izToplam = new IzToplamV769();
                    var harnes = _repo.GetRepository<OrHarnessModel>().Get(x => x.Id == issona.HarnesModelId).Data;
                    izToplam.IdDonanim = barcode;
                    izToplam.Eltest = vardiyaFull;
                    izToplam.Eltestb = false;
                    izToplam.Eltestgec = true;
                    izToplam.Elvar = vardiyaFull;
                    izToplam.Eltestdata = tarih;
                    izToplam.Referans = harnes.HarnessModelName;
                    izToplam.Paketleme = vardiyaFull;
                    izToplam.Paketvar = vardiyaFull;
                    izToplam.Paketdata = tarih;
                    izToplam.Paketlemeb = false;
                    izToplam.Paketgec = true;
                    izToplam.Alert = (int)harnes.AlertNumber;
                    izToplam.SideCode = harnes.SideCode;
                    izToplam.CustomId = harnes.CustomerID;
                    izToplam.Fabrika = (short)istayon.FabrikaId;
                    izToplam.Kolib = true;
                    _repo.GetRepository<IzToplamV769>().Add(izToplam);
                    Messaglama.MessagYaz($"GitDegerleHerseySToplamV769IsBaypass {izToplam.Referans}");
                    if (PaketlemeBak(barkodAndFull))
                        AddDPaketlemeId(new IzDonanimCount
                        {
                            DonanimReferans = barkodAndFull,
                            AlertNumber = izToplam.Alert,
                            UrIstasyonId = istayon.Id,
                            MashinId = istayon.MashinId,
                            VardiyaId = vardiya.Id,
                            CreateDate = tarih,
                            UpdateDate = tarih
                        });

                    return new SuccessDataResults<IzToplamV769>(izToplam, "DoanimSystemeYazdirildi");
                }
                else
                {
                    return new SuccessDataResults<IzToplamV769>(null, "Donanim Toplamda Yok!!!");
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return new SuccessDataResults<IzToplamV769>(null, "Hata Systemde Bulunamadi!");
            }
        }
        public SuccessDataResults<IzToplamV769> GitDegerleHerseySToplamV769Gromet(string barcode, string sicil)
        {
            try
            {
                // Валидиране на данни
                var istasyonList = GetByIstasyon();
                var personalTakip = perosnalTakibValidasyonu.GetPersonalTakib(istayon)?.Data;
                string vardiyaPersonal = personalTakip?.FirstOrDefault(x => x.Sicil == sicil)?.Sicil ?? string.Empty;

                // Изтегляне на информация за "IzToplam"
                var izToplam = _repo.GetRepository<IzToplamV769>().Get(x => x.IdDonanim == barcode).Data;
                DateTime currentDate = TarihHesaplama.GetSystemDate();
                var istas = modulerList.FirstOrDefault(x => x.Id == istayon.ModulerYapiId);

                if (izToplam == null)
                {
                    if (istas.Etap is "KlipTest" or "ElTest")
                    {
                        return HandleNewIzToplam(izToplam, vardiyaPersonal, currentDate, barcode);
                    }
                    return new SuccessDataResults<IzToplamV769>(null, "Donanim Toplamda Yok!!!");
                }

                var istasNewPaketVarmi = FindNextStation(istasyonList, istayon);
                UpdateIzToplamData(izToplam, istas, istasNewPaketVarmi, vardiyaPersonal, currentDate, barcode);

                return toplamV769Services.UpdateToplamV769(izToplam);
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return new SuccessDataResults<IzToplamV769>(null, $"Грешка: {ex.Message}");
            }
        }

        // Намира следващата станция, базирано на текущото състояние
        private UrIstasyon FindNextStation(IEnumerable<UrIstasyon> istasyonList, UrIstasyon istayon)
        {
            return istasyonList
                .Where(x => x.Toplam >= istayon.Toplam)
                .OrderBy(x => x.Toplam)
                .FirstOrDefault();
        }

        // Обработва нов обект IzToplam
        private SuccessDataResults<IzToplamV769> HandleNewIzToplam(IzToplamV769 izToplam, string vardiyaPersonal, DateTime currentDate, string barcode)
        {
            izToplam = new IzToplamV769
            {
                Paketleme = vardiyaPersonal,
                Paketvar = vardiyaPersonal,
                Paketdata = currentDate,
                Paketlemeb = false,
                Eltestb = false,
                Torkb = false,
                Gozb = false,
                Paketgec = true,
                Kolib = true
            };

            string barcodeFull = izToplam.Referans + barcode.PadLeft(8, '0');
            if (PaketlemeBak(barcodeFull))
            {
                AddDPaketlemeId(new IzDonanimCount
                {
                    DonanimReferans = barcodeFull,
                    AlertNumber = izToplam.Alert,
                    UrIstasyonId = istayon.Id,
                    MashinId = istayon.MashinId,
                    VardiyaId = vardiya.Id,
                    CreateDate = currentDate,
                    UpdateDate = currentDate
                });
            }

            return new SuccessDataResults<IzToplamV769>(izToplam, "DoanimSystemeYazdirildi");
        }

        // Актуализира данните на IzToplam на база текущ етап
        private void UpdateIzToplamData(IzToplamV769 izToplam, UrModulerYapi? istas, UrIstasyon istasNewPaketVarmi, string vardiyaPersonal, DateTime currentDate, string barcode)
        {
            string fullInfo = $"{Environment.MachineName}{vardiya.Name}{vardiyaPersonal}";
            switch (istas.Etap)
            {
                case "Konveyor":
                    izToplam.Konveyor = fullInfo;
                    izToplam.Kondata = currentDate;
                    izToplam.Konveyorgec = true;
                    UpdateNextStageFlags(izToplam, istasNewPaketVarmi);
                    break;
                case "Gromet":
                    izToplam.Gromet = fullInfo;
                    izToplam.Grometdata = currentDate;
                    izToplam.Grometgec = true;
                    izToplam.Kliptestb = true;
                    break;
                case "KlipTest":
                    izToplam.Kliptest = fullInfo;
                    izToplam.Kliptestdata = currentDate;
                    izToplam.Klipgec = true;
                    izToplam.Eltestb = true;
                    break;
                case "ElTest":
                    UpdateElTestData(izToplam, istasNewPaketVarmi, fullInfo, currentDate, barcode);
                    break;
                case "GozKontrol":
                    izToplam.Goz = fullInfo;
                    izToplam.Gozdata = currentDate;
                    izToplam.Gozgec = true;
                    UpdateNextStageFlags(izToplam, istasNewPaketVarmi);
                    break;
                case "Tork":
                    izToplam.Tork = fullInfo;
                    izToplam.Torkdate = currentDate;
                    izToplam.Torkgec = true;
                    izToplam.Paketlemeb = true;
                    break;
                case "Paket":
                    izToplam.Paketleme = fullInfo;
                    izToplam.Paketdata = currentDate;
                    izToplam.Paketgec = true;
                    izToplam.Kolib = true;
                    if (PaketlemeBak($"{izToplam.Referans}{barcode.PadLeft(8, '0')}"))
                        AddDPaketlemeId(new IzDonanimCount
                        {
                            DonanimReferans = $"{izToplam.Referans}{barcode.PadLeft(8, '0')}",
                            AlertNumber = izToplam.Alert,
                            UrIstasyonId = istayon.Id,
                            MashinId = istayon.MashinId,
                            VardiyaId = vardiya.Id,
                            CreateDate = currentDate,
                            UpdateDate = currentDate
                        });
                    break;
            }
        }

        // Обработва данни за етапа "ElTest"
        private void UpdateElTestData(IzToplamV769 izToplam, UrIstasyon istasNewPaketVarmi, string vardiyaFull, DateTime currentDate, string barcode)
        {
            izToplam.Eltest = vardiyaFull;
            izToplam.Eltestdata = currentDate;
            izToplam.Eltestgec = true;

            if (istasNewPaketVarmi != null)
            {
                UpdateNextStageFlags(izToplam, istasNewPaketVarmi);
            }
            else
            {
                izToplam.Paketleme = vardiyaFull;
                izToplam.Paketdata = currentDate;
                izToplam.Paketgec = true;
                izToplam.Kolib = true;
                if (PaketlemeBak($"{izToplam.Referans}{barcode.PadLeft(8, '0')}"))
                    AddDPaketlemeId(new IzDonanimCount
                    {
                        DonanimReferans = $"{izToplam.Referans}{barcode.PadLeft(8, '0')}",
                        AlertNumber = izToplam.Alert,
                        UrIstasyonId = istayon.Id,
                        MashinId = istayon.MashinId,
                        VardiyaId = vardiya.Id,
                        CreateDate = currentDate,
                        UpdateDate = currentDate
                    });
            }
        }

        // Актуализира флаговете за следващия етап на обработка
        private void UpdateNextStageFlags(IzToplamV769 izToplam, UrIstasyon istasNewPaketVarmi)
        {
            if (istasNewPaketVarmi.ModulerYapi.Etap == "Gromet")
                izToplam.Grometb = true;
            else if (istasNewPaketVarmi.ModulerYapi.Etap == "KlipTest")
                izToplam.Kliptestb = true;
            else if (istasNewPaketVarmi.ModulerYapi.Etap == "ElTest")
                izToplam.Eltestb = true;
        }


        public SuccessDataResults<IzToplamV769> GitDegerleHerseySToplamV769(string barcode)
        {
            UrIstasyon istasNewPaketVarmi;
            try
            {
                vardiyaTPersonal = string.Empty;
                //var istsyon = istasyonValidasyon.GetIstasyons(vardiya.Name);
                var istasyonListce = GetByIstasyon();
                if (perosnalTakibValidasyonu.GetPersonalTakib(istayon).Data != null)
                {
                    personalTakip = perosnalTakibValidasyonu.GetPersonalTakib(istayon).Data;
                    foreach (var item in personalTakip)
                    {
                        vardiyaTPersonal += $"*{item.Sicil}";
                    }
                }
                var izToplam = _repo.GetRepository<IzToplamV769>().Get(x => x.IdDonanim == barcode).Data;
                DateTime tarih = TarihHesaplama.GetSystemDate();
                var istas = modulerList.FirstOrDefault(x => x.Id == istayon.ModulerYapiId);
                try
                {
                    istasNewPaketVarmi = istasyonListce.Where(x => x.Toplam > istayon.Toplam).MinBy(x => x.Toplam);
                }

                catch (ErrorExceptionHandller ex)
                {
                    istasNewPaketVarmi = istasyonListce.Where(x => x.Toplam > istayon.Toplam).MinBy(x => x.Toplam);
                    if (istas.Etap != "Paket")
                    {
                        throw new ErrorExceptionHandller(ex.Message);
                    }
                }
                string barkodAndFull = $"{izToplam.Referans}{barcode.PadLeft(8, '0')}";
                string vardiyaFull = $"{Environment.MachineName}{vardiya.Name}{vardiyaTPersonal}";
                if (izToplam != null)
                {
                    switch (istas.Etap)
                    {
                        case "Konveyor":
                            izToplam.Konveyor = vardiyaFull;
                            izToplam.Konvar = vardiyaFull;
                            izToplam.Kondata = tarih;
                            izToplam.Konveyorb = false;
                            izToplam.Konveyorgec = true;
                            if (modulerList.FirstOrDefault(x => x.Id == istasNewPaketVarmi.ModulerYapiId).Etap == "Gromet")
                            {
                                izToplam.Grometb = true;
                            }
                            else if (modulerList.FirstOrDefault(x => x.Id == istasNewPaketVarmi.ModulerYapiId).Etap == "KlipTest")
                            {
                                izToplam.Kliptestb = true;
                            }
                            else if (modulerList.FirstOrDefault(x => x.Id == istasNewPaketVarmi.ModulerYapiId).Etap == "ElTest")
                            {
                                izToplam.Eltestb = true;
                            }
                            break;

                        case "Gromet":
                            izToplam.Konveyorb = false;
                            izToplam.Grometb = false;
                            izToplam.Grometgec = true;
                            izToplam.Kliptestb = true;
                            izToplam.Gromet = vardiyaFull;
                            izToplam.Grometvar = vardiyaFull;
                            izToplam.Grometdata = tarih;
                            break;
                        //return;
                        case "KlipTest":
                            izToplam.Kliptest = vardiyaFull;
                            izToplam.Klipvar = vardiyaFull;
                            izToplam.Kliptestdata = tarih;
                            izToplam.Konveyorb = false;
                            izToplam.Kliptestb = false;
                            izToplam.Klipgec = true;
                            izToplam.Eltestb = true;
                            break;
                        // return true;
                        case "ElTest":
                            if (istasNewPaketVarmi != null)
                            {
                                izToplam.Konveyorb = false;
                                izToplam.Kliptestb = false;
                                izToplam.Eltestb = false;
                                izToplam.Eltest = vardiyaFull;
                                izToplam.Elvar = vardiyaFull;
                                izToplam.Eltestdata = tarih;
                                izToplam.Eltestgec = true;
                                if (modulerList.FirstOrDefault(x => x.Id == istasNewPaketVarmi.ModulerYapiId).Etap == "GozKontrol")
                                {
                                    izToplam.Gozb = true;
                                }
                                else if (modulerList.FirstOrDefault(x => x.Id == istasNewPaketVarmi.ModulerYapiId).Etap == "Tork")
                                {
                                    izToplam.Torkb = true;
                                }
                                else if (modulerList.FirstOrDefault(x => x.Id == istasNewPaketVarmi.ModulerYapiId).Etap == "Paket")
                                {
                                    izToplam.Paketlemeb = true;
                                }
                                else
                                {
                                    izToplam.Konveyorb = false;
                                    izToplam.Kliptestb = false;
                                    izToplam.Eltestb = false;
                                    izToplam.Gozb = false;
                                    izToplam.Paketleme = vardiyaFull;
                                    izToplam.Paketvar = vardiyaFull;
                                    izToplam.Paketdata = tarih;
                                    izToplam.Paketlemeb = false;
                                    izToplam.Paketgec = true;
                                    izToplam.Kolib = true;
                                    if (PaketlemeBak(barkodAndFull))
                                        AddDPaketlemeId(new IzDonanimCount
                                        {
                                            DonanimReferans = barkodAndFull,
                                            AlertNumber = izToplam.Alert,
                                            UrIstasyonId = istayon.Id,
                                            MashinId = istayon.MashinId,
                                            VardiyaId = vardiya.Id,
                                            CreateDate = tarih,
                                            UpdateDate = tarih
                                        });
                                }
                            }
                            else
                            {
                                izToplam.Konveyorb = false;
                                izToplam.Kliptestb = false;
                                izToplam.Eltestb = false;
                                izToplam.Gozb = false;
                                izToplam.Eltest = vardiyaFull;
                                izToplam.Elvar = vardiyaFull;
                                izToplam.Eltestdata = tarih;
                                izToplam.Eltestgec = true;
                                izToplam.Paketleme = vardiyaFull;
                                izToplam.Paketvar = vardiyaFull;
                                izToplam.Paketdata = tarih;
                                izToplam.Paketlemeb = false;
                                izToplam.Paketgec = true;
                                izToplam.Kolib = true;
                                if (PaketlemeBak(barkodAndFull))
                                    AddDPaketlemeId(new IzDonanimCount
                                    {
                                        DonanimReferans = $"{izToplam.Referans}{barcode.PadLeft(8, '0')}",
                                        AlertNumber = izToplam.Alert,
                                        UrIstasyonId = istayon.Id,
                                        MashinId = istayon.MashinId,
                                        VardiyaId = vardiya.Id,
                                        CreateDate = tarih,
                                        UpdateDate = tarih
                                    });
                            }
                            break;
                        // return true;
                        case "GozKontrol":
                            izToplam.Goz = vardiyaFull;
                            izToplam.Gozvar = vardiyaFull;
                            izToplam.Konveyorb = false;
                            izToplam.Kliptestb = false;
                            izToplam.Eltestb = false;
                            izToplam.Gozb = false;
                            izToplam.Gozdata = tarih;
                            izToplam.Gozgec = true;

                            if (modulerList.FirstOrDefault(x => x.Id == istasNewPaketVarmi.ModulerYapiId).Etap == "Tork")
                            {
                                izToplam.Torkb = true;
                            }
                            else if (modulerList.FirstOrDefault(x => x.Id == istasNewPaketVarmi.ModulerYapiId).Etap == "Paket")
                            {
                                izToplam.Paketlemeb = true;
                            }
                            break;

                        case "Tork":
                            izToplam.Tork = vardiyaFull;
                            izToplam.Torkvar = vardiyaFull;
                            izToplam.Konveyorb = false;
                            izToplam.Kliptestb = false;
                            izToplam.Eltestb = false;
                            izToplam.Gozb = false;
                            izToplam.Torkb = false;
                            izToplam.Torkdate = tarih;
                            izToplam.Torkgec = true;
                            izToplam.Paketlemeb = true;
                            break;
                        // return true;
                        case "Paket":
                            izToplam.Konveyorb = false;
                            izToplam.Kliptestb = false;
                            izToplam.Eltestb = false;
                            izToplam.Gozb = false;
                            izToplam.Torkb = false;
                            izToplam.Paketlemeb = false;
                            izToplam.Paketleme = vardiyaFull;
                            izToplam.Paketvar = vardiyaFull;
                            izToplam.Paketdata = tarih;
                            izToplam.Paketgec = true;
                            izToplam.Kolib = true;
                            if (PaketlemeBak(barkodAndFull))
                                AddDPaketlemeId(new IzDonanimCount
                                {
                                    DonanimReferans = barkodAndFull,
                                    AlertNumber = izToplam.Alert,
                                    UrIstasyonId = istayon.Id,
                                    MashinId = istayon.MashinId,
                                    VardiyaId = vardiya.Id,
                                    CreateDate = tarih,
                                    UpdateDate = tarih
                                });
                            break;
                        // return true;
                        case "AntenKablo":
                            break;
                            // return true;
                    }
                    return toplamV769Services.UpdateToplamV769(izToplam);
                }
                else if (izToplam == null && (istas.Etap is "KlipTest" | istas.Etap is "ElTest"))
                {
                    izToplam = new IzToplamV769();
                    izToplam.Paketleme = vardiyaFull;
                    izToplam.Paketvar = vardiyaFull;
                    izToplam.Paketdata = tarih;
                    izToplam.Paketlemeb = false;
                    izToplam.Eltestb = false;
                    izToplam.Torkb = false;
                    izToplam.Gozb = false;
                    izToplam.Paketgec = true;
                    izToplam.Kolib = true;
                    if (PaketlemeBak(barkodAndFull))
                        AddDPaketlemeId(new IzDonanimCount
                        {
                            DonanimReferans = barkodAndFull,
                            AlertNumber = izToplam.Alert,
                            UrIstasyonId = istayon.Id,
                            MashinId = istayon.MashinId,
                            VardiyaId = vardiya.Id,
                            CreateDate = tarih,
                            UpdateDate = tarih
                        });
                    return new SuccessDataResults<IzToplamV769>(izToplam, "DoanimSystemeYazdirildi");
                }
                else
                {
                    return new SuccessDataResults<IzToplamV769>(null, "Donanim Toplamda Yok!!!");
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return new SuccessDataResults<IzToplamV769>(null, "Hata Systemde Bulunamadi!");
            }
        }

        private bool PaketlemeBak(string v)
        {
            try
            {
                var paketleme = _repo.GetRepository<IzPaketCount>().Get(x => x.DonanimReferans == v);

                if (paketleme.Data != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return false;
            }
        }
        public IEnumerable<UrIstasyon> GetByIstasyonSigorta(int? digortaId)
        {
            try
            {
                return _repo.GetRepository<UrIstasyon>().GetAll(x => x.FamilyId == digortaId && x.ModulerYapiId != null).Data;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        public IEnumerable<UrIstasyon> GetByIstasyon()
        {
            try
            {
                return _repo.GetRepository<UrIstasyon>().GetAll(x => x.FamilyId == family.Id && x.ModulerYapiId != null).Data;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }
        public IEnumerable<UrIstasyon> GetByIstasyonNew()
        {
            try
            {
                return _repo.GetRepository<UrIstasyon>().GetAll(x => x.ModulerYapiId != null).Data;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }
        public UrIstasyon GetIstasyon()
        {
            try
            {
                istayon = _repo.GetRepository<UrIstasyon>().Get(x => x.MashinId == makine.Id & x.VardiyaId == vardiya.Id).Data;
                if (istayon != null)
                {
                    return istayon;
                }
                else
                {
                    Messaglama.MessagException("Vardiya Bu Makine-le Eslesmiyor!");
                    return null;
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        public IEnumerable<IzDonanimCount> GetByOkunan(string? barcodeIcerik)
        {
            try
            {
                var res = StringSpanConverter.SplitWithoutAllocationReturnArray(barcodeIcerik.AsSpan(), '-');
                var idres = StringSpanConverter.GetCharsIsDigit(res[2]);
                return _repo.GetRepository<IzDonanimCount>().GetAll(x => x.IdDonanim == idres).Data;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        public UrVardiya GetVardiya(string vardyaString)
        {
            try
            {
                vardiya = _repo.GetRepository<UrVardiya>().Get(x => x.Name == vardyaString).Data;
                return vardiya;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        public OpMashin GetMakine()
        {
            try
            {
                makine = _repo.GetRepository<OpMashin>().Get(x => x.MasineName == Environment.MachineName).Data;
                if (makine != null)
                    return makine;
                else
                {
                    Messaglama.MessagException("Boyle Bir Makine Sytemde Yok!");
                    return null;
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        public OrFamily GetFamyly()
        {
            try
            {
                family = _repo.GetRepository<OrFamily>().Get(x => x.Id == istayon.FamilyId).Data;
                return family;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        public IEnumerable<UrModulerYapi> GetModulerYapi()
        {
            try
            {
                modulerList = _repo.GetRepository<UrModulerYapi>().GetAll(null).Data;
                return modulerList;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }
        public DataResult<IzDonanimCount> AddDonanimCountSigorta(SyBarcodeInput barcode, IzGenerateId data, UrIstasyon istasyon)
        {
            try
            {
                var barcod = GitYaziciDegiskenParcalama(new SyBarcodeOut
                {
                    BarcodeIcerik = barcode.BarcodeIcerik,
                    RegexInt = barcode.RegexInt,
                    RegexString = barcode.RegexString,
                    PadLeft = barcode.PadLeft
                });

                // Парсваме ID на доналима
                int idDonanim = int.Parse(barcod.IdDonanim);

                // Създаваме нов инстанция на IzDonanimCount
                var donanimCount = CreateDonanimCountSigorta(barcode, data, idDonanim, istasyon);

                // Добавяме доналима към репозитория
                var result = _repo.GetRepository<IzDonanimCount>().Add(donanimCount);

                // Проверка на резултата
                return result.Data != null
                    ? new DataResult<IzDonanimCount>(result.Data, true, "IDDonanim Yazildi")
                    : new DataResult<IzDonanimCount>(null, false, "Donanim ID Yazilamadi");
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                // Връщаме DataResult с информация за грешката
                return new DataResult<IzDonanimCount>(null, false, ex.Message);
            }
        }
        //#R2X6-17K400-AAB_02182058
        //#R2X6-15K867-ACB_02181980

        // Метод за създаване на IzDonanimCount
        private IzDonanimCount CreateDonanimCountSigorta(SyBarcodeInput barcode, IzGenerateId data, int idDonanim, UrIstasyon istasyon)
        {
            return new IzDonanimCount
            {
                DonanimReferans = barcode.BarcodeIcerik,
                IdDonanim = idDonanim,
                OrHarnessModel = _donanimService.GetDonanimHarnesOne((int)data.HarnesModelId)?.HarnessModelName,
                AlertNumber = _donanimService.GetAllAlertBaglanti((int)data.AlertNumber)?.AlertNumber,
                MasaId = _konveyorNumara.Id == 0 ? null : _konveyorNumara.Id,
                VardiyaId = vardiya.Id,
                MashinId = istasyon?.MashinId,
                UrIstasyonId = (data.UrIstasyonId != null) ? istayon.Id : 0,
                CreateDate = TarihHesapla.GetSystemDate(),
                UpdateDate = TarihHesapla.GetSystemDate()
            };
        }
        public DataResult<IzDonanimCount> AddDonanimCount(SyBarcodeInput barcode, IzGenerateId data)
        {
            try
            {
                var barcod = GitYaziciDegiskenParcalama(new SyBarcodeOut
                {
                    BarcodeIcerik = barcode.BarcodeIcerik,
                    RegexInt = barcode.RegexInt,
                    RegexString = barcode.RegexString,
                    PadLeft = barcode.PadLeft
                });

                // Парсваме ID на доналима
                int idDonanim = int.Parse(barcod.IdDonanim);

                // Създаваме нов инстанция на IzDonanimCount
                var donanimCount = CreateDonanimCount(barcode, data, idDonanim);

                // Добавяме доналима към репозитория
                var result = _repo.GetRepository<IzDonanimCount>().Add(donanimCount);

                // Проверка на резултата
                return result.Data != null
                    ? new DataResult<IzDonanimCount>(result.Data, true, "IDDonanim Yazildi")
                    : new DataResult<IzDonanimCount>(null, false, "Donanim ID Yazilamadi");
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                // Връщаме DataResult с информация за грешката
                return new DataResult<IzDonanimCount>(null, false, ex.Message);
            }
        }
        //#R2X6-17K400-AAB_02182058
        //#R2X6-15K867-ACB_02181980

        // Метод за създаване на IzDonanimCount
        private IzDonanimCount CreateDonanimCount(SyBarcodeInput barcode, IzGenerateId data, int idDonanim)
        {
            return new IzDonanimCount
            {
                DonanimReferans = barcode.BarcodeIcerik,
                IdDonanim = idDonanim,
                OrHarnessModel = _donanimService.GetDonanimHarnesOne((int)data.HarnesModelId)?.HarnessModelName,
                AlertNumber = _donanimService.GetAllAlertBaglanti((int)data.AlertNumber)?.AlertNumber,
                MasaId = _konveyorNumara.Id == 0 ? null : _konveyorNumara.Id,
                VardiyaId = vardiya.Id,
                MashinId = istayon?.MashinId,
                UrIstasyonId = (data.UrIstasyonId != null) ? istayon.Id : 0,
                CreateDate = TarihHesapla.GetSystemDate(),
                UpdateDate = TarihHesapla.GetSystemDate()
            };
        }
        public Result GitElTestBarcodeBas(SyBarcodeInput Barcode)
        {
            try
            {
                if (GenerateIdBak(Barcode).Success)
                {
                    if (istayon.SyBarcodeOutId != null)
                    {
                        SysBarcodOutAra(Barcode);
                        var printer = _repo.GetRepository<SyPrinter>().Get(x => x.Id == barcodeOut.PrinetrId);

                        _donanimCount.AlertNumber = izGeneraciq.AlertNumber;
                        _donanimCount.DonanimReferans = izGeneraciq.Barcode;
                        _donanimCount.OrHarnessModel = _orHarnessModel.HarnessModelName;

                        var resultce = GitBakbarcodeBasaymi(izGeneraciq, _donanimCount, barcodeOut, printer.Data);

                        if (resultce.Data != null && GitBakOkunukmu(Barcode).Success)
                        {
                            bool fgt = AddDonanimCount(Barcode, izGeneraciq).Success;
                            if (fgt)
                            {
                                return result;
                            }
                        }
                    }
                    else
                    {
                        result = new Result(true, "Donanim Okundu!");
                    }
                }

                return result;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        private SyBarcodeOut SysBarcodOutAra(SyBarcodeInput Barcode)
        {
            barcodeOut = _repo.GetRepository<SyBarcodeOut>().GetById((int)istayon.SyBarcodeOutId).Data;
            barcodeOut.BarcodeIcerik = Barcode.BarcodeIcerik;
            var gelenBarcode = GitYaziciDegiskenParcalama(barcodeOut);
            return gelenBarcode;
        }

        public Result GitBarcodeBas(IEnumerable<SyBarcodeInput> Barcode)
        {
            try
            {
                if (GenerateIdBak(Barcode.FirstOrDefault()).Success)
                {
                    foreach (var item in Barcode)
                    {
                        bool fgt = AddDonanimCount(item, izGeneraciq).Success;
                        if (fgt == true)
                        {
                            if (istayon.SyBarcodeOutId != null)
                            {
                                var barcodece = _repo.GetRepository<SyBarcodeOut>().Get(x => x.Id == istayon.SyBarcodeOutId);
                                barcodece.Data.BarcodeIcerik = Barcode.FirstOrDefault().BarcodeIcerik;
                                var gelenBarcode = GitYaziciDegiskenParcalama(barcodece.Data);
                                var printer = _repo.GetRepository<SyPrinter>().Get(x => x.Id == barcodece.Data.PrinetrId);
                                return GitBakbarcodeBasaymi(izGeneraciq, _donanimCount, gelenBarcode, printer.Data);
                            }
                            else
                            {
                                result = new Result(true, "Donanim Okundu!");
                            }
                        }
                        else
                        {
                            result = new Result(false, "Donanim Yazilamadi");
                        }
                    }
                }
                return result;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        //KK2T-14401-ABCD_000000004_T015-1_131129.txt
        //[0] KK2T-14401-ABCD
        //[1] 000000004
        //[2] T015-1
        //[3] 131129.txt
        public SyBarcodeOut GitYaziciDegiskenParcalama(SyBarcodeOut barcodece)
        {
            try
            {
                string[] oarca = barcodece.BarcodeIcerik.Split('-');
                barcodece.prefix = oarca[0];
                barcodece.family = oarca[1];
                barcodece.suffix = Regex.Replace(oarca[2], barcodece.RegexString, "");
                barcodece.IdDonanim = StringSpanConverter.GetCharsIsDigitPadingLeft(barcodece.BarcodeIcerik.AsSpan(), (int)barcodece.PadLeft).ToString();
                return barcodece;
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        private SuccessDataResults<SyBarcodeOut> GitBakbarcodeBasaymi(IzGenerateId izGenerates, IzDonanimCount donanimCount, SyBarcodeOut barcode, SyPrinter printer)
        {
            try
            {
                DirectPrinting yazici = null;
                string message = "Успешно отчете устройството!";
                string barcodeNameUpper = barcode.Name.ToUpper();
                bool isSpecialBarcode = false;

                if (barcodeNameUpper.Contains("FINAL"))
                {
                    yazici = new DirectPrinting(barcode, donanimCount, printer);
                    var veri = yazici.FinalEtiket(0, vardiya.Name, "3", printer.Id);
                    return new SuccessDataResults<SyBarcodeOut>(veri, message);
                }

                if (barcodeNameUpper.Contains("TORK") ||
                    barcodeNameUpper.Contains("FIRST") ||
                    barcodeNameUpper.Contains("KLIPTEST") ||
                    barcodeNameUpper.Contains("Revork-Giris") ||
                    barcodeNameUpper.Contains("Revork-Islem") ||
                    barcodeNameUpper.Contains("Revork-Cikis"))
                {
                    yazici = new DirectPrinting(barcode, izGenerates, printer);
                    isSpecialBarcode = true;
                }

                if (isSpecialBarcode)
                {
                    if (barcodeNameUpper.Contains("Revork-Giris"))
                    {
                        message = "Успешно записахте Revork Giris!";
                    }
                    else if (barcodeNameUpper.Contains("Revork-Islem"))
                    {
                        message = "Успешно извършихте Revork Islem!";
                    }
                    else if (barcodeNameUpper.Contains("Revork-Cikis"))
                    {
                        message = "Успешно завършихте Revork Cikis!";
                    }

                    var veri = yazici.KucukEtiketBas(vardiya.Name);
                    return new SuccessDataResults<SyBarcodeOut>(veri, message);
                }

                return new SuccessDataResults<SyBarcodeOut>(null, message);
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                // Обработка на грешката
                return null; // Препоръчително е да има по-подробна обработка на грешките
            }
        }

        private SuccessDataResults<UrKonveyorNumara> MasaGitBak(SyBarcodeInput barkode)
        {
            try
            {
                var veri = _MasaTara.MasaGet(barkode.OzelChar + barkode.BarcodeIcerik);
                _konveyorNumara = veri;
                barkode.BarcodeIcerik = null;
                if (veri != null)
                {
                    veri = null;
                    return new SuccessDataResults<UrKonveyorNumara>(veri, "Pano Okuttunuz!");
                }
                else
                {
                    veri = null;
                    return new SuccessDataResults<UrKonveyorNumara>(null, Message.PanoBulunamadi);
                }
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
                return null;
            }
        }

        public void SystemePFBKayitYap(IEnumerable<SyBarcodeInput> glnBarcode, string pfbSerial)
        {
            try
            {
                var donanim = GenerateIdBak(glnBarcode.FirstOrDefault()).Data;
                donanim.PFBSocket = pfbSerial;
                _repo.GetRepository<IzGenerateId>().Update(donanim);
                _repo.SaveChanges();
            }
            catch (ErrorExceptionHandller ex)
            {
                Messaglama.MessagException(ex.Message);
            }
        }
        public bool IsAlertGkLocked(string barcode)
        {
            // var harness = StringSpanConverter.SafeSubSpan(barcode.AsSpan(), 0, barcode.Length - 8).ToString();
            var harnes = _repo.GetRepository<IzGenerateId>().Get(x => x.Barcode == barcode).Data;
            if (harnes == null)
            {
                return false; // няма такъв харнес, не е заключено
            }
            var alertNumber = harnes.AlertNumber;
            var alertGk = _repo.GetRepository<OrAlertGk>().Get(x => x.AlertNumber == alertNumber).Data;
            return alertGk == null; // true ако няма запис (заключено)
        }
    }
}