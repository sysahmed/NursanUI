using Nursan.Business.Services;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Logging.Messages;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Personal.Valadation;
using Nursan.UI.Library;
using Nursan.UI.OzelClasslar;
using Nursan.Validations.ProcessServise.ProcesManager;
using Nursan.Validations.ValidationCode;
using System.IO.Ports;


namespace Nursan.UI
{
    public partial class Gromet : Form
    {
        private static UnitOfWork _repo;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        //private static List<UrModulerYapi> _modulerYapiList;
        //private static List<SyBarcodeOut> _syBarcodeOutList;
        //private static List<SyPrinter> _syPrinterList;
        //private static List<OrFamily> _familyList;
        //SyBarcodeInput BarcodeInput = new SyBarcodeInput();
        //string pfbSerial;
        //SerialPort _serialPort;
        //BarcodeValidation barcode;
        //int brtSayi = 0;
        ////List<SyBarcodeInput> Barcode = new List<SyBarcodeInput>();
        //ProcessServices process;
        private List<UrPersonalTakib> urPersonalTakibs;
        CountDegerValidations _countDegerValidations;
        int pi;
        static int brcodeVCount;
        Proveri proveri = new Proveri();
        PersonalValidasyonu personal;
        TorkService tork;
        UrIstasyon _istasyon;
        System.Windows.Forms.Timer timer;
        public Gromet(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
        {

            //_syBarcodeOutList = syBarcodeOutList;
            //_syPrinterList = syPrinterList;
            //_familyList = familyList;
            //process = new ProcessServices();
            //brtSayi = brcodeVCount;
            //_modulerYapiList = modulerYapiList;
            _repo = repo;
            _makine = makine;
            _vardiya = vardiya;
            _istasyonList = istasyonList;
            _syBarcodeInputList = syBarcodeInputList;

            brcodeVCount = _syBarcodeInputList.Count;
            _countDegerValidations = new CountDegerValidations(_repo, _makine, _vardiya, _istasyonList);
            Form.CheckForIllegalCrossThreadCalls = false;
            personal = new PersonalValidasyonu(new UnitOfWorPersonal(new PersonalContext()), _repo);
            tork = new TorkService(repo, vardiya);
            _istasyon = istasyonList.FirstOrDefault(x => x.MashinId == _makine.Id);
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 3 * 1000;
            timer.Tick += new System.EventHandler(GetTikket);
            timer.Start();
            InitializeComponent();
        }

        private void GetTikket(object? sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            await GetPDF();
            GetCounts();
            GetPersonal();
            StaringAP frm = new StaringAP();
            Thread.Sleep(500);
            frm.Dispose();
            frm.Close();
        }

        private async Task GetPDF()
        {
            if (axAcropdf1 != null)
            {
                try
                {
                    string veri = $"{Environment.CurrentDirectory}\\pdfdoc.pdf";
                    axAcropdf1.src = veri;
                    axAcropdf1.Dock = Dock;
                    axAcropdf1.setShowToolbar(false);
                    axAcropdf1.Show();
                }
                catch (Exception)

                {
                    axAcropdf1.Dispose();
                }
            }
        }

        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            DateTime date = OtherTools.GetValuesDatetime();
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.StartsWith("*"))
                {
                    try
                    {
                        string sicilPersonal = txtBarcode.Text.Substring(1);
                        var personalResult = personal.GetPersonal(sicilPersonal).Data;
                        var personalTakipResult = personal.GetPersonalAndSicilTakibTek(sicilPersonal).Data;
                        Messaglama.MessagYaz(sicilPersonal);
                        if (personalTakipResult == null)
                        {
                            Messaglama.MessagYaz(personalResult.FIRST_NAME);
                            //Messaglama.MessagYaz(personalTakipResult.FullName);
                            personal.ADDPersonalTakib(new UrPersonalTakib
                            {
                                Sicil = sicilPersonal,
                                FullName = $"{personalResult.FIRST_NAME} {personalResult.LAST_NAME}",
                                UrIstasyonId = _istasyon.Id,
                                DayOfYear = $"{_istasyon.Id}*{date.Year}{date.Month}{date.Day}",
                                CreateDate = date,
                                UpdateDate = date
                            });
                        }
                        else if (personalTakipResult != null && personalTakipResult.UrIstasyonId != _istasyon.Id)
                        {
                            Messaglama.MessagYaz(personalTakipResult.FullName + "H");
                            personal.UpdatePersonalTakib(new UrPersonalTakib
                            {
                                Id = personalTakipResult.Id,
                                Sicil = sicilPersonal,
                                FullName = $"{personalResult.FIRST_NAME} {personalResult.LAST_NAME}",
                                UrIstasyonId = _istasyon.Id,
                                DayOfYear = $"{_istasyon.Id}*{date.Year}{date.Month}{date.Day}",
                                UpdateDate = date
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (txtBarcode.Text.Length != 10)
                {
                    if (_vardiya.Name != txtBarcode.Text)
                    {
                        listBox1.Items.Add(txtBarcode.Text);
                        _syBarcodeInputList[pi].BarcodeIcerik = txtBarcode.Text;
                        if (!txtBarcode.Text.StartsWith(_syBarcodeInputList[pi].OzelChar == null ? "" : _syBarcodeInputList[pi].OzelChar))
                        {
                            proveri.MessageAyarla($"Yanlis Brcode Okudunuz!", Color.Red, lblMessage);
                            txtBarcode.Clear(); listBox1.Items.Clear(); pi = 0;
                            return;
                        }
                        pi++;
                        if (_syBarcodeInputList.Count == pi)
                        {
                            var veri = tork.GetTorkDonanimBarcode(_syBarcodeInputList);
                            listBox1.Items.Clear();
                            for (int i = 0; i < pi; i++)
                            {
                                _syBarcodeInputList[i].BarcodeIcerik = null;
                                pi = 0;
                            }
                            proveri.MessageAyarla($"{veri.Message} {txtBarcode.Text} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage);
                        }
                        //proveri.MessageAyarla($"{veri.Message} {txtBarcode.Text} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage);
                        txtBarcode.Clear();
                        GetCounts(); GetPersonal();
                    }
                    else
                    {
                        SicilOkumaAP sicil = new SicilOkumaAP(_repo, txtBarcode.Text);
                        sicil.ShowDialog(); this.Hide();
                        //Thread.Sleep(10000);
                        this.Dispose();
                        this.Close();
                    }
                }
            }


        }

        private void GetPersonal()
        {
            listBox2.Items.Clear();
            urPersonalTakibs = personal.GetPersonalTakib(_istasyon).Data;
            var veriler = urPersonalTakibs.Count() == 0 ? null : urPersonalTakibs;
            Messaglama.MessagYaz(_istasyon.Id.ToString());
            if (veriler != null)
            {
                //string[] parca = veriler.First().DayOfYear.Split('*');
                foreach (var item in urPersonalTakibs)
                {
                    //Messaglama.MessagYaz($"{item.Sicil}-{item.FullName}-{GitSytemDeAyiklaVesay(item.Sicil)}");
                    listBox2.Items.Add($"{item.Sicil}-{item.FullName}-{GitSytemDeAyiklaVesay(item.Sicil)}");
                }
            }
        }
        int ortalamaCount;
        int vardiyaCount;
        int toplamCount;
        private void GetCounts()
        {

            Label[] lable;
            lable = new Label[9];
            lable[0] = label1;
            lable[1] = lblVardiya; lable[2] = lblToplama; lable[3] = lblOrtalama; lable[4] = label3; lable[5] = label5; lable[6] = label7; lable[7] = label8; lable[8] = label9;

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
        private int GitSytemDeAyiklaVesay(string? sicil)
        {
            var result = SayiIzlemeSIcilBagizliService.SayiHesapla(sicil, _vardiya.Name);
            return ((int)_istasyonList.First().Realadet + result);
        }

    }
}