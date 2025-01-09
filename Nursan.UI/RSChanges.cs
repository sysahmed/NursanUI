using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using System.Text.RegularExpressions;

namespace Nursan.UI
{
    public partial class R_SChanges : Form
    {
        UretimOtomasyonContext Otomasyon = new UretimOtomasyonContext();
        DirectPrinting printing;
        SyPrinter printer;
        OrHarnessModel harnessModell;
        IzDonanimCount izDonanim;
        OrAlert alert;
        SyBarcodeOut sysBarcode;
        public R_SChanges()
        {
            InitializeComponent();
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string donanimReferans = textBox1.Text.ToUpper().TrimStart().TrimEnd();
                printer = GetPrinter();
                izDonanim = GetDonanim(donanimReferans);
                string[] veriler = donanimReferans.Split('-');
                string suffix = Regex.Replace(veriler[2], "[^a-z,A-Z,@,^,/,]", "");
                harnessModell = GetHarness($"{veriler[0]}-{veriler[1]}-{suffix}");
                string preffix = veriler[0];
                string family = veriler[1];
                alert = GetAlert(harnessModell.HarnessModelName);
                try
                {
                    sysBarcode = new SyBarcodeOut
                    {
                        BarcodeIcerik = $"S{donanimReferans.Substring(1)}",
                        PrinetrId = printer.Id,
                        prefix = $"S{preffix.Substring(1)}",
                        family = family,
                        suffix = suffix,
                        releace = harnessModell.Release,
                        eclntcode = harnessModell.SideCode,
                        IdDonanim = Regex.Replace(veriler[2], "[^0-9]", ""),
                        Sira1 = alert.Name,
                        PadLeft = 8

                    };
                    printing = new DirectPrinting(sysBarcode, izDonanim, printer, 1);
                    printing.PrintFileNew(sysBarcode);
                    textBox1.Clear();
                }
                catch (Exception ex)
                {
                    textBox1.Clear();
                }
            }
        }
        private OrAlert GetAlert(string? harnessModelName)
        {
            return Otomasyon.OrAlerts.FirstOrDefault(x => x.AlertNumber == harnessModell.AlertNumber);
        }
        private OrHarnessModel GetHarness(string v)
        {
            return Otomasyon.OrHarnessModels.FirstOrDefault(x => x.HarnessModelName == v);
        }
        public IzDonanimCount GetDonanim(string donanim)
        {
            return Otomasyon.IzDonanimCounts.FirstOrDefault(x => x.DonanimReferans == donanim);
        }
        public SyPrinter GetPrinter()
        {
            return Otomasyon.SyPrinters.FirstOrDefault(x => x.Id == 8);
        }
    }
}
