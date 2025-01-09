using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Validations.ValidationCode
{
    public class MakineOpsionGetir
    {
        #region Degisken
        //private readonly UretimOtomasyonContext _context;
        private readonly IUnitOfWork _repo;
        private readonly string _vardiyaName;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<UrModulerYapi> _modulerYapiList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        private static List<SyBarcodeOut> _syBarcodeOutList;
        private static List<SyPrinter> _syPrinterList;
        private static List<OrFamily> _familyList;

        private List<SyBarcodeInCrossIstasyon> syBarcodeInCrossIstasyons;
        #endregion

        #region Construktor
        public MakineOpsionGetir(UretimOtomasyonContext context,
           IUnitOfWork repo, out OpMashin makine, out UrVardiya vardiya, out List<UrIstasyon> istasyonList, out List<UrModulerYapi> modulerYapiList, out List<SyBarcodeInput> syBarcodeInputList, out List<SyBarcodeOut> syBarcodeOutList, out List<SyPrinter> syPrinterList, out List<OrFamily> familyList,
            string vardiyaName)// 
        {
            //_context = context;
            _repo = repo;
            _vardiyaName = vardiyaName;
            _vardiya = GEtVardiya();
            _makine = GetMakine();
            _familyList = GetFamily();
            _istasyonList = GetIstasyonList();
            _modulerYapiList = GetModulerYapiList();
            syBarcodeInCrossIstasyons = GetSyBarcodeInCrossIstasyons();
            _syBarcodeInputList = GetBarcodeInput();
            _syBarcodeOutList = GetBarcodeOutBak();
            _syPrinterList = GetPrinterList();

            vardiya = _vardiya;
            makine = _makine;
            familyList = _familyList;
            istasyonList = _istasyonList;
            modulerYapiList = _modulerYapiList;
            syBarcodeInputList = _syBarcodeInputList;
            syBarcodeOutList = _syBarcodeOutList;
            syPrinterList = _syPrinterList;

        }

        #endregion

        #region Funcsionnlar
        /// <summary>
        /// Get Vardiya
        /// </summary>
        /// <returns></returns>
        public UrVardiya GEtVardiya()
        {
            try
            {
                if (_vardiya is null)
                    _vardiya = _repo.GetRepository<UrVardiya>().Get(x => x.Name == _vardiyaName).Data;
            }
            catch (Exception ex)
            {
                return null;
            }
            return _vardiya;
        }
        /// <summary>
        /// Get Makine Makine verir Pc Ismi systemde
        /// </summary>
        /// <returns></returns>
        public OpMashin GetMakine()
        {
            if (_makine is null)
                _makine = _repo.GetRepository<OpMashin>().Get(x => x.MasineName == Environment.MachineName).Data;
            return _makine;
        }
        /// <summary>
        /// hello Family cek pc isminden filtrele
        /// </summary>
        /// <returns></returns>
        public List<OrFamily> GetFamily()
        {
            if (_familyList is null)
                _familyList = _repo.GetRepository<OrFamily>().GetAll(null).Data;
            return _familyList;
        }
        public List<UrIstasyon> GetIstasyonList()
        {
            if (_istasyonList is null && _familyList is not null)
            {
                _istasyonList = new List<UrIstasyon>();
                foreach (var item in _familyList)
                {
                    var returnco = _repo.GetRepository<UrIstasyon>().GetAll(x => x.FamilyId == item.Id).Data.OrderBy(x => x.Id);
                    if (returnco != null)
                    {
                        _istasyonList.AddRange(returnco);
                    }
                }
            }
            else if (_istasyonList is null)
            {
                _familyList = GetFamily();
                GetIstasyonList();
            }
            return _istasyonList;
        }
        public List<UrModulerYapi> GetModulerYapiList()
        {
            if (_modulerYapiList is null)
                _modulerYapiList = _repo.GetRepository<UrModulerYapi>().GetAll(null).Data;
            return _modulerYapiList;
        }
        private List<SyBarcodeInCrossIstasyon> GetSyBarcodeInCrossIstasyons()
        {
            try
            {
                if (syBarcodeInCrossIstasyons is null && _istasyonList is not null)
                {
                    syBarcodeInCrossIstasyons = new List<SyBarcodeInCrossIstasyon>();
                    var veri = _istasyonList.Where(x => x.MashinId == _makine.Id && x.VardiyaId == _vardiya.Id);
                    foreach (var item in veri)
                    {
                        syBarcodeInCrossIstasyons.AddRange(_repo.GetRepository<SyBarcodeInCrossIstasyon>().GetAll(x => x.UrIstasyonId == item.Id).Data);
                    }
                }
                else if (_istasyonList is null)
                {
                    _istasyonList = GetIstasyonList();
                    GetSyBarcodeInCrossIstasyons();
                }
                return syBarcodeInCrossIstasyons;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public List<SyBarcodeInput> GetBarcodeInput()
        {
            try
            {
                if (_syBarcodeInputList is null && syBarcodeInCrossIstasyons is not null)
                {
                    _syBarcodeInputList = new List<SyBarcodeInput>();
                    foreach (var item in syBarcodeInCrossIstasyons)
                    {
                        _syBarcodeInputList.Add(_repo.GetRepository<SyBarcodeInput>().Get(x => x.Id == item.SysBarcodeInId).Data);
                    }
                }
                else if (syBarcodeInCrossIstasyons is null)
                {
                    syBarcodeInCrossIstasyons = GetSyBarcodeInCrossIstasyons();
                    GetBarcodeInput();
                }
                return _syBarcodeInputList;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<SyBarcodeOut> GetBarcodeOutBak()
        {
            try
            {
                if (_syBarcodeOutList is null && _istasyonList is not null)
                {
                    _syBarcodeOutList = new List<SyBarcodeOut>();
                    foreach (var item in _istasyonList.Where(x => x.MashinId == _makine.Id))
                    {
                        _syBarcodeOutList.Add(_repo.GetRepository<SyBarcodeOut>().Get(x => x.Id == item.SyBarcodeOutId).Data);
                    }
                }
                else if (_istasyonList is null)
                {
                    _istasyonList = GetIstasyonList();
                    GetBarcodeInput();
                }
                return _syBarcodeOutList;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<SyPrinter> GetPrinterList()
        {
            try
            {

                if (_syPrinterList is null && _syBarcodeOutList is not null)
                {
                    _syPrinterList = new List<SyPrinter>();
                    foreach (var item in _syBarcodeOutList)
                    {
                        _syPrinterList.Add(_repo.GetRepository<SyPrinter>().Get(x => x.Id == item.PrinetrId).Data);
                    }

                }
                return _syPrinterList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        #endregion

    }
}
