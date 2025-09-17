using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using Nursan.UI.Library;
using Nursan.UI.OzelClasslar;
using Nursan.Validations.ValidationCode;
using System.Text.RegularExpressions;

namespace Nursan.UI
{
    public partial class Paket : Form
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
        CountDegerValidations _countDegerValidations;
        OzelReferansControlEt ozel;
        int pi;
        TorkService tork;
        List<SyBarcodeInput> secondSyBarcodeInputList = new List<SyBarcodeInput>();
        static int brcodeVCount;
        Proveri proveri = new Proveri();
        int brtSayi = 0;
        public Paket(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
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
            InitializeComponent();
            tork = new TorkService(repo, vardiya);
            ozel = new OzelReferansControlEt(repo);
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            GetCounts();
            Staring frm = new Staring();
            Thread.Sleep(500);
            frm.Dispose();
            frm.Close();
        }

        private void txtBarcode_KeyUpAsync(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (_vardiya.Name != txtBarcode.Text)
                {
                    pictureBox1.Image = null;
                    if (txtBarcode.Text.StartsWith("#"))
                    {
                        string[] parcala = txtBarcode.Text.Substring(1).Split('-');
                        string suffix = Regex.Replace(parcala[2], "[^a-z,A-Z,@,^,/,]", "");
                        string veri = $"{parcala[0]}-{parcala[1]}-{suffix}";
                        var ozelRezult = ozel.ControletSystemi(veri, int.Parse(Regex.Replace(parcala[2], "[^0-9]", "")).ToString());
                        if (ozelRezult.Success && ozelRezult.Data != null)
                        {
                            using (MemoryStream ms = new MemoryStream(ozelRezult.Data.image))
                            {
                                pictureBox1.Image = Image.FromStream(ms);
                            }
                            txtBarcode.Clear();
                            proveri.MessageAyarla($"{ozelRezult.Message} {txtBarcode.Text} ", ozelRezult.Success != true ? Color.LightBlue : Color.Red, lblMessage);
                            listBox1.Items.Clear();
                            return;
                        }
                        else
                        {
                            lblMessage.Text = "";
                            listBox1.Items.Add(txtBarcode.Text);
                            _syBarcodeInputList[pi].BarcodeIcerik = txtBarcode.Text;
                            proveri.MessageAyarla($"Sigorta Etiketini Okuttunuz! {txtBarcode.Text}", Color.Lime, lblMessage);
                            if (!txtBarcode.Text.StartsWith(_syBarcodeInputList[pi].OzelChar == null ? "" : _syBarcodeInputList[pi].OzelChar))
                            {
                                proveri.MessageAyarla($"Yanlis Brcode Okudunuz!", Color.Red, lblMessage);
                                txtBarcode.Clear(); listBox1.Items.Clear(); pi = 0;
                                return;
                            }
                            pi++;
                            txtBarcode.Clear();
                            //GetCounts();
                        }
                    }
                    else
                    {
                        listBox1.Items.Add(txtBarcode.Text);
                        _syBarcodeInputList[pi].BarcodeIcerik = txtBarcode.Text;
                        if (_syBarcodeInputList.First().BarcodeIcerik.Contains(_syBarcodeInputList.Last().BarcodeIcerik))
                        {
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
                            //proveri.MessageAyarla($"{veri.Message} {txtBarcode.Text} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage);
                            txtBarcode.Clear();
                            GetCounts();
                        }
                        else
                        {
                            proveri.MessageAyarla($"Okutugunuz Barcod-lar Yalnis!!!", Color.Red, lblMessage);
                            for (int i = 0; i < pi; i++)
                            {
                                proveri.MessageAyarla($"Yanlis Brcode Okudunuz!", Color.Red, lblMessage);
                                txtBarcode.Clear(); listBox1.Items.Clear(); pi = 0;
                                return;
                            }
                        }
                    }
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
