using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.UI.Library;
using Nursan.UI.OzelClasslar;
using Nursan.Validations.Interface;
using Nursan.Validations.Opsionlar;
using Nursan.Validations.ValidationCode;
using System.IO.Ports;

namespace Nursan.UI
{
    public partial class Tork : Form
    {

        private readonly UnitOfWork _repo;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<UrModulerYapi> _modulerYapiList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        private static List<SyBarcodeOut> _syBarcodeOutList;
        private static List<SyPrinter> _syPrinterList;
        private static List<OrFamily> _familyList;
        private static IzGenerateId _id;
        private static OrHarnessConfig _config;
        private static TorkAktar _torkaAktar;
        string pfbSerial;
        SerialPort _serialPort;
        BarcodeValidation barcode;
        CountDegerValidations _countDegerValidations;
        int pi;
        SyBarcodeInput BarcodeInput = new SyBarcodeInput();
        static int brcodeVCount;
        Proveri proveri = new Proveri();
        int brtSayi = 0;
        List<SyBarcodeInput> Barcode = new List<SyBarcodeInput>();
        private string buffer { get; set; }
        TorkService tork;// = new TorkService();
        public static int h1; public static int w1;
        ITorkManager _torkmanager;
        IHarnesConfigServices _harnessConfig;
        private SpcCalculator _spcCalculator; // CPK функционалност

        // Болтов процес - локално състояние (без да пипаме текущия поток)
        private class BoltStep
        {
            public int StepIndex { get; set; }
            public int RoiX { get; set; }
            public int RoiY { get; set; }
            public int RoiWidth { get; set; }
            public int RoiHeight { get; set; }
        }

        private class BoltSession
        {
            public string HarnessModelName { get; set; }
            public List<BoltStep> Steps { get; set; }
            public int CurrentBoltIndex { get; set; }
            public long? UretimId { get; set; }
        }

        private BoltSession _boltSession;
        public Tork(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
        {
            _repo = repo;
            _makine = makine;
            _vardiya = vardiya;
            _modulerYapiList = modulerYapiList;
            _istasyonList = istasyonList;
            _syBarcodeInputList = syBarcodeInputList;
            _syBarcodeOutList = syBarcodeOutList;
            _syPrinterList = syPrinterList;
            _familyList = familyList;
            //BarcodeInput = _syBarcodeInputList.FirstOrDefault(x=>x.);
            brcodeVCount = _syBarcodeInputList.Count;
            brtSayi = brcodeVCount;
            _countDegerValidations = new CountDegerValidations(_repo, _makine, _vardiya, _istasyonList);
            barcode = new BarcodeValidation(_repo, _makine, _vardiya, _istasyonList, _modulerYapiList, _syBarcodeInputList, _syBarcodeOutList, _syPrinterList, _familyList);
            _serialPort = new SerialPort();
            this._serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_DataReceived);
            Form.CheckForIllegalCrossThreadCalls = false;
            tork = new TorkService(repo, vardiya);
            _torkmanager = new TorkKonfigService(_repo);
            _harnessConfig = new HarnessConfigManager(_repo);
            _torkaAktar = new TorkAktar(new Domain.TORKS.NursandatabaseContext(), repo);
            InitializeComponent();
        }

        private void StartBoltSession(string harnessModelName, int urIstasyonId, long? uretimId)
        {
            _boltSession = new BoltSession
            {
                HarnessModelName = harnessModelName,
                Steps = LoadBoltSteps(harnessModelName),
                CurrentBoltIndex = 1,
                UretimId = uretimId
            };
            if (_boltSession.Steps == null || _boltSession.Steps.Count == 0)
            {
                _boltSession.Steps = CreateDefaultThreeSteps();
            }
            ShowBoltHint(_boltSession.CurrentBoltIndex, _boltSession.Steps.Count);
        }

        private List<BoltStep> LoadBoltSteps(string harnessModelName)
        {
            // За сега: без директно четене – връщаме празно и падаме към 3 по подразбиране
            return new List<BoltStep>();
        }

        private List<BoltStep> CreateDefaultThreeSteps()
        {
            return new List<BoltStep>
            {
                new BoltStep{ StepIndex = 1 },
                new BoltStep{ StepIndex = 2 },
                new BoltStep{ StepIndex = 3 }
            };
        }

        private void ShowBoltHint(int current, int total)
        {
            try
            {
                if (lbl2Massage != null)
                {
                    lbl2Massage.Text = $"Стегни болт {current} / {total}";
                    lbl2Massage.ForeColor = Color.Yellow;
                }
            }
            catch { }
        }

        private void OnBoltOk()
        {
            if (_boltSession == null) return;
            if (_boltSession.CurrentBoltIndex < _boltSession.Steps.Count)
            {
                _boltSession.CurrentBoltIndex++;
                ShowBoltHint(_boltSession.CurrentBoltIndex, _boltSession.Steps.Count);
            }
        }

        private void OnProcessFinished(bool success)
        {
            _boltSession = null;
            try
            {
                if (lbl2Massage != null)
                {
                    lbl2Massage.Text = success ? "Процес приключи" : "Процес отменен";
                    lbl2Massage.ForeColor = success ? Color.Lime : Color.Red;
                }
            }
            catch { }
        }
        private async Task GitAktar()
        {
            await _torkaAktar.ExecuteTorkAktar();
        }
        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            buffer = buffer + _serialPort.ReadExisting();
            if (buffer.Length != 0) //substring(buffer,buffer-2,1)='\n'
            {
                if (true)
                {
                    GelenVeriDegerle(buffer.Substring(0, 1));
                }
            }
            buffer = "";
        }
        private void GelenVeriDegerle(string veri)
        {
            Messaglama.MessagYaz(veri);
            switch (veri)
            {
                case "1":
                    OnBoltOk();
                    break;
                case "2":
                    plc_StopWOrk(Barcode);
                    Barcode.Clear();
                    OnProcessFinished(true);
                    GetBarcodeInput(); break;
                case "3":
                    plc_Zanulqvane(); OnProcessFinished(false); GetBarcodeInput(); break;
                case "4":
                    plc_Reset(); OnProcessFinished(false); GetBarcodeInput(); break;
                case "5":
                    base.WindowState = FormWindowState.Maximized;
                    plc_Reset(); OnProcessFinished(false); GetBarcodeInput(); break;
                case "6":
                    base.WindowState = FormWindowState.Maximized;
                    plc_StopWOrk(Barcode); OnProcessFinished(true); break;

                default: break;

            }
        }
        public void plc_Reset()
        {
            _serialPort.WriteLine("3");
            listBox1.Items.Clear();
            txtBarcode.Clear(); Barcode.Clear();
            Thread newmetod = new Thread(() =>
            {
                BeginInvoke((Action)delegate ()
                {
                    lblMessage.Text = "Lutfen Yeni Donanim Takin \n\r Ve Barkodlarini Okutun!!!"; lblMessage.ForeColor = Color.Lime;
                });

            });
            newmetod.Start();
        }
        private void plc_StopWOrk(IEnumerable<SyBarcodeInput> glnBarcode)//systeme kayit yap!
        {
            base.WindowState = FormWindowState.Maximized;
            this.Activate();
            lbl2Massage.Text = "";
            Result result = tork.GitBarcodeBas(glnBarcode);
            if (result.Success == true)
            {
                tork.SystemePFBKayitYap(glnBarcode, pfbSerial);
                MessageGonder("System Verileri Kayit Etti \n\r Lutfen Kutulari Cikarin!", Color.Lime);
                for (int i = 0; i < _syBarcodeInputList.Count; i++)
                {
                    _syBarcodeInputList[i].BarcodeIcerik = null;
                }
                GetCounts();
                plc_Zanulqvane();
            }
            else
            {
                for (int i = 0; i < _syBarcodeInputList.Count; i++)
                {
                    _syBarcodeInputList[i].BarcodeIcerik = null;
                }
                plc_Zanulqvane();
                lblMessage.Text = result.Message;
            }
            // MessageGonder("System Verileri SQL Server-e AKTARAMADI!", Color.Red);
        }
        public void MessageGonder(string message, Color boq)
        {
            //lblHata.Dispose();
            try
            {
                lblMessage.Text = message.ToString();
                lblMessage.ForeColor = boq;
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz(ex.Message);
            }
        }
        private void plc_Zanulqvane()
        {
            _serialPort.WriteLine("3");
            listBox1.Items.Clear();
            Barcode.Clear();
            txtBarcode.Clear();

        }
        private void seri_port_baglan(string com)
        {
            if (_serialPort.IsOpen) // Bağlantıyı açıyoruz.eğer önceden bağlan butonuna basmış isek yani bağlantıyı açmışsak aşağıdaki hata mesajını verecektir.
            {
                return;
            }
            else
            {
                try
                {
                    _serialPort.BaudRate = int.Parse("9600"); // Hız olarak 9600 verdik.
                    _serialPort.DataBits = int.Parse("8"); // Veri bit ini de 8 bit olarak verdik
                    _serialPort.StopBits = System.IO.Ports.StopBits.One; // Durma bitini tek sefer olarak verdik.
                    _serialPort.Parity = Parity.None; // eşlik bit ini vermedik.
                    _serialPort.PortName = com; // Port adlarını comboboxtan alıyoruz.
                    _serialPort.Open(); // Bağlantıyı açıyoruz
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
            h1 = Height;
            w1 = Width;
            seri_port_baglan(XMLIslemi.XmlPLSOku());
            GetCounts();// Tetikle();
            Staring frm = new Staring();
            Thread.Sleep(500);
            frm.Dispose();
            frm.Close();
        }
        public async Task<IEnumerable<SyBarcodeInput>> GetBarcodeInput()
        {
            try
            {
                var istasyon = _istasyonList.FirstOrDefault(x => x.MashinId == _makine.Id && x.VardiyaId == _vardiya.Id);
                var crosTable = _repo.GetRepository<SyBarcodeInCrossIstasyon>().GetAll(x => x.UrIstasyonId == istasyon.Id).Data;
                _syBarcodeInputList = new();
                foreach (var item in crosTable)
                {
                    _syBarcodeInputList.Add(_repo.GetRepository<SyBarcodeInput>().Get(x => x.Id == item.SysBarcodeInId).Data);
                }
                await GitAktar();
                return _syBarcodeInputList;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private void txtBarcode_KeyUpAsync(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string veriler = txtBarcode.Text.ToUpper().TrimEnd().TrimStart();
                ReadOnlySpan<char> input = veriler;
                BarcodeInput.BarcodeIcerik = input.Slice(1).ToString();
                if (!txtBarcode.Text.StartsWith(_syBarcodeInputList.FirstOrDefault(x => x.Name.Equals("First", StringComparison.OrdinalIgnoreCase)).OzelChar))
                {
                    proveri.MessageAyarla($"Yanlis Donanim Okudunuz!", Color.Red, lblMessage);
                    txtBarcode.Clear(); listBox1.Items.Clear(); pi = 0;
                    return;
                }
                if (_syBarcodeInputList[pi].Name == "First")
                {

                    _id = _torkmanager.GetTorkConfigId(BarcodeInput).Data;
                    _config = _harnessConfig.Get(x => x.OrHarnessModelId == _id.HarnesModelId).Data;
                    if (_config != null)
                    {

                        listBox1.Items.Add(veriler);
                        Barcode.Add(_syBarcodeInputList[pi]);
                        _syBarcodeInputList[pi].BarcodeIcerik = veriler;
                        proveri.MessageAyarla($"Doanim Barcodunu Okuttunuz!!! {veriler} ", Color.Lime, lblMessage); txtBarcode.Clear(); pi++;
                        return;
                    }
                    else
                    {
                        proveri.MessageAyarla($"Hatali Barkod Okutunuz!!! {veriler} ", Color.Red, lblMessage); listBox1.Items.Clear(); txtBarcode.Clear(); pi = 0;
                    }
                }
                if (_id != null)
                {

                    if (_syBarcodeInputList[pi].Name == "PFBRef")
                    {

                        _syBarcodeInputList[pi].BarcodeIcerik = veriler;

                        if (_config == null)
                        {
                            proveri.MessageAyarla($"Boyle Bir Config Yok!! {veriler} ", Color.Red, lblMessage); txtBarcode.Clear();
                            listBox1.Items.Clear(); pi = 0;
                            _id = null;

                        }
                        else
                        {
                            _syBarcodeInputList[pi].BarcodeIcerik = veriler;
                            listBox1.Items.Add(veriler);
                            proveri.MessageAyarla($"{_syBarcodeInputList[pi].Name} Okuttunuz!!! {txtBarcode.Text} ", Color.Lime, lblMessage); txtBarcode.Clear();
                            pi++;
                            return;
                        }
                    }

                    if (_syBarcodeInputList[pi].Name == "PFBRefSerial")
                    {
                        pfbSerial = veriler;
                        _syBarcodeInputList[pi].BarcodeIcerik = veriler;
                        pi++;
                    }
                    txtBarcode.Clear();
                    if (_syBarcodeInputList.Count == pi)
                    {
                        pi = 0;
                        var veri = tork.GetTorkDonanimBarcode(_syBarcodeInputList);
                        if (veri.Success)
                        {
                            this.BackColor = Color.Black;
                            seri_port_baglan(XMLIslemi.XmlPLSOku());
                            _serialPort.WriteLine(veri.Message); pi = 0;

                            StartPosition = FormStartPosition.Manual;
                            WindowState = FormWindowState.Normal;
                            Size = new System.Drawing.Size(w1, 220);
                            Location = new Point(0, h1 - 250);

                            proveri.MessageAyarla($"Bilgiler Torka Gonderildi {veri.Message} {veriler} ", veri.Success == true ? Color.LightBlue : Color.Red, lbl2Massage); listBox1.Items.Clear();
                            lblMessage.Text = "";
                            listBox1.Items.Clear();
                            pi = 0;

                            // Старт на bolt-сесията (без промяна на основния поток)
                            try
                            {
                                var istasyon = _istasyonList.FirstOrDefault(x => x.MashinId == _makine.Id && x.VardiyaId == _vardiya.Id);
                                string harnessModelName = null;
                                if (_id != null && _id.HarnesModelId.HasValue)
                                {
                                    var hm = _repo.GetRepository<OrHarnessModel>().Get(x => x.Id == _id.HarnesModelId.Value).Data;
                                    harnessModelName = hm?.HarnessModelName;
                                }
                                if (!string.IsNullOrWhiteSpace(harnessModelName))
                                {
                                    StartBoltSession(harnessModelName, istasyon?.Id ?? 0, null);
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            GetBarcodeInput();
                            proveri.MessageAyarla($"{veri.Message} {veriler} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage); listBox1.Items.Clear();
                            Barcode.Clear();
                        }
                    }
                    txtBarcode.Clear();
                    // _id = null;
                }
                else
                {
                    _id = null;
                    pi = 0;
                    proveri.MessageAyarla($"Hata Olustu Yanlis Barcode Okutuunuz!!! {veriler} ", Color.Red, lbl2Massage); listBox1.Items.Clear();
                    listBox1.Items.Clear(); txtBarcode.Clear();
                }

            }
        }
        int ortalamaCount;
        int vardiyaCount;
        int toplamCount;
        private void GetCounts()
        {

            Label[] lable;
            lable = new Label[10];
            lable[0] = label1;
            lable[1] = lblVardiya; lable[2] = lblToplama; lable[4] = lblOrtalama; lable[5] = label3; lable[6] = label5; lable[7] = label7; lable[8] = label8; lable[9] = label9;

            if (Domain.SystemClass.XMLSeverIp.SayiGoster())
            {
                foreach (Label l in lable)
                {
                    if (l != null)
                        SayiGoster.LabellerVisibleYap(l);
                }
                _countDegerValidations.Hesapla(out ortalamaCount, out vardiyaCount, out toplamCount);
                lblOrtalama.Text = ortalamaCount.ToString();
                lblToplama.Text = toplamCount.ToString();
                lblVardiya.Text = vardiyaCount.ToString();
            }
            else
            {
                foreach (Label l in lable)
                {
                    if (l != null)
                        SayiGoster.LabellerNonVisibleYap(l);
                }
            }

        }
        //private void Tetikle()
        //{
        //    listBox1.Items.Add(txtBarcode.Text);
        //    _syBarcodeInputList[0].BarcodeIcerik = "#R2X6-14290-BBPB00000201";
        //    _syBarcodeInputList[1].BarcodeIcerik = "H1BT-14A094-AG";
        //    _syBarcodeInputList[2].BarcodeIcerik = "1234567";
        //    var veri = tork.GetTorkDonanimBarcode(_syBarcodeInputList); proveri.MessageAyarla($"{veri.Message} {txtBarcode.Text.ToUpper().TrimEnd().TrimStart()} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage);
        //    seri_port_baglan(XMLIslemi.XmlPLSOku());
        //    _serialPort.WriteLine(veri.Message);
        //    Barcode.Add(_syBarcodeInputList[0]);
        //}

        //private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        //{
        //    //Tetikle();
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    // Tetikle();
        //}

        private async void lblToplam_Click(object sender, EventArgs e)
        {
            await GitAktar();
        }
        
        /// <summary>
        /// CPK изчисляване бутон
        /// </summary>
        private void btnCPK_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка за null референции
                if (_spcCalculator == null)
                {
                    // Инициализирай SpcCalculator ако не е инициализиран
                    string connectionString = "Server=localhost;Database=NursanDatabase;Trusted_Connection=true;TrustServerCertificate=true;";
                    _spcCalculator = new SpcCalculator(connectionString, "TorkData", listViewCPK);
                }

                if (listViewCPK == null)
                {
                    throw new InvalidOperationException("ListView не е намерен");
                }

                // Скрий другите елементи и покажи CPK ListView
                if (lbl2Massage != null) lbl2Massage.Visible = false;
                listViewCPK.Visible = true;
                
                // Изчисти ListView преди изчисляване
                listViewCPK.Items.Clear();
                
                // Добави заглавие
                var headerItem = new ListViewItem("=== CPK HESAPLAMA ===");
                listViewCPK.Items.Add(headerItem);
                
                var emptyItem = new ListViewItem("");
                listViewCPK.Items.Add(emptyItem);
                
                // Изчисли CPK стойностите
                _spcCalculator.CalculateCpK();
                
                // Покажи съобщение за успех
                if (lblMessage != null)
                {
                    lblMessage.Text = "CPK hesaplama tamamlandi!";
                    lblMessage.ForeColor = Color.Lime;
                }
            }
            catch (Exception ex)
            {
                // Покажи грешка
                if (lblMessage != null)
                {
                    lblMessage.Text = $"CPK hesaplama hatasi: {ex.Message}";
                    lblMessage.ForeColor = Color.Red;
                }
                
                // Добави грешката в ListView
                if (listViewCPK != null)
                {
                    var errorItem = new ListViewItem($"HATA: {ex.Message}");
                    listViewCPK.Items.Add(errorItem);
                }
                
                // Логвай грешката
                Console.WriteLine($"CPK Error: {ex}");
            }
        }
    }
}