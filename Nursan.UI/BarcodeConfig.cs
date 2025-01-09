using Nursan.Domain.Entity;
using Nursan.Persistanse.Extensions;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;

namespace Nursan.UI
{
    public partial class BarcodeConfig : Form
    {
        SyBarcodeInput barcein;
        SyBarcodeOut barceout;
        BarcodeValidation valide;
        PrinterManager print;
        List<SyPrinter> printerce;

        private readonly IConfigurationFactory _configurationFactory;
        public BarcodeConfig()
        {
            //_configurationFactory = configurationFactory;
            barcein = new SyBarcodeInput();
            barceout = new SyBarcodeOut();
            print = new PrinterManager(new UnitOfWork(new UretimOtomasyonContext()));
            printerce = new List<SyPrinter>();
            valide = new BarcodeValidation(new UnitOfWork(new UretimOtomasyonContext()), new IzDonanimCount());
            InitializeComponent();
        }

        private void BarcodeConfig_Load(object sender, EventArgs e)
        {
            Loading();
        }
        public void Loading()
        {
            var modelin = valide.GetBarcodeInput();
            var modelout = valide.GetBarcodeOut();
            if (modelin != null)
            {
                foreach (var item in modelin)
                {
                    listBarcode.Items.Add(item.Name);
                }
                printerce = print.GetAllPrinter();
                foreach (var item in printerce)
                {
                    cbPrinter1.Items.Add(item.Name);
                }

            }
            if (modelout != null)
            {
                foreach (var item in modelout)
                {
                    listBarcodeCikis.Items.Add(item.Name);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (cbBrcodeSec.Text != "")
            {
                if (cbBrcodeSec.Text == "Okutma Barcode Sakla")
                {
                    barcein.Name = txtBarcodeismi.Text;
                    barcein.PrinterId = printerce.FirstOrDefault(x => x.Name == cbPrinter1.Text).Id;
                    barcein.Leght = Convert.ToInt16(ndUnunluk.Value);
                    barcein.ParcalamaChar = Convert.ToChar(txtParcalamaCar.Text);
                    barcein.PadLeft = Convert.ToInt16(ndPadlef.Value);
                    barcein.StartingSubstring = Convert.ToInt16(ndStartingSutring.Value);
                    barcein.StopingSubstring = Convert.ToInt16(ndStopimgSubstring.Value);
                    barcein.RegexInt = txtregexInt.Text;
                    barcein.RegexString = txtRegexString.Text;

                    valide.AddBarcodeIn(barcein);
                    Temizle();
                }
                else if (cbBrcodeSec.Text == "Basma Barcode Sakla")
                {
                    barceout.Name = txtBarcodeismi.Text;
                    try
                    {
                        barceout.PrinetrId = printerce.FirstOrDefault(x => x.Name == cbPrinter1.Text).Id;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Yazici Sec!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }
                    barceout.Leght = Convert.ToInt16(ndUnunluk.Value);
                    try
                    {
                        barceout.ParcalamaChar = Convert.ToChar(txtParcalamaCar.Text);
                    }
                    catch (Exception)
                    {
                        barceout.ParcalamaChar = '@';
                    }
                    barceout.PadLeft = Convert.ToInt16(ndPadlef.Value);
                    barceout.StartingSubstring = Convert.ToInt16(ndStartingSutring.Value);
                    barceout.StopingSubstring = Convert.ToInt16(ndStopimgSubstring.Value);
                    barceout.RegexInt = txtregexInt.Text;
                    barceout.RegexString = txtRegexString.Text;
                    valide.AddBarcodeOut(barceout);
                    Temizle();
                }
            }

        }
        void Temizle()
        {
            txtBarcodeismi.Clear();
            txtParcalamaCar.Clear();
            txtregexInt.Clear();
            txtRegexString.Clear();
            cbPrinter1.Items.Clear();
            ndPadlef.Value = 0;
            ndStartingSutring.Value = 0;
            ndStopimgSubstring.Value = 0;
            ndUnunluk.Value = 0;
            Loading();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            var result = print.GetPrinter(cbPrinter1.Text);
            if (!string.IsNullOrEmpty(cbPrinter1.Text) && result == null)
            {
                print.AddPrinter(new SyPrinter { Name = cbPrinter1.Text });
                MessageBox.Show("Yeni Yazici Tanittiniz!", "OK", icon: MessageBoxIcon.Information, buttons: MessageBoxButtons.OKCancel);
                cbPrinter1.Text = ""; Loading();
            }
            else
            {
                MessageBox.Show("Yeni Yazici Tanittiniz! Boyle bir yazici mevcut!", "Hata", icon: MessageBoxIcon.Error, buttons: MessageBoxButtons.OKCancel);
                cbPrinter1.Text = "";
            }
        }
    }
}
