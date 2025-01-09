using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.SortedList;

namespace Nursan.Validations.ValidationCode
{
    public abstract class ValidationCode //: IDisposable
    {
        DirectPrinting directPrinting;
        private BarcodeValidation valideBarcode;
        //private OpMashin _mashin;
        private UrIstasyon _urIstasyon;
        //private SyBarcodeInput _gelenBecode;
        //private SyPrinter _prtinter;
        //private UrModulerYapi _modulerYapi;
        private SyBarcodeOut _barcodeOUT;
        private IzDonanimCount _izDonanim;
        //private OrFamily _family;
        private readonly UnitOfWork _repo;
        //private MakineOpsionGetir makine;
        //private string _gelenVeri;
        //private string[] parca;
        //private SqlValidation sqlValidation;
 
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<UrModulerYapi> _modulerYapiList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        private static List<SyBarcodeOut> _syBarcodeOutList;
        private static List<SyPrinter> _syPrinterList;
        private static List<OrFamily> _familyList;
       // private static MakineOpsionGetir _mkOpsion;
        private EtiketBasma _elstesBsma;
       // private DonanimService _donanimService;
        TorkService tork;
        public ValidationCode(UnitOfWork repo)
        {
            _repo = repo;
            _izDonanim = new IzDonanimCount();
           // sqlValidation = new(_repo);
            //_gelenBecode = new SyBarcodeInput();
           // _donanimService = new DonanimService(_repo);

        }

        public Result ConfigCek(string gelenVeri)
        {
            try
            {
                // Парсване на входния стринг
 
                
                string[] parca = StringSpanConverter.SplitWithoutAllocationReturnArray(gelenVeri.AsSpan(),'_');
                if (parca == null || parca.Length < 3)
                {
                    return new Result(false, "Невалиден формат на данни");
                }

                // Инициализация на основни обекти
              //  InitializeMakineAndVardiya(parca[2]);

                // Валидация на входен баркод
                if (_syBarcodeInputList.Count > 0)
                {
                    return ValidateAndTriggerBarcode(parca);
                }

                // Проверка и обработка на харнес модела
                var harness = GetHarnessModel(parca[0]);
                if (harness != null)
                {
                    return ProcessHarnessModel(harness, parca);
                }

                // Генериране на етикет при липсващ харнес модел
                GenerateErrorBarcode(parca[0]);
                return new Result(false, "Referans Systemde Yok!" + parca[0]);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
  
        private Result ValidateAndTriggerBarcode(string[] parca)
        {
            _syBarcodeInputList.First().BarcodeIcerik = parca[0] + parca[1];
            valideBarcode = new BarcodeValidation(_repo, _makine, _vardiya, _istasyonList, _modulerYapiList, _syBarcodeInputList, _syBarcodeOutList, _syPrinterList, _familyList);
            return valideBarcode.BarcodeTetikle(_syBarcodeInputList);
        }

        private OrHarnessModel GetHarnessModel(string harnessName)
        {
            return _repo.GetRepository<OrHarnessModel>().Get(x => x.HarnessModelName == harnessName).Data;
        }

        private Result ProcessHarnessModel(OrHarnessModel harness, string[] parca)
        {
            var idResult = CreateAndGenerateId(harness);
            UpdateIdResultWithBarcode(parca, idResult);

            var result = AddDonanimAndHandlePackaging(idResult, harness);
            if (result.Success)
            {
                tork = new TorkService(_repo, new UrVardiya { Id = _vardiya.Id, Name = _vardiya.Name });
                tork.GitDegerleHerseySToplamV769IsBaypass(idResult.Id.ToString());
            }

            return _elstesBsma.BarodeBas(_izDonanim, harness);
        }

        private IzGenerateId CreateAndGenerateId(OrHarnessModel harness)
        {
            return _elstesBsma.AddGenbrateId(new IzGenerateId
            {
                HarnesModelId = harness.Id,
                ReferasnLeght = harness.HarnessModelName.Length,
                DonanimTorkReferansId = harness.OrHarnessConfigId,
                UrIstasyonId = _urIstasyon.Id,
                AlertNumber = harness.AlertNumber,
                CreateDate = TarihHesapla.GetSystemDate()
            }).Data;
        }

        private void UpdateIdResultWithBarcode(string[] parca, IzGenerateId idResult)
        {
            idResult.UpdateDate = TarihHesapla.GetSystemDate();
            idResult.DonanimIdLeght = idResult.Id.ToString().Length;
            _barcodeOUT = _syBarcodeOutList.FirstOrDefault(x => x.Id == _urIstasyon.SyBarcodeOutId);
            idResult.Barcode = parca[0] + idResult.Id.ToString().PadLeft((int)_barcodeOUT.PadLeft, '0');
            _elstesBsma.UpdateIDGenerate(idResult);
        }

        private Result AddDonanimAndHandlePackaging(IzGenerateId idResult, OrHarnessModel harness)
        {
            _izDonanim = new IzDonanimCount
            {
                DonanimReferans = idResult.Barcode,
                IdDonanim = idResult.Id,
                OrHarnessModel = harness.HarnessModelName,
                AlertNumber = harness.AlertNumber,
                VardiyaId = _vardiya.Id,
                MashinId = _makine.Id,
                UrIstasyonId = _urIstasyon.Id,
                CreateDate = TarihHesapla.GetSystemDate(),
                UpdateDate = TarihHesapla.GetSystemDate()
            };

            var result = _elstesBsma.AddDonanimIdAdd(_izDonanim);
            if (result.Success)
            {
                var paketResult = _elstesBsma.AddDPaketlemeId(_izDonanim);
                if (!paketResult.Success)
                {
                    Messaglama.MessagYaz(result.Message);
                }
                return paketResult;
            }
            else
            {
                Messaglama.MessagYaz(result.Message);
                return result;
            }
        }

        private void GenerateErrorBarcode(string reference)
        {
            _barcodeOUT = _syBarcodeOutList.First();
            _barcodeOUT.BarcodeIcerik = "Systemde bu Referans Yok!";
            directPrinting = new DirectPrinting(_barcodeOUT, new IzGenerateId(), _syPrinterList.FirstOrDefault(x => x.Id == _barcodeOUT.PrinetrId));
            directPrinting.FinalHataEtiketBas(_barcodeOUT);
            Messaglama.MessagYaz("Referans Systemde Yok!" + reference);
        }
        public SuccessDataResults<UrIstasyon> IstasyonGet(List<UrIstasyon> istasyon)
        {
            var result = istasyon.FirstOrDefault(x => x.MashinId == _makine.Id & x.VardiyaId == _vardiya.Id);
            if (result == null)
            {
                return new SuccessDataResults<UrIstasyon>(result, "Boyle bir istasyon boyle vardiya la yok!");
            }
            else
            {
                return new SuccessDataResults<UrIstasyon>(result, "Istasyon GetList");
            }
        }
    }
}
