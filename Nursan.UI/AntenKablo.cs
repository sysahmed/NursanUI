using Nursan.Business.Services;
using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Domain.ModelsDisposible;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;
using Nursan.XMLTools;

namespace Nursan.UI
{
    public partial class AntenKablo : Form
    {
        public event System.IO.Ports.SerialDataReceivedEventHandler DataReceived;
        UretimOtomasyonContext contex;
        private readonly UnitOfWork _repo;
        private readonly string _name;
        private OpMashin _makine;// = new OpMashin();
        private UrModulerYapi _modulerYapi;
        private UrVardiya _vardiya;// = new UrVardiya();
        private UrIstasyon _urIstasyon;//= new List<UrIstasyon>();
        private List<UrIstasyon> _istasyonList;
        private List<SyBarcodeInput> _syBarcodeINPUTList;//= new List<BarcodeINPUT>();
        private List<SyPrinter> _syPrtinterList;
        private List<UrModulerYapi> _modulerYapiList;// = new UrModulerYapi();
        private List<SyBarcodeOut> _syBarcodeOUTList;// = new List<BarcodeOUT>();
        static ListViewItem listce;

        IzCoaxCableConfig izCoaxCableConfig;
        List<IzCoaxCableConfig> coaxCableConfig;
        List<IzCoaxCableCross> coaxCableCross;//= new List<DonanimHedef>();
        List<OrFamily> familyList;
        GenrateIdDonanimManager generateIdDonanimManager;
        XMLIslemi xmlim;
        //SerialPort seril;
        private DateTime t;
        string konveyor = string.Empty;
        public static string _sicil;
        delegate void SetTextCallback(string text);
        List<OrHarnessModel> harnesDonanimList;
        List<HarnesDonanimCoax> harnesDonanimCoaxList;
        HarnesDonanimCoax harnesDonanimCoax;
        List<IzCoaxCableCount> izCoaxCableCounts;
        SyBarcodeOut _sysBarcodOut;
        DirectPrinting directPrintin;
        // TorkService torkServices;
        private int sayiCount;
        public AntenKablo(string sicil, UnitOfWork repo)
        {
            InitializeComponent();
            _repo = repo;
            StaticDegisken();
            _sysBarcodOut = new SyBarcodeOut();
        }
        public AntenKablo(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> family)
        {
            InitializeComponent();
            _repo = repo;
            _makine = makine;
            _vardiya = vardiya;
            _urIstasyon = istasyonList.FirstOrDefault(x => x.MashinId == _makine.Id & x.VardiyaId == vardiya.Id);
            StaticDegisken();
            _modulerYapiList = modulerYapiList;
            // _modulerYapi = modulerYapi;
            _istasyonList = istasyonList;
            _syBarcodeINPUTList = syBarcodeInputList;
            _syBarcodeOUTList = syBarcodeOutList;
            _sysBarcodOut = _syBarcodeOUTList.First();
            _syPrtinterList = syPrinterList;
            familyList = family;
            int lstw = listReferansSec.Size.Width;
            int lsth = listReferansSec.Size.Height;
            contex = new();
            generateIdDonanimManager = new GenrateIdDonanimManager(_repo);
            // torkServices = new TorkService(repo, vardiya);

        }
        private void StaticDegisken()
        {
            izCoaxCableCounts = new List<IzCoaxCableCount>();
            coaxCableCross = new List<IzCoaxCableCross>();
            harnesDonanimCoaxList = new List<HarnesDonanimCoax>();
            harnesDonanimList = new List<OrHarnessModel>();
            xmlim = new XMLIslemi();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            VeriGetir();
        }
        #region Buton
        private void btnHome_Click(object sender, EventArgs e)
        {
            VeriGetir();
        }
        private void AnaSayfa_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        #endregion
        #region Other
        public void VeriGetir()
        {
            textBox1.Text = "";
            listReferansSec.BeginUpdate();
            listReferansSec.Items.Clear();
            listReferansSec.EndUpdate();
            lstBiten.Items.Clear();

            // Изчисти списъка!
            harnesDonanimCoaxList.Clear();

            coaxCableCross = _repo.GetRepository<IzCoaxCableCross>().GetAll(x => x.Activ == true).Data;
            coaxCableConfig = _repo.GetRepository<IzCoaxCableConfig>().GetAll(x => x.Activ == true).Data;
            TarihHIM tr = TarihHesapla.TarihHesab();
            harnesDonanimList = _repo.GetRepository<OrHarnessModel>().GetAll(x => x.FamilyId == _urIstasyon.FamilyId && x.Activ == true).Data;

            // Създай речник за бързо търсене
            var coaxConfigDict = coaxCableConfig.ToDictionary(x => x.Id, x => x.CoaxCabloReferans);

            listReferansSec.BeginUpdate();
            // Покажи само Harness моделите, без да зареждаш Coax кабели
            foreach (var harness in harnesDonanimList)
            {
                // Покажи ВСИЧКИ Harness модели
                listReferansSec.Items.Add(harness.HarnessModelName + "    ");
                // НЕ зареждай Coax кабели тук!
            }
            listReferansSec.EndUpdate();
        }
        private async void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Ако е търсене, не се изпълнявай повторно
            if (_isSearching) return;
            
            if (listAntenCabloOut.Items.Count == 0)
            {
                listAntenCableIn.Items.Clear();
                listAntenCabloOut.Items.Clear();
                t = TarihHesapla.GetSystemDate();
                
                if (listReferansSec.SelectedIndices.Count <= 0) return;
                
                int selectedIndex = listReferansSec.SelectedIndices[0];
                if (selectedIndex >= 0)
                {
                    string selectedHarnessName = listReferansSec.Items[selectedIndex].Text.TrimEnd();
                    label1.Text = selectedHarnessName;
                    
                    // Намери избрания Harness модел
                    var selectedHarness = harnesDonanimList.FirstOrDefault(x => x.HarnessModelName == selectedHarnessName);
                    if (selectedHarness != null)
                    {
                        // ЗАЯВКА КЪМ DB - зареди Coax кабели за този Harness
                        var coaxCablesForSelectedHarness = _repo.GetRepository<IzCoaxCableCross>()
                            .GetAll(x => x.HarnessModelId == selectedHarness.Id && x.Activ == true).Data;
                        
                        // Изчисти списъка преди да го пълниш
                        harnesDonanimCoaxList.Clear();
                        
                        // Добави Coax кабелите в списъка
                        foreach (var coax in coaxCablesForSelectedHarness)
                        {
                            if (!coax.CoaxCableBarcodeId.HasValue) continue;
                            var coaxConfig = coaxCableConfig.FirstOrDefault(x => x.Id == coax.CoaxCableBarcodeId.Value);
                            if (coaxConfig != null)
                            {
                                listAntenCableIn.Items.Add(coaxConfig.CoaxCabloReferans);
                                
                                // Добави към harnesDonanimCoaxList за по-късно използване
                                harnesDonanimCoaxList.Add(new HarnesDonanimCoax
                                {
                                    HarnesId = selectedHarness.Id,
                                    harnessModel = selectedHarness.HarnessModelName,
                                    CoaxId = coax.Id,
                                    BarcodeCoax = coaxConfig.CoaxCabloReferans
                                });
                            }
                        }
                    }
                }
                this.ActiveControl = textBox1;
                textBox1.Focus();
            }
        }
        private void cbKonveyor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //konveyor = cbFamily.Text;
            VeriGetir();
        }
        private void cbFamily_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // konveyor = cbFamily.Text;
                //  VeriGetir();
                //  cbFamily.Text = "";
            }
        }
        string idDonanimBarcode;
        private bool BarcodeBas(List<IzCoaxCableCount> listCoaxCount, HarnesDonanimCoax gl, int sayikount)
        {
            t = TarihHesapla.GetSystemDate();
            string formattedDateTime = $"{t.Year:D4}{t.Month:D2}{t.Day:D2}{t.Hour:D2}{t.Minute:D2}{t.Second:D2}";

            // Парсване на форматирания низ в Int64
            long idName = Int64.Parse(formattedDateTime);
            foreach (var item in listCoaxCount)
            {

                idDonanimBarcode = item.CoaxTutulanId.ToString();
                item.CoaxTutulanId = idName;
                item.Activ = true;
                _repo.GetRepository<IzCoaxCableCount>().Add(item);
                _repo.SaveChanges();
            }
            _sysBarcodOut.IdDonanim = _sysBarcodOut.BarcodeIcerik = idName.ToString();
            _sysBarcodOut.Leght = sayikount;
            directPrintin = new DirectPrinting(_sysBarcodOut, _syPrtinterList.First(), gl);
            directPrintin.AntenKabloBas("V");
            listCoaxCount = null;
            izCoaxCableCounts = null;
            // izCoaxCableCounts = new();
            return true;
        }

        #endregion
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (listAntenCableIn.Items.Count == 0) return;
                sayiCount = (int)numSayi.Value;
                string inputText = textBox1.Text.ToUpper();
                // Създай HashSet за по-бързо търсене
                var inItems = new HashSet<string>(listAntenCableIn.Items.Cast<string>(), StringComparer.OrdinalIgnoreCase);
                string found = inItems.FirstOrDefault(lb => inputText.Contains(lb.ToUpper()));
                if (found != null)
                {
                    try
                    {
                        if (izCoaxCableCounts == null)
                            izCoaxCableCounts = new();
                        // Създай речник за coaxCableConfig
                        var coaxConfigDict = coaxCableConfig.ToDictionary(x => x.CoaxCabloReferans, x => x.Id, StringComparer.OrdinalIgnoreCase);
                        var harnesDict = harnesDonanimCoaxList.FirstOrDefault(x => x.harnessModel == label1.Text);
                        izCoaxCableConfig = new(); harnesDonanimCoax = new();
                        int veri = coaxConfigDict[found];
                        int coaxId = coaxCableCross.FirstOrDefault(x => x.CoaxCableBarcodeId == veri)?.Id ?? 0;
                        izCoaxCableCounts.Add(new IzCoaxCableCount
                        {
                            HarnessModelId = harnesDict.HarnesId,
                            CoaxCableName = inputText,
                            CoaxCableId = coaxId,
                            UrIstasyonId = _urIstasyon.Id,
                            OrPcNameId = _urIstasyon.MashinId,
                            VardiyaId = _urIstasyon.VardiyaId,
                            Activ = true,
                            CreateDate = TarihHesapla.GetSystemDate(),
                        });
                        textBox1.Clear();
                        listAntenCabloOut.BeginUpdate();
                        listAntenCabloOut.Items.Add(found);
                        listAntenCabloOut.EndUpdate();
                        listAntenCableIn.BeginUpdate();
                        listAntenCableIn.Items.Remove(found);
                        listAntenCableIn.EndUpdate();
                        if (listAntenCableIn.Items.Count == 0)
                        {
                            for (int r = 0; r < listAntenCabloOut.Items.Count; r++)
                            {
                                string propName = $"Sira{r + 1}";
                                string modelValue = listAntenCabloOut.Items[r].ToString();
                                harnesDonanimCoax.GetType().GetProperty(propName)?.SetValue(harnesDonanimCoax, modelValue);
                            }
                            sayiCount++;
                            harnesDonanimCoax.harnessModel = label1.Text;
                            BarcodeBas(izCoaxCableCounts, harnesDonanimCoax, sayiCount);
                            listAntenCableIn.BeginUpdate();
                            foreach (var item in harnesDonanimCoaxList.Where(x => x.harnessModel == label1.Text))
                            {
                                listAntenCableIn.Items.Add(item.BarcodeCoax);
                            }
                            listAntenCableIn.EndUpdate();
                            listAntenCabloOut.Items.Clear();
                            izCoaxCableConfig = null; harnesDonanimCoax = null;
                            izCoaxCableCounts = null;
                            numSayi.Value = sayiCount;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Може да се логне грешката при нужда
                    }
                }
            }
        }

        private void AntenKablo_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private bool _isSearching = false; // Флаг за предотвратяване на двойно извикване
        
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = textBox2.Text.Trim();
            
            if (string.IsNullOrEmpty(searchText))
            {
                // Ако полето е празно, премахни селекцията
                listReferansSec.SelectedItems.Clear();
                return;
            }

            // Търси в ListView елементите
            for (int i = 0; i < listReferansSec.Items.Count; i++)
            {
                string itemText = listReferansSec.Items[i].Text;
                if (itemText.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Намери първия съвпадащ елемент
                    listReferansSec.SelectedIndices.Clear();
                    listReferansSec.SelectedIndices.Add(i);
                    listReferansSec.EnsureVisible(i);
                    
                    // Ако е натиснат Enter, избери този елемент
                    if (e.KeyCode == Keys.Enter)
                    {
                        _isSearching = true; // Маркирай, че е търсене
                        // Симулирай клик върху избрания елемент
                        listReferansSec.Focus();
                        // Тригервай събитието за избор
                        listView1_SelectedIndexChanged_1(listReferansSec, e);
                        _isSearching = false; // Премахни маркера
                    }
                    return;
                }
            }
            
            // Ако не е намерен нито един елемент, премахни селекцията
            if (e.KeyCode != Keys.Enter)
            {
                listReferansSec.SelectedItems.Clear();
            }
        }

        private void btnPrintConfig_Click(object sender, EventArgs e)
        {
            VeriGetir();
        }
    }
}


