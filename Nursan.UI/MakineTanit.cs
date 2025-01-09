using Nursan.Business.Manager;
using Nursan.Business.Services;
using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using System.Net;
using System.Net.NetworkInformation;

namespace Nursan.UI
{
    public partial class MakineTanit : Form
    {
        #region Promenlivi
        UretimOtomasyonContext _context;
        IUnitOfWork unitOfWork;
        List<OpMashin> mashinList = new List<OpMashin>();
        List<UrModulerYapi> ModelYapiList = new List<UrModulerYapi>();
        List<OrFamily> familyList = new List<OrFamily>();
        List<UrVardiya> vardiyaList = new List<UrVardiya>();
        List<SyBarcodeInput> BarcodeIn = new List<SyBarcodeInput>();
        List<SyBarcodeOut> BarcodeOut = new List<SyBarcodeOut>();
        ISytemService<SyBarcodeInCrossIstasyon> systemService;
        UrIstasyon istasyon = new UrIstasyon();
        #endregion
        public MakineTanit(UnitOfWork repo)
        {
            unitOfWork = repo;
            _context = new UretimOtomasyonContext();
            unitOfWork = new UnitOfWork(_context);

            InitializeComponent();
        }
        void load()
        {
            OpsionYukle();
            txtIpAddress.Clear(); txtMakine.Clear(); cbVardiya.Text = "";
            ModelYapiList = unitOfWork.GetRepository<UrModulerYapi>().GetAll(null).Data;
            mashinList = unitOfWork.GetRepository<OpMashin>().GetAll(null).Data;
            BarcodeIn = unitOfWork.GetRepository<SyBarcodeInput>().GetAll(null).Data;
            BarcodeOut = unitOfWork.GetRepository<SyBarcodeOut>().GetAll(null).Data;
            gridMakine.DataSource = mashinList.ToList();
            //  gridOpsion.DataSource = unitOfWork.GetRepository<UrIstasyon>().GetAll(null).Data;
            txtMakine.Text = Environment.MachineName;
            txtIpAddress.Text = Dns.GetHostByName(Environment.MachineName).AddressList[1].ToString();
            familyList = unitOfWork.GetRepository<OrFamily>().GetAll(null).Data;
            vardiyaList = unitOfWork.GetRepository<UrVardiya>().GetAll(null).Data;
            cbMashin.DataSource = mashinList.ToList();
            cbMashin.DisplayMember = "MasineName";
            cbFamily.DataSource = familyList;
            cbFamily.DisplayMember = "FamilyName";
            cbArkaPlanEtap.DataSource = ModelYapiList.ToList();
            cbArkaPlanEtap.DisplayMember = "Etap";
            cbModuleryapi.DataSource = ModelYapiList.ToList();
            cbModuleryapi.DisplayMember = "Etap";
            cbVardiya.DataSource = vardiyaList.ToList();
            cbVardiya.DisplayMember = "Name";
            cbOkuma.DataSource = BarcodeIn.ToList();
            cbOkuma.DisplayMember = "Name";
            cbBasma.DataSource = BarcodeOut.ToList();
            cbBasma.DisplayMember = "Name";
            listBarcodeIn.Items.Clear();
            listBarcodeOut.Items.Clear();
        }
        private void OpsionYukle()
        {
            var result = (from i in _context.UrIstasyons
                          join f in _context.OrFamilies on i.FamilyId equals f.Id
                          join m in _context.UrModulerYapis on i.ModulerYapiId equals m.Id
                          join p in _context.OpMashins on i.MashinId equals p.Id
                          join b in _context.SyBarcodeOuts on i.SyBarcodeOutId equals b.Id
                          join v in _context.UrVardiyas on i.VardiyaId equals v.Id
                          // where p.MasineName == Environment.MachineName
                          select new
                          {
                              IstasyonID = i.Id,
                              Istasyon = i.Name,
                              MakineId = p.Id,
                              Makine = p.MasineName,
                              VardiyaID = i.VardiyaId,
                              Vardiya = v.Name,
                              ModulerYapiID = m.Id,
                              ModulerYapiEtap = m.Etap,
                              FamilyID = f.Id,
                              FamilyName = f.FamilyName,
                              BarcodeBasma = b.Name,
                              CalismaSaati = i.Calismasati,
                              Hedef = i.Hedef,
                              SayiCarp = i.Sayicarp,
                              Sayi = i.Sayi,
                              RealAdet = i.Realadet,
                              Ortalama = i.Orta,



                          }).ToList();
            gridOpsion.DataSource = result;
        }
        private void StartingOption_Load(object sender, System.EventArgs e)
        {
            load();
        }
        public string makadress(string macadress)
        {

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                OperationalStatus ot = nic.OperationalStatus;
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macadress = nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macadress;
        }
        private void btnMYKaydet_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNameConfig.Text) && !string.IsNullOrEmpty(cbFamily.Text))
            {


                try
                {
                    istasyon.Name = $"{cbFamily.Text}-{txtNameConfig.Text}";
                    istasyon.ModulerYapiId = ModelYapiList.SingleOrDefault(x => x.Etap == cbModuleryapi.Text).Id;
                    istasyon.MashinId = mashinList.SingleOrDefault(x => x.MasineName == cbMashin.Text).Id;
                    istasyon.VardiyaId = vardiyaList.SingleOrDefault(x => x.Name == cbVardiya.Text).Id;
                    istasyon.Calismasati = txtCalismaSati.Text;
                    istasyon.FamilyId = familyList.SingleOrDefault(x => x.FamilyName == cbFamily.Text).Id;
                    istasyon.Hedef = int.Parse(txtHedef.Text);
                    istasyon.Sayicarp = txtSayiCarp.Text == "" ? 0 : int.Parse(txtSayiCarp.Text);
                    istasyon.SyBarcodeOutId = cbBasma.Text == "" ? null : BarcodeOut.SingleOrDefault(x => x.Name == cbBasma.Text).Id;
                    istasyon.Activ = true;
                    var result = unitOfWork.GetRepository<UrIstasyon>().Add(istasyon);
                    if (listBarcodeIn.Items.Count > 0)
                    {
                        foreach (var item in listBarcodeIn.Items)
                        {
                            var gelenBarcoedeInId = BarcodeIn.SingleOrDefault(x => x.Name == item.ToString()).Id;
                            unitOfWork.GetRepository<SyBarcodeInCrossIstasyon>().Add(new SyBarcodeInCrossIstasyon
                            {
                                SysBarcodeInId = gelenBarcoedeInId,
                                UrIstasyonId = result.Data.Id

                            });
                        }
                    }
                    unitOfWork.SaveChanges();
                    load();
                }
                catch (Exception EX)
                {

                }

            }
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMakine.Text) && !string.IsNullOrEmpty(txtIpAddress.Text))
            {
                try
                {
                    var veri = mashinList.Find(x => x.MasineName == txtMakine.Text);
                    if (veri == null)
                    {
                        unitOfWork.GetRepository<OpMashin>().Add(new OpMashin() { MasineName = txtMakine.Text, IpAddress = txtIpAddress.Text });
                        //unitOfWork.SaveChanges();
                        // ModelYapiList.FirstOrDefault(x => x.Etap == cbArkaPlanEtap.Text).Id }
                        MessageBox.Show($"{txtMakine.Text} Makineyi SitemeKayit Ettiniz!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        load();
                    }
                    else
                    {
                        MessageBox.Show($"{txtMakine.Text} Boyle bir makine var!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }

                }
                catch (ArgumentNullException ex)
                {

                }

            }
            else
            {
                MessageBox.Show("Tum verileri girin!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btnMYRefres_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Vardiya Giriniz",
                       "Vardiya Giris",
                       "Vardiya",
                       0,
                       0);
            try
            {
                var veri = vardiyaList.SingleOrDefault(x => x.Name == input);
                if (veri == null && input != "")
                {
                    unitOfWork.GetRepository<UrVardiya>().Add(new UrVardiya { Name = input });
                    MessageBox.Show("Girdiginiz Vardiya Sisteme Kayit Ettiniz!", "Vardiya", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    load();
                }
                else if (veri != null && input != "")
                {
                    MessageBox.Show("Girdiginiz Vardiya Sistemde Tanitik!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void gridMakine_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.gridMakine.Rows[e.RowIndex];
                try
                {
                    txtMakine.Text = row.Cells["MasineName"].Value.ToString();
                    cbMashin.Text = txtMakine.Text;

                }
                catch (Exception ex)
                {

                }
            }
            gridMakine.ClearSelection();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string message = "Makine Silmek Istediginize Emin Mi Si Niz!\",\"Warning?";
            DialogResult dialogResult = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                if (txtMakine.Text.Length > 0)
                {

                    unitOfWork.GetRepository<OpMashin>().Delete(mashinList.SingleOrDefault(x => x.MasineName == txtMakine.Text));
                    MessageBox.Show($"{txtMakine.Text} Makineyi sildiniz!", "Makine", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    load();
                }
                else
                {
                    MessageBox.Show($"{txtMakine.Text} Makineyi Seciniz!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
        }

        private void gridOpsion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            listBarcodeIn.Items.Clear();
            listBarcodeOut.Items.Clear();
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.gridOpsion.Rows[e.RowIndex];
                try
                {
                    istasyon.Id = int.Parse(row.Cells["IstasyonID"].Value.ToString());
                    istasyon.MashinId = int.Parse(row.Cells["MakineId"].Value.ToString());
                    cbMashin.Text = row.Cells["Makine"].Value.ToString();
                    istasyon.VardiyaId = int.Parse(row.Cells["VardiyaID"].Value.ToString());
                    cbVardiya.Text = row.Cells["Vardiya"].Value.ToString();
                    istasyon.ModulerYapiId = int.Parse(row.Cells["ModulerYapiID"].Value.ToString());
                    cbModuleryapi.Text = row.Cells["ModulerYapiEtap"].Value.ToString();
                    istasyon.FamilyId = int.Parse(row.Cells["FamilyID"].Value.ToString());
                    listBarcodeOut.Items.Add(row.Cells["BarcodeBasma"].Value.ToString());
                    txtCalismaSati.Text = istasyon.Calismasati = row.Cells["CalismaSaati"].Value.ToString();
                    txtHedef.Text = row.Cells["Hedef"].Value.ToString();
                    txtSayiCarp.Text = row.Cells["SayiCarp"].Value.ToString();
                    //row.Cells["Sayi"].Value.ToString();
                    //row.Cells["RealAdet"].Value.ToString();
                    //row.Cells["Ortalama"].Value.ToString();
                    cbFamily.Text = row.Cells["Istasyon"].Value.ToString().Split('-')[0];
                    txtNameConfig.Text = row.Cells["Istasyon"].Value.ToString().Split('-')[1];
                    istasyon.Name = $"{cbFamily.Text}-{txtNameConfig.Text}";
                    systemService = new SytemMnager<SyBarcodeInCrossIstasyon>(new UnitOfWork(new UretimOtomasyonContext()));
                    var barcodeCrosseIstasyon = systemService.GetAll(x => x.UrIstasyonId == istasyon.Id).Data.ToList();

                    foreach (var item in barcodeCrosseIstasyon)
                    {
                        SyBarcodeInput result = BarcodeIn.FirstOrDefault(x => x.Id == item.SysBarcodeInId);
                        listBarcodeIn.Items.Add(result.Name);
                    }



                    //istasyon.Hedef = int.Parse(txtHedef.Text);
                    // istasyon.Sayicarp = txtSayiCarp.Text == "" ? 0 : int.Parse(txtSayiCarp.Text);
                    istasyon.SyBarcodeOutId = BarcodeOut.SingleOrDefault(x => x.Name == cbBasma.Text).Id;
                }
                catch (Exception ex)
                {

                }
            }
            gridOpsion.ClearSelection();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtMakine.Text) && !string.IsNullOrEmpty(txtIpAddress.Text))
            {
                try
                {
                    var veri = mashinList.Find(x => x.MasineName == txtMakine.Text);
                    if (veri == null)
                    {
                        unitOfWork.GetRepository<OpMashin>().Update(new OpMashin() { MasineName = txtMakine.Text, IpAddress = txtIpAddress.Text });
                        //unitOfWork.SaveChanges();
                        // ModelYapiList.FirstOrDefault(x => x.Etap == cbArkaPlanEtap.Text).Id }
                        MessageBox.Show($"{txtMakine.Text} Makineyi SitemeKayit Ettiniz!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        load();
                    }
                    else
                    {
                        MessageBox.Show($"{txtMakine.Text} Boyle bir makine var!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    }

                }
                catch (ArgumentNullException ex)
                {

                }

            }
            else
            {
                MessageBox.Show("Tum verileri girin!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Etap Giriniz",
                       "Etap Giris",
                       "Etap",
                       0,
                       0);
            try
            {
                var veri = ModelYapiList.SingleOrDefault(x => x.Etap == input);
                if (veri == null && input != "")
                {
                    unitOfWork.GetRepository<UrModulerYapi>().Add(new UrModulerYapi { Etap = input });
                    MessageBox.Show("Girdiginiz Etap Sisteme Kayit Ettiniz!", "Vardiya", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    load();
                }
                else if (veri != null && input != "")
                {
                    MessageBox.Show("Girdiginiz Etap Sistemde Tanitik!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BarcodeConfig frm = new BarcodeConfig();
            frm.ShowDialog();
        }

        private void cbOkuma_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBarcodeIn.Items.Add(cbOkuma.Text);
        }

        private void cbBasma_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBarcodeOut.Items.Add(cbBasma.Text);
        }
        private void btnMYGuncelle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNameConfig.Text) && !string.IsNullOrEmpty(cbFamily.Text))
            {
                try
                {
                    istasyon.Name = $"{cbFamily.Text}-{txtNameConfig.Text}";
                    istasyon.ModulerYapiId = ModelYapiList.SingleOrDefault(x => x.Etap == cbModuleryapi.Text).Id;
                    istasyon.MashinId = mashinList.SingleOrDefault(x => x.MasineName == cbMashin.Text).Id;
                    istasyon.VardiyaId = vardiyaList.SingleOrDefault(x => x.Name == cbVardiya.Text).Id;
                    istasyon.Calismasati = txtCalismaSati.Text;
                    istasyon.FamilyId = familyList.SingleOrDefault(x => x.FamilyName == cbFamily.Text).Id;
                    istasyon.Hedef = int.Parse(txtHedef.Text);
                    istasyon.Sayicarp = txtSayiCarp.Text == "" ? 0 : int.Parse(txtSayiCarp.Text);
                    if (cbBasma.Text == "")
                    { istasyon.SyBarcodeOutId = null; }
                    else { istasyon.SyBarcodeOutId = BarcodeOut.SingleOrDefault(x => x.Name == cbBasma.Text).Id; }
                    istasyon.UnikId = "";
                    istasyon.Activ = true;
                }
                catch (Exception EX)
                {

                }
                var result = unitOfWork.GetRepository<UrIstasyon>().Update(istasyon);

                // var gelenBarcoedeInId = BarcodeIn.SingleOrDefault(x => x.Name == cbOkuma.Text).Id;
                var gelen = unitOfWork.GetRepository<SyBarcodeInCrossIstasyon>().GetAll(x => x.UrIstasyonId == istasyon.Id).Data;

                foreach (var i in listBarcodeIn.Items)
                {
                    unitOfWork.GetRepository<SyBarcodeInCrossIstasyon>().Update(new SyBarcodeInCrossIstasyon { SysBarcodeInId = BarcodeIn.FirstOrDefault(x => x.Name == i.ToString()).Id, UrIstasyonId = istasyon.Id });
                }


                unitOfWork.SaveChanges();
                load();
            }
        }

        private void btnSilBarcodeOkuma_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBarcodeIn.SelectedItems.Count; i++)
                listBarcodeIn.Items.Remove(listBarcodeIn.SelectedItems[i]);
        }

        private void btnSilBarcodeBasma_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBarcodeOut.SelectedItems.Count; i++)
                listBarcodeOut.Items.Remove(listBarcodeOut.SelectedItems[i]);
        }

        private void btnMYSilme_Click(object sender, EventArgs e)
        {
            string message = "Istasyon Silmek Istediginize Emin Mi Si Niz!\",\"Warning?";
            DialogResult dialogResult = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)

            {
                if (!string.IsNullOrEmpty(txtNameConfig.Text) && !string.IsNullOrEmpty(cbFamily.Text))
                {
                    var result = unitOfWork.GetRepository<UrIstasyon>().Get(x => x.Name == $"{txtNameConfig.Text}-{cbFamily.Text}");
                    if (result != null)
                    {
                        unitOfWork.GetRepository<UrIstasyon>().Delete(result.Data);
                        var silinen = unitOfWork.GetRepository<SyBarcodeInCrossIstasyon>().GetAll(x => x.UrIstasyonId == result.Data.Id);
                        foreach (var item in silinen.Data)
                        {
                            unitOfWork.GetRepository<SyBarcodeInCrossIstasyon>().Delete(item);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Istasyon Silinmedi", "islem", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void btnRefres_Click(object sender, EventArgs e)
        {
            this.load();
        }


    }
}
