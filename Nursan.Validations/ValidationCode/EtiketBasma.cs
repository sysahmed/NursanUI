using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Validations.ValidationCode
{
    public class EtiketBasma
    {
        //UretimOtomasyonContext contex;
        private readonly IUnitOfWork _repo;
        //private readonly string _name;
        //private OpMashin _makine;// = new OpMashin();
        //private UrModulerYapi _modulerYapi;
        private UrVardiya _vardiya;// = new UrVardiya();
        private UrIstasyon _urIstasyon;//= new List<UrIstasyon>();
        //private List<UrIstasyon> _istasyonList;
        //private List<SyBarcodeInput> _syBarcodeINPUTList;//= new List<BarcodeINPUT>();
        private SyPrinter _syPrtinter;
        //private List<UrModulerYapi> _modulerYapiList;// = new UrModulerYapi();
        private List<SyBarcodeOut> _syBarcodeOUTList;// = new List<BarcodeOUT>();
        //private OrFamily _family;
        //IzDonanimHedef izDonanimHedef;
        List<OrHarnessModel> harnesModelList;
        //List<OrHarnessModel> harnesModelListSecond;
        //List<IzDonanimHedef> donanimHedefs;//= new List<DonanimHedef>();
        //List<IzDonanimHedef> donanimHedesECOND;
        //List<OrFamily> familyList;
        //XMLIslemi xmlim;

        //string konveyor = string.Empty;
        // public static string _sicil;
        delegate void SetTextCallback(string text);
        //List<HarnesDonanimHedef> harnesDonanimHedefsList;
        //HarnesDonanimHedef harnesDonanimHedef;
        DirectPrinting directPrintin;
        //TorkService torkService;
        public EtiketBasma(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, UrIstasyon Istasyon, List<UrIstasyon> istasyonList, UrModulerYapi modulerYapi, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, SyPrinter syPrinter, OrFamily family)
        {
            _repo = repo;
            //_makine = makine;
            _vardiya = vardiya;
            _urIstasyon = Istasyon;
            //_modulerYapiList = modulerYapiList;
            //_modulerYapi = modulerYapi;
            //_istasyonList = istasyonList;
            //_syBarcodeINPUTList = syBarcodeInputList;
            _syBarcodeOUTList = syBarcodeOutList;
            _syPrtinter = syPrinter;
            //_family = family;
            //donanimHedefs = new List<IzDonanimHedef>();
            //donanimHedesECOND = new List<IzDonanimHedef>();
            //harnesDonanimHedef = new HarnesDonanimHedef();
            //harnesDonanimHedefsList = new List<HarnesDonanimHedef>();
            //izDonanimHedef = new IzDonanimHedef();
            //torkService = new TorkService(repo, vardiya);
        }
        public SuccessDataResults<IzGenerateId> AddGenbrateId(IzGenerateId id)
        {
            var result = _repo.GetRepository<IzGenerateId>().Add(id);
            return new SuccessDataResults<IzGenerateId>(result.Data, result.Message);
        }
        public SuccessDataResults<IzGenerateId> UpdateIDGenerate(IzGenerateId data)
        {
            var result = _repo.GetRepository<IzGenerateId>().Update(data);
            return new SuccessDataResults<IzGenerateId>(result.Data, result.Message);
        }
        private void VeriGetir(string name)
        {
            harnesModelList = _repo.GetRepository<OrHarnessModel>().GetAll(x => x.HarnessModelName == name).Data;
        }

        public Result BarodeBas(IzDonanimCount donanimCounting, OrHarnessModel gl)
        {
            try
            {
                VeriGetir(gl.HarnessModelName);
                var harness = harnesModelList.FirstOrDefault(x => x.HarnessModelName == gl.HarnessModelName);

                /* harness.HarnessModelName + harness.Id.ToString().PadLeft((int)_syBarcodeOUTList.FirstOrDefault(x => x.Id == _urIstasyon.SyBarcodeOutId).PadLeft, '0')*/
                ;
                //UpdateIDGenerate(result.Data);

                var barcode = _syBarcodeOUTList.FirstOrDefault(x => x.Id == _urIstasyon.SyBarcodeOutId);
                barcode.BarcodeIcerik = harness.HarnessModelName;

                barcode.family = harness.Family;
                barcode.suffix = harness.Suffix;
                barcode.prefix = harness.Prefix;
                // barcode.Sira1 = split1[1].ToString().Split("/")[1];
                barcode.IdDonanim = donanimCounting.IdDonanim.ToString().PadLeft((int)_syBarcodeOUTList.FirstOrDefault(x => x.Id == _urIstasyon.SyBarcodeOutId).PadLeft, '0');
                barcode.CreateDate = TarihHesapla.GetSystemDate();
                directPrintin = new DirectPrinting(barcode, donanimCounting, _syPrtinter);

                directPrintin.FinalEtiket(1, _vardiya.Name, XMLIslemi.XmlDensity(), 1);
                barcode = null;
                return new Result(true, "Donanim Bastiniz!");

            }
            catch (Exception ex)
            {

                return new Result(false, ex.Message);
            }
        }

        internal Result AddDonanimIdAdd(IzDonanimCount izDonanimCount)
        {
            try
            {
                var result = _repo.GetRepository<IzDonanimCount>().Add(izDonanimCount).Success;
                return new Result(true, izDonanimCount.DonanimReferans + "Yazdirildi");
            }
            catch (Exception)
            {
                return new Result(false, izDonanimCount.DonanimReferans + "Tazdirilamadi");
            }
        }
        internal Result AddDPaketlemeId(IzDonanimCount izDonanimCount)
        {
            if (_urIstasyon != null)
                izDonanimCount.UrIstasyonId = _urIstasyon.Id;
            IzPaketCount izPaketCount = new IzPaketCount
            {
                DonanimReferans = izDonanimCount.DonanimReferans,
                AlertNumber = izDonanimCount.AlertNumber,
                VardiyaId = izDonanimCount.VardiyaId,
                MachinId = izDonanimCount.MashinId,
                Id = 0,
                UrIstasyonId = izDonanimCount.UrIstasyonId,
                CreateDate = TarihHesapla.GetSystemDate(),
                UpdateDate = TarihHesapla.GetSystemDate()
            };
            try
            {
                var result = _repo.GetRepository<IzPaketCount>().Add(izPaketCount).Success;
                return new Result(true, izPaketCount.DonanimReferans + "Yazdirildi");
            }
            catch (Exception)
            {
                return new Result(false, izPaketCount.DonanimReferans + "Yazdirilamadi Hata!");
            }
        }
    }
}
