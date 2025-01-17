using Nursan.Business.Services;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Personal.Valadation;
using Nursan.UI.Library;
using Nursan.UI.OzelClasslar;
using Nursan.Validations.ValidationCode;

namespace Nursan.UI
{
    public partial class Konveyor : Form
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
        BarcodeValidation barcode;
        TorkService tork;
        PersonalValidasyonu personal;
        CountDegerValidations _countDegerValidations;
        int pi;
        List<SyBarcodeInput> secondSyBarcodeInputList = new List<SyBarcodeInput>();
        static int brcodeVCount;
        Proveri proveri = new Proveri();
        int brtSayi = 0;
        public Konveyor(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
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
            brcodeVCount = _syBarcodeInputList.Count;
            brtSayi = brcodeVCount;
            _countDegerValidations = new CountDegerValidations(_repo, _makine, _vardiya, _istasyonList);
            personal = new PersonalValidasyonu(new UnitOfWorPersonal(new PersonalContext()), _repo);
            tork = new TorkService(repo, vardiya);


            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GetCounts();
            Staring frm = new Staring();
            Thread.Sleep(500);
            frm.Dispose();
            frm.Close();
        }
        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (txtBarcode.Text.Length != 10)
                {
                    listBox1.Items.Add(txtBarcode.Text);
                    secondSyBarcodeInputList = _syBarcodeInputList;
                    _syBarcodeInputList[pi].BarcodeIcerik = txtBarcode.Text;
                    if (!txtBarcode.Text.StartsWith(_syBarcodeInputList[pi].OzelChar == null ? "" : _syBarcodeInputList[pi].OzelChar))
                    {
                        proveri.MessageAyarla($"Yanlis Barcode Okudunuz!", Color.Red, lblMessage);
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
                }
                else
                {
                    var p = personal.GetPersonalGromCardID(txtBarcode.Text).Data;
                    var pt = personal.GetPersonalAndSicilTakibTek(p.USER_CODE).Data; DateTime date = OtherTools.GetValuesDatetime();
                    var istasyon = _istasyonList.FirstOrDefault(x => x.MashinId == _makine.Id);
                    if (pt != null && p != null)
                    {
                        pt.UrIstasyonId = _istasyonList.FirstOrDefault(x => x.MashinId == _makine.Id).Id;
                        pt.UpdateDate = date;
                        personal.UpdatePersonalTakib(pt);
                        proveri.MessageAyarla($"{p.FIRST_NAME} Istasyona Dahil Oldunuz!", Color.Lime, lblMessage);

                    }
                    else if (pt == null && p != null)
                    {
                        personal.ADDPersonalTakib(new UrPersonalTakib
                        {
                            Sicil = p.USER_CODE,
                            FullName = $"{p.FIRST_NAME} {p.LAST_NAME}",
                            Department = p.DEPARTMENT,
                            UrIstasyonId = istasyon.Id,
                            DayOfYear = $"{istasyon.Id}*{date.Year}{date.Month}{date.Day}",
                            CreateDate = date,
                            UpdateDate = date
                        });
                        proveri.MessageAyarla($"{p.FIRST_NAME} {p.LAST_NAME} Istasyona Dahil Oldunuz!", Color.Lime, lblMessage);
                    }
                    else
                    {
                        proveri.MessageAyarla($"Boyle Bir Card {txtBarcode.Text} Sistemde Yok", Color.Red, lblMessage);
                    }
                }
                //proveri.MessageAyarla($"{veri.Message} {txtBarcode.Text} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage);
                txtBarcode.Clear();
                GetCounts();
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

        private void Konveyor_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Konveyor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
