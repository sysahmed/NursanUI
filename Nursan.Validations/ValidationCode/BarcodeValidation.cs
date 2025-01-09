using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.SortedList;
using System.Text.RegularExpressions;

namespace Nursan.Validations.ValidationCode
{
    public class BarcodeValidation
    {
        private readonly IUnitOfWork _repo;
        private MasaPanoValidasyonlari _MasaTara;
        static UrKonveyorNumara _urKonveyorNumara;
        public string getBarcode;
        IzDonanimCount _donanimCount;
        List<IzDonanimCount> izDonanimCountsList;
        DonanimService _donanimService;
        IzGenerateId _izGenerates;
        OrFamily _family;
        UrModulerYapi _modulerYapi;
        UrIstasyon _istasyon;
        SyBarcodeOut _barkodOut;
        SyPrinter _syPrinter;
        OrHarnessModel _orHarnessModel;
        OrHarnessConfig _orHarnessConfig;

        private List<IzDonanimCount> _donanimCountList;
        private List<IzGenerateId> _generaciaId;
        int sayiBarcode;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<UrModulerYapi> _modulerYapiList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        private static List<SyBarcodeOut> _syBarcodeOutList;
        private static List<SyPrinter> _syPrinterList;
        private static List<OrFamily> _familyList;
        private Result result;
        //private static string _gelenTorkPFBSerial;
        //private static string _gelenTorkPFBRef;
        //private int configIdHarness;
        private string _PfbSerial; SystemDeValidasyon sysValidatioon;
        public BarcodeValidation(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(_repo));
            _makine = makine ?? throw new ArgumentNullException(nameof(_makine));
            _vardiya = vardiya; //?? throw new ArgumentNullException(nameof(_vardiya));
            _modulerYapiList = modulerYapiList ?? throw new ArgumentNullException(nameof(_modulerYapiList));
            _istasyonList = istasyonList ?? throw new ArgumentNullException(nameof(_istasyonList));
            _syBarcodeInputList = syBarcodeInputList;// ?? throw new ArgumentNullException(nameof(_syBarcodeInputList));
            _syBarcodeOutList = syBarcodeOutList ?? throw new ArgumentNullException(nameof(_syBarcodeOutList));
            _syPrinterList = syPrinterList ?? throw new ArgumentNullException(nameof(_syPrinterList));
            _familyList = familyList ?? throw new ArgumentNullException(nameof(_familyList));
            _MasaTara = new MasaPanoValidasyonlari(repo);
            _donanimService = new DonanimService(repo);
            _istasyon = _istasyonList.FirstOrDefault(x => x.MashinId == makine.Id && x.Activ == true);
            _barkodOut = _syBarcodeOutList.FirstOrDefault(x => x.Id == _istasyon.SyBarcodeOutId);
            _syPrinter = _syPrinterList.FirstOrDefault(x => x.Id == _barkodOut.PrinetrId);
            _modulerYapi = _modulerYapiList.FirstOrDefault(x => x.Id == _istasyon.ModulerYapiId);
            _family = _familyList.FirstOrDefault(x => x.Id == _istasyon.FamilyId);
            _orHarnessConfig = new OrHarnessConfig();
            _orHarnessModel = new OrHarnessModel();
            _donanimCount = new IzDonanimCount();
            sysValidatioon = new SystemDeValidasyon(repo, _family);
        }
        public BarcodeValidation(UnitOfWork repo,
                        IzDonanimCount donanimCount)
        {
            _repo = repo;
            _MasaTara = new MasaPanoValidasyonlari(repo);
            _donanimCount = donanimCount;
            _donanimService = new DonanimService(repo);
        }

       
        public Result BarcodeTetikle(List<SyBarcodeInput> Barcode)
        {

            foreach (var item in Barcode)
            {

                result = BarcodeDegerle(item);

                if (result.Message != "Donanim Bi Onceki Istasyonda Okutun!")
                {
                    if (result.Message != "Donanim Okunmus!")
                    {
                        if (_istasyon.SyBarcodeOutId != null)
                        {
                            _barkodOut = _syBarcodeOutList.FirstOrDefault(x => x.Id == _istasyon.SyBarcodeOutId);
                            return GitBakbarcodeBasaymi(_izGenerates, _donanimCount, _barkodOut, _syPrinter);
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    return result;
                }
            }

            return result;
        }

        private Result BarcodeDegerle(SyBarcodeInput _bar)
        {
            if (_bar.OzelChar != null)
            {
                UpdateBarcodeIcerik(_bar);
            }

            var methodDictionary = new Dictionary<string, Func<SyBarcodeInput, Result>>
                 {
               {"Masa", MasaGitBak},
               {"First", GitBarcodeBak},
               {"PFBRef", GitBarcodePFBRefBak},
               {"PFBRefSerial", (input) => { _PfbSerial = input.BarcodeIcerik; return GitBarcodePFBRefSerialBak(input); }},
               {"Final", GitBarcodeFinalBak},
               {"Tork", GitBarcodeTorkBak},
                      };

            return methodDictionary.TryGetValue(_bar.Name, out Func<SyBarcodeInput, Result> method)
                ? method(_bar)
                : new Result(true, "\"Hata Olustu!\"");
        }

        private void UpdateBarcodeIcerik(SyBarcodeInput _bar)
        {
            if (_bar.BarcodeIcerik.StartsWith(_bar.OzelChar))
            {
                _bar.BarcodeIcerik = _bar.BarcodeIcerik.Remove(0, 1);
            }
            else if (_bar.BarcodeIcerik.EndsWith(_bar.OzelChar))
            {
                string[] parca = _bar.BarcodeIcerik.Split(_bar.OzelChar);
                _bar.BarcodeIcerik = parca[0];
            }
            else if (_bar.BarcodeIcerik.Contains(_bar.OzelChar))
            {
                string[] parca = _bar.BarcodeIcerik.Split(_bar.OzelChar);
                _bar.BarcodeIcerik = parca[0];
            }
        }
        private Result GitBarcodePFBRefSerialBak(SyBarcodeInput bar)
        {
            //_gelenTorkPFBSerial = bar.BarcodeIcerik;
            return new Result(true, "SeriNo Okundu!");
        }

        private Result GitBarcodeFinalBak(SyBarcodeInput bar)
        {
            return GitBarcodeBak(bar);
        }

        private DataResult<OrHarnessModel> GitBarcodeTorkBak(SyBarcodeInput bar)
        {
            string[] parcala = bar.BarcodeIcerik.Split('-'); string suffix = Regex.Replace(parcala[2], "[^a-z,A-Z,@,^,/,]", "");
            string veri = $"{parcala[0]}-{parcala[1]}-{suffix}";
            _orHarnessModel = _repo.GetRepository<OrHarnessModel>().Get(x => x.HarnessModelName == veri).Data;
            return new DataResult<OrHarnessModel>(_orHarnessModel, true, "Harness Cektiniz!");
        }

        private DataResult<OrHarnessConfig> GitBarcodePFBRefBak(SyBarcodeInput bar)
        {
            _orHarnessConfig = _repo.GetRepository<OrHarnessConfig>().Get(x => x.PFBSocket == bar.BarcodeIcerik && x.OrHarnessModelId == _orHarnessModel.Id).Data;
            return new DataResult<OrHarnessConfig>(_orHarnessConfig, true, "Donanim Cektiniz!");
        }

        public Result GitBarcodeBak(SyBarcodeInput barkode)
        {

            if (GenerateId(barkode).Success)
            {
                if (GitBarcodeTorkBak(barkode).Success)
                    DataCount(barkode);
                if (_modulerYapi.Etap.ToUpper() != "KONVEYOR" && _modulerYapi.Etap.ToUpper() != "ELTEST")
                {
                    var result = _istasyonList.FindAll(x => x.ModulerYapiId == _modulerYapi.Id);
                    // bak kactane result var ve moduler yapidan kactane gedjek ve hesapla
                    foreach (var item in result)
                    {
                        _donanimCount = _donanimCountList.FirstOrDefault(x => x.UrIstasyonId == _istasyon.Id);
                        if (_donanimCount == null)
                        {

                            if (GitSytemeSayiBac(barkode).Success == true)
                            {
                                sayiBarcode++;
                                if (_syBarcodeInputList.Count == sayiBarcode)
                                {
                                    if (AddDonanimCount(barkode, _izGenerates) == true)
                                    {
                                        if (_modulerYapi.Etap.ToUpper() == "TORK")
                                        {
                                            _izGenerates.UpdateDate = TarihHesapla.GetSystemDate();
                                            _izGenerates.PFBSocket = _PfbSerial;
                                            _izGenerates.DonanimTorkReferansId = _orHarnessConfig.Id;
                                            UpdateIDGenerate(_izGenerates);
                                            sayiBarcode = 0;
                                            return new Result(true, "Donanimi Okuttunuz!");
                                        }
                                        sayiBarcode = 0;
                                        return new Result(true, "Donanimi Okuttunuz!");
                                    }

                                }
                            }
                            else
                            {
                                sayiBarcode = 0;
                                return new Result(false, "Donanim Bi Onceki Istasyonda Okutun!");
                            }
                        }
                        else if (_donanimCount.DonanimReferans != null && _donanimCount.DonanimReferans == barkode.BarcodeIcerik)
                        {
                            sayiBarcode = 0;
                            return new Result(false, "Donanim Okunmus!");
                        }
                    }

                }
                else if (_modulerYapi.Etap.ToUpper() == "ELTEST")
                {
                    if (_donanimCountList.Count() > 0)
                    {
                        sayiBarcode = 0;
                        return new Result(true, "Donanimi Okuttunuz!");
                    }
                    else
                    {
                        if (AddDonanimCount(barkode, _izGenerates) == true)
                        {
                            sayiBarcode = 0;
                            return new Result(true, "Donanimi Okuttunuz!");
                        }
                    }
                }
                else
                {
                    if (_donanimCountList.Count() == 0)
                    {
                        if (AddDonanimCount(barkode, _izGenerates) == true)
                        {
                            sayiBarcode = 0;
                            return new Result(true, "Donanimi Okuttunuz!");
                        }
                        else
                        {
                            sayiBarcode = 0;
                            return new Result(false, "Donanim Okunmus!");
                        }
                    }
                    else
                    {
                        sayiBarcode = 0;
                        return new Result(false, "Donanim Systemde Okunmus!");
                    }
                }
            }
            else
            {
                sayiBarcode = 0;
                return new Result(false, "Donanim Systemde Yok!");
            }
            return new Result(false, "Donanim Systemde Yok!");
        }
        List<UrIstasyon> novini = new();

        private Result GitSytemeSayiBac(SyBarcodeInput barkode)
        {
            var results = sysValidatioon.GetByOkunan(barkode.BarcodeIcerik);
            var fins = sysValidatioon.GetByIstasyon().Where(x => x.ModulerYapiId < _istasyon.ModulerYapiId);
            foreach (var i in fins)
            {
                try
                {
                    //var rt = results.MinBy(x => x.UrIstasyonId == i.Id);
                    if (!results.Any(x => x.UrIstasyonId == i.Id))
                    {
                        result = new Result(false, "Donanimi Bi oceki Istasyona Yonlendirin");
                    }
                    else
                    {
                        result = new Result(true);
                    }
                }
                catch (Exception ex)
                {
                    result = new Result(false, "Donanimi Bi oceki Istasyona Yonlendirin");
                }

            }
            return result;
        }

        public SuccessDataResults<IzGenerateId> UpdateIDGenerate(IzGenerateId data)
        {
            var result = _repo.GetRepository<IzGenerateId>().Update(data);
            return new SuccessDataResults<IzGenerateId>(result.Data, result.Message);
        }
        /// <summary>
        /// Barkod
        /// </summary>
        /// <param name="izGenerates"></param>
        /// <param name="donanimCount"></param>
        /// <param name="barcode"></param>
        /// <param name="printer"></param>
        /// <returns></returns>
        private SuccessDataResults<SyBarcodeOut> GitBakbarcodeBasaymi(IzGenerateId izGenerates, IzDonanimCount donanimCount, SyBarcodeOut barcode, SyPrinter printer)
        {


            if (_syBarcodeOutList.Exists(x=>x.Name == "Final"))
            {
                DirectPrinting yazici = new(barcode, donanimCount, _syPrinter);
                var veri = yazici.FinalEtiket(0, Environment.MachineName + _vardiya.Name, "", printer.Id);
                return new SuccessDataResults<SyBarcodeOut>(veri, "Donanimi Okuttunuz!");
            }
            else if (_syBarcodeOutList.Exists(x => x.Name == "Tork"))
            {
                DirectPrinting yazici = new(barcode, izGenerates, _syPrinter);
                var veri = yazici.KucukEtiketBas(_vardiya.Name);
                return new SuccessDataResults<SyBarcodeOut>(veri, "Donanimi Okuttunuz!");

            }
            else if (_syBarcodeOutList.Exists(x => x.Name == "First"))
            {
                DirectPrinting yazici = new(barcode, izGenerates, _syPrinter);
                var veri = yazici.KucukEtiketBas(_vardiya.Name);
                return new SuccessDataResults<SyBarcodeOut>(veri, "Donanimi Okuttunuz!");
            }
            else
            {

                return new SuccessDataResults<SyBarcodeOut>(null, "Donanimi Okuttunuz!");
            }

        }
        /// <summary>
        /// Chat Cpt Ile guncellendi!
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool AddDonanimCount(SyBarcodeInput barcode, IzGenerateId data)
        {
            var donanimCount = new IzDonanimCount
            {
                DonanimReferans = barcode.BarcodeIcerik,
                IdDonanim = data.Id,
                OrHarnessModel = _donanimService.GetDonanimHarnesOne((int)data.HarnesModelId)?.HarnessModelName,
                AlertNumber = _donanimService.GetAllAlertBaglanti((int)data.AlertNumber)?.AlertNumber,
                MasaId = _urKonveyorNumara?.Id,
                VardiyaId = _vardiya.Id,
                MashinId = _istasyon.MashinId,
                UrIstasyonId = (data.UrIstasyonId != null) ? _istasyon.Id : 0,
                CreateDate = TarihHesapla.GetSystemDate(),
                UpdateDate = TarihHesapla.GetSystemDate(),
            };
            _repo.GetRepository<IzDonanimCount>().Add(donanimCount);
            return true;
        }
        public SuccessDataResults<IEnumerable<IzDonanimCount>> DataCount(SyBarcodeInput barkode)
        {
            var res = StringSpanConverter.SplitWithoutAllocationReturnArray(barkode.BarcodeIcerik.AsSpan(), barkode.ParcalamaChar.Value);
            var idres = StringSpanConverter.GetCharsIsDigit(res[2]);
            _donanimCountList = _repo.GetRepository<IzDonanimCount>()
                .GetAll(x => x.IdDonanim == idres)
                .Data;
            return new SuccessDataResults<IEnumerable<IzDonanimCount>>(_donanimCountList, Message.DoanimIDOkutunuz);
        }

        public SuccessDataResults<IzGenerateId> GenerateId(SyBarcodeInput barkode)
        {

            var res = StringSpanConverter.SplitWithoutAllocationReturnArray(barkode.BarcodeIcerik.AsSpan(), barkode.ParcalamaChar.Value);
            var idres = StringSpanConverter.GetCharsIsDigit(res[2]);
            _izGenerates = _repo.GetRepository<IzGenerateId>()
                .Get(x => x.Id == idres)
                .Data;
            return new SuccessDataResults<IzGenerateId>
                  (_izGenerates, Message.DoanimIDOkutunuz);
        }
        private SuccessDataResults<SyBarcodeInput> MasaGitBak(SyBarcodeInput barkode)
        {
            var veri = _MasaTara.MasaGet(barkode.OzelChar + barkode.BarcodeIcerik);
            _urKonveyorNumara = veri;
            barkode.BarcodeIcerik = null;
            if (veri != null)
            {
                veri = null;
                return new SuccessDataResults<SyBarcodeInput>(barkode, "Pano Okuttunuz!");
            }
            else
            {
                veri = null;
                return new SuccessDataResults<SyBarcodeInput>(null, Message.PanoBulunamadi);
            }
        }
        public List<SyBarcodeInput> GetBarcodeInput()
        {
            return _repo.GetRepository<SyBarcodeInput>()
                .GetAll(null).Data;
        }

        public SyBarcodeInput AddBarcodeIn(SyBarcodeInput barce)
        {
            var result = _repo.GetRepository<SyBarcodeInput>()
                .Add(barce);
            _repo.SaveChanges();
            return result.Data;
        }
        public SyBarcodeOut AddBarcodeOut(SyBarcodeOut barce)
        {

            var result = _repo.GetRepository<SyBarcodeOut>()
                .Add(barce);
            _repo.SaveChanges();

            return result.Data;
        }
        public List<SyBarcodeOut> GetBarcodeOut()
        {
            return _repo.GetRepository<SyBarcodeOut>()
                .GetAll(null)
                .Data;
        }
    }
}
