using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Persistanse.Repository;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.UI.Library;
using Nursan.UI.OzelClasslar;
using Nursan.Validations.SortedList;
using Nursan.Validations.ValidationCode;
using System.IO.Ports;

namespace Nursan.UI
{
    public partial class Revork : Form
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
        string pfbSerial;


        private readonly IRepositoryAmabar<ErRework> _revork;
        SerialPort _serialPort;
        BarcodeValidation barcode;
        CountDegerValidations _countDegerValidations;
        int pi;
        SyBarcodeOut BarcodeOut = new SyBarcodeOut();
        static int brcodeVCount;
        Proveri proveri = new Proveri();
        int brtSayi = 0;
        List<SyBarcodeInput> Barcode = new List<SyBarcodeInput>();
        TorkService tork; ErRework revorkSQL;
        List<ErRework> revorkSQLList;
        List<IzDonanimCount> izDonanimCountList;
        DirectPrinting direct;
        public Revork(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
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
            //BarcodeInput = _syBarcodeInputList.FirstOrDefault();
            BarcodeOut = _syBarcodeOutList.FirstOrDefault();
            brcodeVCount = _syBarcodeInputList.Count;
            brtSayi = brcodeVCount;
            _countDegerValidations = new CountDegerValidations(_repo, _makine, _vardiya, _istasyonList);
            Form.CheckForIllegalCrossThreadCalls = false;
            tork = new TorkService(repo, vardiya);
            IzGenerateId GitGetirIddonainm;
            InitializeComponent();
        }
        public Revork()
        {
            InitializeComponent();
        }
        string veri;
        string[] veri1;
        private void txtBarcodeReader_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcodeReader.Text.Substring(0, 1) == "#")
                {
                    try
                    {
                        try
                        {
                            revorkSQL = _repo.GetRepository<ErRework>().Get(x => x.Referans == txtBarcodeReader.Text.Substring(1) && x.ReworkOut == true).Data;
                        }
                        catch (Exception ex)
                        {
                        }
                        BarcodeOut.BarcodeIcerik = txtBarcodeReader.Text.Substring(1);
                        var valuesRevork = tork.GitYaziciDegiskenParcalama(BarcodeOut);
                        var erRework = new ErRework
                        {
                            IdDonanim = int.Parse(valuesRevork.IdDonanim),
                            IstasyonTarihi = TarihHesapla.GetSystemDate(),
                            Istasyon = Environment.MachineName,
                            Referans = txtBarcodeReader.Text.Substring(1),
                            ReworkInDate = TarihHesapla.GetSystemDate(),
                            ReworkInOperator = _vardiya.Name,
                            Activ = true

                        };
                        veri = txtBarcodeReader.Text.Substring(1);
                        IzGenerateId GitGetirIddonainm = GetIzDonanimIDGeneraciq(veri).Data.First();
                        if (revorkSQL != null)
                        {
                            if (revorkSQL.Activ == true)
                            {
                                var dialogResult = MessageBox.Show("Donanim Systemde Zaten Var!\n\r Tekrar Kayit Etmek Istermisiniz!", "Dikkat", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    var revorkid1 = RevorkGiris(erRework, veri);
                                    BarcodeOut.Name = "Giris";
                                    BarcodeOut.BarcodeIcerik = $"IN_{veri}_{revorkid1.Data.Id}";
                                    direct = new DirectPrinting(BarcodeOut, GitGetirIddonainm, _syPrinterList.First());
                                    direct.PrintFileNew(BarcodeOut);
                                }
                                else if (dialogResult == DialogResult.No)
                                {
                                    MessageBoxLbl.MessageBas("Donanim Systemde Zaten Var!", Color.Red, lblMessage);
                                }
                            }
                            else
                            {
                                var revorkid2 = RevorkGiris(erRework, veri);
                                BarcodeOut.Name = "Giris";
                                BarcodeOut.BarcodeIcerik = $"IN_{veri}_{revorkid2.Data.Id}";
                                direct = new DirectPrinting(BarcodeOut, GitGetirIddonainm, _syPrinterList.First());
                                direct.PrintFileNew(BarcodeOut);
                            }
                        }
                        else
                        {
                            var revorkid3 = RevorkGiris(erRework, veri);
                            BarcodeOut.Name = "Giris";
                            BarcodeOut.BarcodeIcerik = $"IN_{veri}_{revorkid3.Data.Id}";
                            direct = new DirectPrinting(BarcodeOut, GitGetirIddonainm, _syPrinterList.First());
                            direct.PrintFileNew(BarcodeOut);
                        }
                        txtBarcodeReader.Clear();
                        txtFaultName.Focus();
                    }
                    catch (Exception ex)
                    {
                        MessageBoxLbl.MessageBas($"Hata {ex.Message}", Color.Red, lblMessage); txtBarcodeReader.Clear();

                    }
                }
                else if (txtBarcodeReader.Text.ToUpper().Trim().Split('_').First() == "IN")
                {
                    lblIdDonanim.Text = txtBarcodeReader.Text;
                    txtFaultName.Enabled = true;
                    txtBarcodeReader.Clear();
                    SendKeys.Send("{TAB}");
                }
                else if (txtBarcodeReader.Text.ToUpper().Trim().Split('_').First() == "FIX")
                {
                    veri1 = txtBarcodeReader.Text.ToUpper().Trim().Split('_');
                    BarcodeOut.BarcodeIcerik = $"OUT_{veri1[1]}_{veri1[2]}";
                    BarcodeOut.Name = "Onay";
                    IzGenerateId GitGetirIddonainm = GetIzDonanimIDGeneraciq(veri).Data.First();
                    direct = new DirectPrinting(BarcodeOut, GitGetirIddonainm, _syPrinterList.First());
                    RevorkGiris revorkGiris = new RevorkGiris(txtBarcodeReader.Text.ToUpper().Trim().Split('_')[1], direct, BarcodeOut, IzDonanimCountGet(veri1[1]), _istasyonList, _modulerYapiList);
                    //  this.Hide();
                    revorkGiris.SystemTetikKonveyor += RevorkDoldurEvent;
                    revorkGiris.ShowDialog();

                    //this.Close();
                    txtBarcodeReader.Clear();

                }
                else
                {
                    txtBarcodeReader.Clear();
                }
                RevorkDoldur();
            }
        }

        private IDataResult<ErRework> RevorkGiris(ErRework erRework, string revu)
        {
            IDataResult<ErRework> result;
            result = _repo.GetRepository<ErRework>().Add(erRework);
            IDataResult<List<IzGenerateId>> veriler = GetIzDonanimIDGeneraciq(revu);

            foreach (var item in veriler.Data)
            {
                item.Revork = true;
                _repo.GetRepository<IzGenerateId>().Update(item);
            }
            MessageBoxLbl.MessageBas($"Donanim Systemde Kayit Oldu! {BarcodeOut.BarcodeIcerik}", Color.Red, lblMessage);
            if (result.Success)
            {

                return result;
            }
            else
            {
                return null;
            }

        }

        private IDataResult<List<IzGenerateId>> GetIzDonanimIDGeneraciq(string veriler)
        {
            return _repo.GetRepository<IzGenerateId>().GetAll(x => x.Barcode == veriler);
        }

        private void txtFaultName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                var result = _repo.GetRepository<ErErrorCode>().Get(x => x.ErrorCode == txtFaultName.Text.ToUpper().Trim());
                lblHataCode.Text = result.Data.ErrorName;
                txtFaultHarnessLocation.Enabled = true;
                txtFaultHarnessLocation.Focus();
            }
        }

        private void txtFaultHarnessLocation_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtFaultCabloE.Enabled = true;
                txtFaultCabloE.Focus();
            }
        }

        private void txtFaultCabloE_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                txtOperator.Enabled = true;
                txtOperator.Focus();
            }
        }

        private void txtOperator_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtFaultSetLocation.Enabled = true;
                txtFaultSetLocation.Focus();
            }
        }

        private void txtFaultSetLocation_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cbFaultReason.Enabled = true;
                cbFaultReason.Focus();
            }
        }

        private void cbFaultReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtExplanation.Enabled = true;
            txtExplanation.Focus();
        }

        private void txtExplanation_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (txtExplanation.Text != null)
                    {
                        veri1 = lblIdDonanim.Text.Split('_');
                        var result = _repo.GetRepository<ErRework>().Get(x => x.Id == int.Parse(veri1[2])).Data;
                        if (result.ReworkFixOperator == null)
                        {
                            result.UpdateDate = TarihHesapla.GetSystemDate();
                            result.ReworkFixOperator = _vardiya.Name;
                            result.ErrorCode = Convert.ToInt16(txtFaultName.Text);
                            result.FaultGoz = txtFaultCabloE.Text;
                            result.FaultInIstayon = txtFaultSetLocation.Text;
                            result.FaultRegion = txtFaultHarnessLocation.Text;
                            result.Comment = txtExplanation.Text;
                            result.ReworkFixDate = TarihHesapla.GetSystemDate();
                            result.ReworkFixOperator = $"{Environment.MachineName}-{_vardiya.Name}";
                            var resulkt = _repo.GetRepository<ErRework>().Update(result);
                            if (resulkt.Success)
                            {
                                BarcodeOut.BarcodeIcerik = $"FIX_{veri1[1]}_{veri1[2]}";
                                BarcodeOut.Name = "Tamir";
                                lblMessage.Text = "Donanim Reworkta Tamir Edildi!"; lblMessage.ForeColor = Color.Lime;
                                IzGenerateId GitGetirIddonainm = GetIzDonanimIDGeneraciq(veri1[1]).Data.First();
                                direct = new DirectPrinting(BarcodeOut, GitGetirIddonainm, _syPrinterList.First());
                                direct.PrintFileNew(BarcodeOut);
                                txtBarcodeReader.Clear();
                                GitKapa();
                                RevorkDoldur();
                            }
                            else
                            {
                                txtBarcodeReader.Clear();
                                GitKapa();
                                RevorkDoldur();
                            }
                        }
                    }
                    else
                    {
                        txtBarcodeReader.Clear();
                        lblIdDonanim.Text = "";
                        GitKapa();
                        RevorkDoldur();
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxLbl.MessageBas($"{ex.Message}", Color.Red, lblMessage);
                    txtBarcodeReader.Clear();
                    lblIdDonanim.Text = "";
                    GitKapa();
                    RevorkDoldur();
                }
            }
        }
        void GitKapa()
        {
            lblIdDonanim.Text = "Barcode ID";
            lblHataCode.Text = "Hata Kodu";
            txtFaultName.Enabled = false;
            txtFaultHarnessLocation.Enabled = false;
            txtFaultCabloE.Enabled = false;
            txtOperator.Enabled = false;
            txtFaultSetLocation.Enabled = false;
            txtExplanation.Enabled = false;
            cbFaultReason.Enabled = false;
            txtFaultName.Text = "";
            txtFaultHarnessLocation.Text = "";
            txtFaultCabloE.Text = "";
            txtOperator.Text = "";
            txtFaultSetLocation.Text = "";
            txtExplanation.Text = "";
            cbFaultReason.Text = "";

        }
        public void RevorkDoldurEvent(object sender, EventArgs e)
        {

            txtBarcodeReader.Focus();
            revorkSQLList = _repo.GetRepository<ErRework>().GetAll(null).Data;
            var verilerGiris = revorkSQLList.Where(x => x.ReworkInOperator != null && x.ReworkFixOperator == null).OrderByDescending(x => x.ReworkInDate);
            var verilerCikis = revorkSQLList.Where(x => x.ReworkFixOperator != null && x.ReworkOut == null).OrderByDescending(x => x.ReworkInDate);
            girenRevork.DataSource = verilerGiris.ToList();
            cikanRevork.DataSource = verilerCikis.ToList();
        }
        public void RevorkDoldur()
        {

            txtBarcodeReader.Focus();
            revorkSQLList = _repo.GetRepository<ErRework>().GetAll(null).Data;
            var verilerGiris = revorkSQLList.Where(x => x.ReworkInOperator != null && x.ReworkFixOperator == null).OrderByDescending(x => x.ReworkInDate);
            var verilerCikis = revorkSQLList.Where(x => x.ReworkFixOperator != null && x.ReworkOut == null).OrderByDescending(x => x.ReworkInDate);
            girenRevork.DataSource = verilerGiris.ToList();
            cikanRevork.DataSource = verilerCikis.ToList();
        }
        private void Revork_Load(object sender, EventArgs e)
        {
            RevorkDoldur();
            GitKapa();
        }
        private void btnGiris_Click(object sender, EventArgs e)
        {

        }
        private void girenRevork_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.girenRevork.Rows[e.RowIndex];
                try
                {
                    listBox1.Items.Clear();
                    var veriler = row.Cells["Referans"].Value.ToString();
                    lblIdDonanim.Text = veriler;
                    izDonanimCountList = IzDonanimCountGet(veriler);
                    var result = izDonanimCountList.ToList();
                    foreach (var item in result)
                    {
                        listBox1.Items.Add(_istasyonList.FirstOrDefault(x => x.Id == item.UrIstasyonId).Name);
                    }
                    //Thread.Sleep(500);
                    txtFaultName.Focus();
                }
                catch (Exception ex)
                {
                    //lblDoananimReferans.Text = donanim_referasn = row.Cells["HarnessModelName"].Value.ToString();
                    //listBox1.Items.Add(donanim_referasn);
                }
            }
            girenRevork.ClearSelection();
        }

        private List<IzDonanimCount> IzDonanimCountGet(string? veriler)
        {
            var res = StringSpanConverter.SplitWithoutAllocationReturnArray(veriler.AsSpan(), '-');
            var idres = StringSpanConverter.GetCharsIsDigit(res[2]);
            return _repo.GetRepository<IzDonanimCount>().GetAll(x => x.IdDonanim == idres).Data;
        }
    }
}
