using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using Nursan.UI.Library;
using Nursan.UI.OzelClasslar;
using Nursan.Validations.SortedList;
using Nursan.Validations.ValidationCode;
using System.IO.Ports;

namespace Nursan.UI
{
    public partial class GozKontrol : Form
    {
        private static List<SyBarcodeOut> _syBarcodeOutList;
        private static List<SyPrinter> _syPrinterList;
        private static List<OrFamily> _familyList;
        private static List<UrModulerYapi> _modulerYapiList;
        SyBarcodeInput BarcodeInput = new SyBarcodeInput();
        SerialPort _serialPort;
        BarcodeValidation barcode;
        string pfbSerial;
        int brtSayi = 0;
        private static UnitOfWork _repo;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        CountDegerValidations _countDegerValidations;
        int pi; string barkodGk;
        static int brcodeVCount;
        Proveri proveri = new Proveri();
        //List<SyBarcodeInput> Barcode = new List<SyBarcodeInput>();
        TorkService tork;
        public GozKontrol(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
        {

             brtSayi = brcodeVCount;
             _modulerYapiList = modulerYapiList;
             _syBarcodeOutList = syBarcodeOutList;
             _syPrinterList = syPrinterList;
             _familyList = familyList;
            _repo = repo;
            _makine = makine;
            _vardiya = vardiya;
            _istasyonList = istasyonList;
            _syBarcodeInputList = syBarcodeInputList;

            brcodeVCount = _syBarcodeInputList.Count;
            _countDegerValidations = new CountDegerValidations(_repo, _makine, _vardiya, _istasyonList);
            Form.CheckForIllegalCrossThreadCalls = false;
            tork = new TorkService(repo, vardiya);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            GetCounts();
            StaringAP frm = new StaringAP();
            Thread.Sleep(500);
            frm.Dispose();
            frm.Close();
        }
        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_vardiya.Name != txtBarcode.Text)
                {
                    if (txtBarcode.Text.StartsWith("#"))
                        barkodGk = txtBarcode.Text.Substring(1);
                    else
                        barkodGk = txtBarcode.Text;

                    if (tork.IsAlertGkLocked(barkodGk))
                    {
                        // Вземи харнес модела за формата
                        string harnessName;
                        try
                        {
                            harnessName = StringSpanConverter.ExtractText(barkodGk.AsSpan()).ToString();
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            proveri.MessageAyarla($"Невалиден баркод: '{barkodGk}'", Color.Red, lblMessage);
                            txtBarcode.Clear();
                            return;
                        }
                        var harnessModel = _repo.GetRepository<OrHarnessModel>().Get(x => x.HarnessModelName == harnessName).Data;
                        using (var dlg = new AlertGkLockedOpen(_repo, harnessModel))
                        {

                            if (dlg.ShowDialog() != DialogResult.OK)
                            {
                                lblMessage.Text= "GK Locked не е отключена!";
                                lblMessage.ForeColor = Color.Red;
                                //proveri.MessageAyarla($"GK Locked не е отключена!", Color.Red, lblMessage);
                                //txtBarcode.Clear();
                                return;
                            }
                            else
                            {
                                lblMessage.Text = "GK Locked е отключена!";
                                lblMessage.ForeColor = Color.Lime;
                                //proveri.MessageAyarla($"GK Locked е отключена!", Color.Lime, lblMessage);
                                //listBox1.Items.Clear();
                                //txtBarcode.Clear();
                                return;
                            }
                        }
                    }

                    listBox1.Items.Add(txtBarcode.Text);
                    _syBarcodeInputList[pi].BarcodeIcerik = txtBarcode.Text;
                    if (!txtBarcode.Text.StartsWith(_syBarcodeInputList[pi].OzelChar == null ? "" : _syBarcodeInputList[pi].OzelChar))
                    {
                        proveri.MessageAyarla($"Yanlis Brcode Okudunuz!", Color.Red, lblMessage);
                        txtBarcode.Clear(); listBox1.Items.Clear(); pi = 0;
                        return;
                    }
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
                    GetCounts();
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


    }
}

