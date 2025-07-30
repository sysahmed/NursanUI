using Microsoft.EntityFrameworkCore;
using Nursan.Business.Services;
using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Domain.ModelsDisposible;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;
using Nursan.XMLTools;
using System.Data.Common;
using System.Windows.Forms;

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
        DirectPrinting directPrintin; private List<string> allItems = new List<string>();
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
            _urIstasyon = istasyonList?.FirstOrDefault(x => x.MashinId == _makine.Id & x.VardiyaId == vardiya.Id);
            StaticDegisken();
            _modulerYapiList = modulerYapiList;
            // _modulerYapi = modulerYapi;
            _istasyonList = istasyonList;
            _syBarcodeINPUTList = syBarcodeInputList;
            _syBarcodeOUTList = syBarcodeOutList;
            _sysBarcodOut = _syBarcodeOUTList?.First();
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
            this.KeyPreview = true;
            this.KeyDown += AntenKablo_KeyDown;
        }

        private void AntenKablo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }
        #region Buton
        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void AnaSayfa_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

            try
            {
                coaxCableCross = _repo.GetRepository<IzCoaxCableCross>().GetAll(x => x.Activ == true).Data;
                coaxCableConfig = _repo.GetRepository<IzCoaxCableConfig>().GetAll(x => x.Activ == true).Data;
                TarihHIM tr = TarihHesapla.TarihHesab();

                // Проверка дали _urIstasyon не е null
                if (_urIstasyon != null)
                {
                    harnesDonanimList = _repo.GetRepository<OrHarnessModel>().GetAll(x => x.FamilyId == _urIstasyon.FamilyId && x.Activ == true).Data;
                }
                else
                {
                    // Ако _urIstasyon е null, зареди всички активни Harness модели
                    harnesDonanimList = _repo.GetRepository<OrHarnessModel>().GetAll(x => x.Activ == true).Data;
                }

                // Проверка дали harnesDonanimList не е null
                if (harnesDonanimList != null && harnesDonanimList.Count > 0)
                {
                    // Добави колона ако не съществува
                    if (listReferansSec.Columns.Count == 0)
                    {
                        listReferansSec.Columns.Add("HarnessModel", "Harness Model", 280);
                    }
                    allItems = harnesDonanimCoaxList.Select(x => x.harnessModel).ToList();
                    listReferansSec.BeginUpdate();
                    // Покажи само Harness моделите, без да зареждаш Coax кабели
                    foreach (var harness in harnesDonanimList)
                    {
                        // Покажи ВСИЧКИ Harness модели
                        
                       var item = new ListViewItem(harness.HarnessModelName);
                        listReferansSec.Items.Add(item);
                        // НЕ зареждай Coax кабели тук!
                    }
                    listReferansSec.EndUpdate();
                }
                else
                {
                    // MessageBox.Show("Няма намерени Harness модели!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                //  MessageBox.Show($"Грешка при зареждане на данните: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    string selectedHarnessName = listReferansSec.Items[selectedIndex].Text.Trim();
                    label1.Text = selectedHarnessName;

                    // Намери избрания Harness модел
                    var selectedHarness = harnesDonanimList?.FirstOrDefault(x => x.HarnessModelName == selectedHarnessName);
                    if (selectedHarness != null)
                    {
                        try
                        {
                            // ЗАЯВКА КЪМ DB - зареди Coax кабели за този Harness
                            var coaxCablesForSelectedHarness = _repo.GetRepository<IzCoaxCableCross>()
                                .GetAll(x => x.HarnessModelId == selectedHarness.Id && x.Activ == true).Data;

                            // Изчисти списъка преди да го пълниш
                            harnesDonanimCoaxList.Clear();

                            // Добави Coax кабелите в списъка
                            if (coaxCablesForSelectedHarness != null)
                            {
                                foreach (var coax in coaxCablesForSelectedHarness)
                                {
                                    if (!coax.CoaxCableBarcodeId.HasValue) continue;
                                    var coaxConfig = coaxCableConfig?.FirstOrDefault(x => x.Id == coax.CoaxCableBarcodeId.Value);
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
                        catch (Exception ex)
                        {
                            //   MessageBox.Show($"Грешка при зареждане на Coax кабели: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        private async Task<bool> BarcodeBas(List<IzCoaxCableCount> listCoaxCount, HarnesDonanimCoax gl, int sayikount)
        {
            try
            {
                t = TarihHesapla.GetSystemDate();
                string formattedDateTime = $"{t.Year:D4}{t.Month:D2}{t.Day:D2}{t.Hour:D2}{t.Minute:D2}{t.Second:D2}";

                // Парсване на форматирания низ в Int64
                long idName = Int64.Parse(formattedDateTime);
                foreach (var item in listCoaxCount)
                {

                    item.CoaxTutulanId = idName;
                    idDonanimBarcode = item.CoaxTutulanId.ToString();
                    try
                    {
                        using var glContext = new UretimOtomasyonContext();
                        {
                            var result = await glContext.IzCoaxCableCounts.AddAsync(item);
                            glContext.SaveChanges();
                        }
                        // _repo.SaveChanges();
                    }
                    catch (DbException ex)
                    {
                        throw;
                    }
                }
                _sysBarcodOut.IdDonanim = _sysBarcodOut.BarcodeIcerik = idName.ToString();
                _sysBarcodOut.Leght = sayikount;

                // Проверка дали _syPrtinterList не е null и има елементи
                if (_syPrtinterList != null && _syPrtinterList.Count > 0)
                {
                    directPrintin = new DirectPrinting(_sysBarcodOut, _syPrtinterList.First(), gl);
                    directPrintin.AntenKabloBas("V");
                }

                listCoaxCount = null;
                izCoaxCableCounts = null;
                // izCoaxCableCounts = new();
                return true;
            }
            catch (Exception ex)
            {
                // MessageBox.Show($"Грешка при печатане: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                for (int i = 0; i < listAntenCableIn.Items.Count; ++i)
                {
                    sayiCount = int.Parse(numSayi.Value.ToString());
                    string lbString = listAntenCableIn.Items[i].ToString();
                    if (textBox1.Text.ToUpper().Contains(lbString))
                    {
                        try
                        {
                            if (izCoaxCableCounts == null)
                                izCoaxCableCounts = new();
                            if (harnesDonanimCoax == null)
                                harnesDonanimCoax = new();
                            if (izCoaxCableConfig == null)
                                izCoaxCableConfig = new();

                            // Проверка дали coaxCableConfig не е null
                            var coaxConfig = coaxCableConfig?.FirstOrDefault(x => x.CoaxCabloReferans == lbString);
                            if (coaxConfig == null) continue;

                            var veri = coaxConfig.Id;
                            var result = harnesDonanimCoaxList?.FirstOrDefault(x => x.harnessModel == label1.Text);
                            if (result == null) continue;

                            // Проверка дали coaxCableCross не е null
                            var coaxCross = coaxCableCross?.FirstOrDefault(x => x.CoaxCableBarcodeId == veri);
                            if (coaxCross == null) continue;

                            var coaxId = coaxCross.Id;

                            // Проверка дали _urIstasyon не е null
                            if (_urIstasyon == null) continue;

                            izCoaxCableCounts.Add(new IzCoaxCableCount
                            {
                                HarnessModelId = result.HarnesId,
                                CoaxCableName = textBox1.Text.ToUpper(),
                                CoaxCableId = coaxId,
                                UrIstasyonId = _urIstasyon.Id,
                                OrPcNameId = _urIstasyon.MashinId,
                                VardiyaId = _urIstasyon.VardiyaId,
                                Activ = true,
                                CreateDate = TarihHesapla.GetSystemDate(),
                            });
                            textBox1.Clear();
                            listAntenCabloOut.Items.Add(lbString);
                            listAntenCableIn.Items.Remove(lbString);
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
                                //Naka sira yaziyami barcoda
                                BarcodeBas(izCoaxCableCounts, harnesDonanimCoax, sayiCount);
                                foreach (var item in harnesDonanimCoaxList.Where(x => x.harnessModel == label1.Text))
                                {
                                    listAntenCableIn.Items.Add(item.BarcodeCoax);
                                }
                                listAntenCabloOut.Items.Clear();
                                izCoaxCableConfig = null; harnesDonanimCoax = null;
                                izCoaxCableCounts = null;
                                numSayi.Value = sayiCount;
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show($"Грешка при обработка на баркод: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // MessageBox.Show("Barkod yanlış", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        }

        private void btnPrintConfig_Click(object sender, EventArgs e)
        {
            VeriGetir();
        }
        #endregion

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox2.Text.Trim().ToLower();

            listReferansSec.Items.Clear();

            listReferansSec.BeginUpdate();
            // Покажи само Harness моделите, без да зареждаш Coax кабели
            foreach (var harness in harnesDonanimList.Where(x => x.HarnessModelName.ToLower().Contains(searchText)))
            {
                // Покажи ВСИЧКИ Harness модели

                var item = new ListViewItem(harness.HarnessModelName);
                listReferansSec.Items.Add(item);
                // НЕ зареждай Coax кабели тук!
            }
            listReferansSec.EndUpdate();
        }

        
    }
}
