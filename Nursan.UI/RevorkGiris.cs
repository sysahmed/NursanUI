using Nursan.Business.Manager;
using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;

namespace Nursan.UI
{
    public partial class RevorkGiris : Form
    {

        UnitOfWork tork; ErRework revorkSQL;
        string _veri;
        UretimOtomasyonContext _context = new UretimOtomasyonContext();
        public event EventHandler SystemTetikKonveyor;
        private DirectPrinting _print;
        private SyBarcodeOut _out;
        List<IzDonanimCount> _IzDonanim;
        List<UrIstasyon> _istasuonlar;
        List<UrModulerYapi> _modulerYapiList;
        UrVardiya _vardiya;
        public RevorkGiris(string veri, DirectPrinting print, SyBarcodeOut outce, List<IzDonanimCount> IzDonanim, List<UrIstasyon> istasuonlar, List<UrModulerYapi> modulerYapiList)
        {
            InitializeComponent();
            // SystemTetikKonveyor = new EventHandler;
            tork = new UnitOfWork(new UretimOtomasyonContext());
            revorkSQL = new ErRework();
            _veri = veri;
            _print = print;
            _out = outce;
            _IzDonanim = IzDonanim;
            _istasuonlar = istasuonlar;
            _modulerYapiList = modulerYapiList;
            // txtBarcodeReader.Enabled = false;
            _vardiya = new UrVardiya();
            foreach (var item in _IzDonanim)
            {
                listBox1.Items.Add(_istasuonlar.FirstOrDefault(x => x.Id == item.UrIstasyonId).Name);
            }
        }
        private void txtBarcodeReader_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _vardiya = tork.GetRepository<UrVardiya>().Get(x => x.Name == txtBarcodeReader.Text.ToUpper().Trim()).Data;
                if (_vardiya.Name.Contains("QUALITY"))
                {
                    btnReworkOut.Visible = true;
                    cbModulerYapi.Visible = true;
                }
                else
                {
                    Quality.Text = "Dogru Sicili Okutunb!!!"; Quality.ForeColor = Color.Red;
                }
                txtBarcodeReader.Clear();
            }
        }
        private void RevorkGiris_Load(object sender, EventArgs e)
        {
            foreach (var item in _modulerYapiList)
            {
                cbModulerYapi.Items.Add(item.Etap);
            }
            txtBarcodeReader.Focus();
        }
        private bool VeriSilme(string verile)
        {
            var idnot = _istasuonlar.FirstOrDefault(x => x.Name == verile.ToString());
            var systemden = _IzDonanim.FirstOrDefault(x => x.UrIstasyonId == idnot.Id);
            var veriler = _IzDonanim.Where(x => x.Id >= systemden.Id);
            foreach (var item in veriler)
            {
                tork.GetRepository<IzDonanimCount>().Delete(item.Id);
            }
            try
            {
                var coaxveriler = tork.GetRepository<IzCoaxCableCount>().GetAll(x => x.DonanimRederansId == systemden.Id).Data;
                if (coaxveriler != null)
                {
                    foreach (var coax in coaxveriler)
                    {
                        tork.GetRepository<IzCoaxCableCount>().Delete(coax.Id);
                    }
                }
            }
            catch (ErrorExceptionHandller)
            {
            }
            return true;
            //listBox1.Items.Remove(listBox1.SelectedItems[i]);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.SelectedItems.Count; i++)
            {
                Quality.Text = listBox1.SelectedItems[i].ToString();
                //VeriSilme(listBox1.SelectedItems[i].ToString());
            }
            txtBarcodeReader.Enabled = true;
            cbModulerYapi.Visible = true;
        }

        private void btnReworkOut_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbModulerYapi.Text))
            {
                _out.PsName = cbModulerYapi.Text;
                if (!string.IsNullOrEmpty(Quality.Text))
                    VeriSilme(Quality.Text);

                ErRework gelen = tork.GetRepository<ErRework>().Get(x => x.Referans == _veri).Data;
                gelen.ReworkOut = true;
                gelen.ReworkOutOperator = $"{Environment.MachineName}{_vardiya.Name}";
                gelen.ReworkOutDate = TarihHesapla.GetSystemDate();
                tork.GetRepository<ErRework>().Update(gelen);
                IzGenerateId id = tork.GetRepository<IzGenerateId>().Get(x => x.Barcode == _veri).Data;
                id.Revork = false;
                tork.GetRepository<IzGenerateId>().Update(id);
                tork.Dispose();
                SystemTetikKonveyor(this, new EventArgs());
                _print.PrintFileNew(_out);

                // Revork rvk = new Revork();
                //rvk.RevorkDoldur();
                this.Close();
                this.Close();
                _out.PsName = "";
            }
            else
            {

                MessageBox.Show("Donanimi nereye gidecegini secin!", "Quality", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Quality.Focus();
            }
        }


    }
}
