using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.Opsionlar;
using Nursan.Validations.ValidationCode;

namespace Nursan.UI
{
    public partial class DonanimTanitma : Form
    {
        private readonly DonanimService donanim = new DonanimService(new UnitOfWork(new UretimOtomasyonContext()));
        private readonly SqlValidation validation = new SqlValidation(new UnitOfWork(new UretimOtomasyonContext())); // ProgressBar frm = new ProgressBar();
        private readonly AlertHarnesIslemleri alertharnes = new AlertHarnesIslemleri(new UretimOtomasyonContext());
        public DonanimTanitma()
        {

            //Thread tr = new Thread(new ThreadStart(formRun));
            // tr.Start();

            InitializeComponent();
            SystemdenCek();
            // tr.Abort();
        }
        List<AlertHarnesIslemleri> alertHarnes = new List<AlertHarnesIslemleri>();
        OrAlertBaglanti baglanti = new OrAlertBaglanti();
        OrHarnessModel hrModel = new OrHarnessModel();
        OrAlert alertim = new OrAlert();
        List<OrAlertBaglanti> baglantiList = new List<OrAlertBaglanti>();
        List<OrHarnessModel> hrModelList = new List<OrHarnessModel>();
        List<OrAlert> alertimList = new List<OrAlert>();
        List<OrFamily> familyList = new List<OrFamily>();
        List<OrHarnessConfig> hrnessConfig = new List<OrHarnessConfig>();

        #region Начало

        private void DoanimTanitma_Load(object sender, EventArgs e)
        {
            TarihHesapla tarih = new TarihHesapla();
            var veri = tarih.GetSystemDateNEw();
            //formRun();
        }
        private void formRun()
        {
            Application.Run(new ProgressBar());
        }
        private void SystemdenCek()
        {
            try
            {
                cbAlert.Items.Clear();
                cbConfig.Items.Clear();
                cbFamily.Items.Clear();

                dataGridView1.DataSource = alertHarnes = alertharnes.GetharnesAndAlert();
                dataGridView2.DataSource = hrModelList = donanim.GetDonanimHarness();
                hrnessConfig = donanim.GetConfigGetir();
                alertimList = donanim.GetAlertAll();
                familyList = donanim.GetFamily();
                baglantiList = donanim.GetAllBaglanti();
                foreach (var item in alertimList)
                {
                    cbAlert.Items.Add(item.Name);
                }
                foreach (var i in hrnessConfig)
                {
                    cbConfig.Items.Add(i.ConfigTork);
                }
                foreach (var item in familyList)
                {
                    cbFamily.Items.Add(item.FamilyName);
                };
            }
            catch (Exception)
            {

            }
        }
        #endregion
        private void btnFamily_Click(object sender, EventArgs e)
        {
            cbFamily.Items.Clear();
            var result = validation.GetFamily(cbFamily.Text);
            if (!string.IsNullOrEmpty(cbFamily.Text.ToUpper()) && result == null)
            {
                donanim.AddFamily(new OrFamily { FamilyName = cbFamily.Text.ToUpper().ToUpper() });
                cbFamily.Items.Add(cbFamily.Text.ToUpper().ToUpper()); SystemdenCek();
            }
            else
            {
                MessageBox.Show("Bu Aile mevcut!", "Hata", icon: MessageBoxIcon.Error, buttons: MessageBoxButtons.OKCancel);
            }
        }
        private void btnReferanSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSideCode.Text.ToUpper()) && !string.IsNullOrEmpty(cbFamily.Text.ToUpper()) && !string.IsNullOrEmpty(txtPreffix.Text.ToUpper()) && !string.IsNullOrEmpty(txtSuffix.Text.ToUpper()) && !string.IsNullOrEmpty(txtReleace.Text.ToUpper()))
            {
                OrHarnessModel harnessModel = new OrHarnessModel(prefix: txtPreffix.Text.ToUpper(), family: cbFamily.Text.ToUpper(), suffix: txtSuffix.Text.ToUpper(), release: txtReleace.Text, sideCode: txtSideCode.Text.ToUpper(), access: true, active: true, alertnumber: 1);
                harnessModel.FamilyId = familyList.FirstOrDefault(x => x.FamilyName == cbFamily.Text.ToUpper()).Id;
                var result = validation.GetHarnessModel(harnessModel);
                if (result == null)
                {
                    donanim.AddUrHarness(harnessModel);
                    MessageBox.Show($"Donanim {harnessModel.HarnessModelName} sisteme tanitiniz.", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GitSifirlaTextBox();
                }
                else
                    MessageBox.Show($"Donanim {harnessModel.HarnessModelName} sisteme Tanitamadiniz.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GitSifirlaTextBox();
            }
            else
            {
                MessageBox.Show($"Tum simgeleri doldurun", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); GitSifirlaTextBox();
            }
        }

        private void GitSifirlaTextBox()
        {
            txtReleace.Clear(); txtPreffix.Clear(); txtALerNumber.Clear(); txtAlert.Clear();
            txtSideCode.Clear(); txtSuffix.Clear(); cbFamily.Text = ""; SystemdenCek();

        }

        private void btnSaveAlert_Click(object sender, EventArgs e)
        {
            listAlert.Items.Clear();
            if (!string.IsNullOrEmpty(txtALerNumber.Text.ToUpper()) && !string.IsNullOrEmpty(txtAlert.Text.ToUpper()))
            {
                //UrRelease releace = donanim.AddReleace(new UrRelease { CreateDate = DateTime.Now, Name = txtReleace.Text.ToUpper() });
                donanim.AddAlert(new OrAlert { CreateDate = DateTime.Now, Name = txtAlert.Text.ToUpper(), AlertAccess = true, AlertNumber = int.Parse(txtALerNumber.Text) });//chAlert.Checked == true ? true : false, Active = true
                // donanim.GitAlertBaglantiYap
                GitSifirlaTextBox(); SystemdenCek();
            }
        }

        #region Mouse

        private void txtAlert_MouseClick(object sender, MouseEventArgs e)
        {
            txtAlert.Clear();
        }

        private void txtReleace_MouseClick(object sender, MouseEventArgs e)
        {
            txtALerNumber.Clear();
        }
        #endregion

        #region DataGrid
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            listAlert.Items.Clear();
            listRefrans.Items.Clear();
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                try
                {
                    baglanti.Id = row.Cells["baglantiId"].Value != null ? int.Parse(row.Cells["baglantiId"].Value.ToString()) : 0;
                    alertim.Id = int.Parse(row.Cells["AlertId"].Value.ToString());
                    lbllertId.Text = alertim.Id.ToString();
                    txtAlert.Text = alertim.Name = lblAlert.Text = txtAlert.Text = row.Cells["AlertName"].Value.ToString();
                    alertim.AlertAccess = chAlert.Checked = bool.Parse(row.Cells["AlertAccess"].Value.ToString());
                    hrModel.Id = int.Parse(row.Cells["HarnessId"].Value.ToString());
                    lblReferans.Text = hrModel.Id.ToString();
                    hrModel.Access = chReferasn.Checked = bool.Parse(row.Cells["HarnesAccess"].Value.ToString());
                    baglanti.AlertNumber = row.Cells["AlertNumber"].Value != null ? int.Parse(row.Cells["AlertNumber"].Value.ToString()) : 0;
                    txtAlertnumber.Text = baglanti.AlertNumber.ToString();
                    var result = baglantiList.Where(x => x.AlertNumber == baglanti.AlertNumber);

                    foreach (var item in result)
                    {

                        listAlert.Items.Add(alertimList.FirstOrDefault(x => x.Id == item.AlertId).Name);
                        listRefrans.Items.Add(hrModelList.FirstOrDefault(x => x.Id == item.HarnessId).HarnessModelName);

                    }

                    if (alertim.AlertAccess != null)
                    {
                        chAlert.Checked = true;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            dataGridView1.ClearSelection();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
                try
                {
                    hrModel.Id = int.Parse(row.Cells["Id"].Value.ToString());
                    lblHarnesId.Text = hrModel.Id.ToString();
                    baglanti.HarnessId = int.Parse(row.Cells["Id"].Value.ToString());
                    txtPreffix.Text = hrModel.Prefix = txtPreffix.Text = row.Cells["Prefix"].Value.ToString();
                    cbFamily.Text = hrModel.Family = cbFamily.Text = row.Cells["Family"].Value.ToString();
                    txtSuffix.Text = hrModel.Suffix = txtSuffix.Text = row.Cells["Suffix"].Value.ToString();
                    txtReleace.Text = hrModel.Release = txtReleace.Text = row.Cells["Release"].Value.ToString();
                    hrModel.HarnessModelName = lblReferans.Text = row.Cells["HarnessModelName"].Value.ToString();
                    hrModel.Access = chReferasn.Checked = bool.Parse(row.Cells["Access"].Value.ToString());
                    hrModel.SideCode = txtSideCode.Text = row.Cells["SideCode"].Value.ToString();
                    hrModel.CreateDate = DateTime.Now;
                    if (listAlert.Items.Contains(hrModel.HarnessModelName) != true)
                    { listRefrans.Items.Add(hrModel.HarnessModelName); }

                    var verilerAlertReferasn = donanim.GetBaglanti(baglanti.HarnessId);
                    foreach (var item in verilerAlertReferasn)
                    {
                        var ar2 = alertimList.SingleOrDefault(x => x.Id == item.AlertId);
                        if (hrModel.Access != null)
                        {
                            chReferasn.Checked = true;
                        }
                        if (listAlert.Items.Contains(ar2.Name) != true)
                        { listAlert.Items.Add(ar2.Name); }

                    }
                }
                catch (Exception ex)
                {

                }
            }
            dataGridView2.ClearSelection();

        }
        #endregion
        private void btnBirlestir_Click(object sender, EventArgs e)
        {

            if (listAlert.Items.Count > 0 && listRefrans.Items.Count > 0)
            {
                foreach (var a in listAlert.Items)
                {
                    var v2 = donanim.GetAlert(a.ToString());
                    foreach (var r in listRefrans.Items)
                    {
                        var v3 = hrModelList.SingleOrDefault(x => x.HarnessModelName == r);
                        var result = donanim.GetAllAlertBaglanti(new OrAlertBaglanti { AlertId = v2.Id, HarnessId = v3.Id });
                        if (result.Count == 0)
                        {
                            baglanti = new OrAlertBaglanti { AlertId = v2.Id, HarnessId = v3.Id, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, AlertNumber = baglantiList.Count + 1 }; donanim.GitAlertBaglantiYap(baglanti);
                            // baglantiList.Add(baglanti);
                            if (donanim.GetAllAlertBaglanti(baglanti) == null)
                            {
                                donanim.GitAlertBaglantiYap(baglanti);
                            }
                            else
                            {
                                donanim.GitAlertBaglantiGuncelle(baglanti);
                            }
                        }
                    }
                }
                lblAlert.Text = "Aler";
                lblReferans.Text = "Referans";
                listAlert.Items.Clear();
                listRefrans.Items.Clear();
                MessageBox.Show($"Gelen Alertleri Donanim Ismi ile birlestirdiniz!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Ayni anda Donanim ve Alert secmeniz gerek!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblAlert.Text = "Aler";
                lblReferans.Text = "Referans";
                listAlert.Items.Clear();
                listRefrans.Items.Clear();
            }
        }

        private void btnReferansAccess_Click(object sender, EventArgs e)
        {
            if (chReferasn.Checked && lblReferans.Text != "Referans")
            {
                hrModel.Access = true;
                donanim.UpdateUrHarness(hrModel);
            }
            else if (lblReferans.Text != "Referans")
            {
                hrModel.Access = false;
                donanim.UpdateUrHarness(hrModel);
            }
        }

        private void btnAlertAccess_Click(object sender, EventArgs e)
        {
            if (chAlert.Checked && lblAlert.Text != "Alert")
            {
                alertim.AlertAccess = true;
                donanim.UpdateAlert(alertim);
            }
            else if (lblAlert.Text != "Alert")
            {
                alertim.AlertAccess = false;
                donanim.UpdateAlert(alertim);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listRefrans.SelectedItems.Count; i++)
                listRefrans.Items.Remove(listRefrans.SelectedItems[i]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listAlert.SelectedItems.Count; i++)
                listAlert.Items.Remove(listAlert.SelectedItems[i]);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSideCode.Text.ToUpper()) && !string.IsNullOrEmpty(cbFamily.Text.ToUpper()) && !string.IsNullOrEmpty(txtPreffix.Text.ToUpper()) && !string.IsNullOrEmpty(txtSuffix.Text.ToUpper()) && !string.IsNullOrEmpty(txtReleace.Text.ToUpper()))
            {
                OrHarnessModel harnessModel = validation.GetHarnessModelID(hrModel.Id);
                harnessModel.Prefix = txtPreffix.Text.ToUpper();
                harnessModel.Family = cbFamily.Text.ToUpper();
                harnessModel.Suffix = txtSuffix.Text.ToUpper();
                harnessModel.Release = txtReleace.Text;
                harnessModel.SideCode = txtSideCode.Text.ToUpper();
                harnessModel.Access = chReferasn.Checked == true ? true : false;
                var result = validation.GetHarnessModel(harnessModel);
                if (result != null)
                {
                    donanim.UpdateUrHarness(harnessModel);
                    MessageBox.Show($"Donanim {harnessModel.HarnessModelName} sisteme GUNCELLEDINIZ.", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GitSifirlaTextBox(); SystemdenCek();
                }
                else
                    MessageBox.Show($"Donanim {harnessModel.HarnessModelName} sisteme Tanitamadiniz.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GitSifirlaTextBox();
            }
            else
            {
                MessageBox.Show($"Tum simgeleri doldurun", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); GitSifirlaTextBox();
            }
        }

        private void btnALertUpdate_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cbAlert.Text.Length > 0)
            {
                bool result = listAlert.Items.Contains(cbAlert.Text);
                if (result != true)
                {
                    listAlert.Items.Add(cbAlert.Text);
                }

            }
        }

        private void txtDonanimAra_TextChanged(object sender, EventArgs e)
        {
            var veri = hrModelList.Where(x => x.Suffix.Contains(txtDonanimAra.Text.ToUpper()));
            dataGridView2.DataSource = veri;
        }

        private void btnDonanimAra_Click(object sender, EventArgs e)
        {
            var veri = hrModelList.Where(x => x.HarnessModelName.Contains(txtDonanimAra.Text.ToUpper()));
            dataGridView2.DataSource = veri;
        }

        private void txtAlertNumberAra_TextChanged(object sender, EventArgs e)
        {
            var veri = alertHarnes.Where(x => x.AlertName.Contains(txtDonanimAra.Text.ToUpper()));
            dataGridView1.DataSource = veri;
        }

        private void btnAlertNumberAra_Click(object sender, EventArgs e)
        {
            var veri = alertHarnes.Where(x => x.AlertNumber == txtAlertNumberAra.Text);
            dataGridView1.DataSource = veri;
        }

        private void txtAlertnumber_TextChanged(object sender, EventArgs e)
        {
            if (txtAlertnumber.Text.Length > 0)
            {
                var veri = alertHarnes.Where(x => x.AlertNumber.Contains(txtDonanimAra.Text.ToUpper()));
                dataGridView1.DataSource = veri;
            }
        }

        private void btnNumaraAra_Click(object sender, EventArgs e)
        {
            var veri = alertHarnes.Where(x => x.AlertNumber.Contains(txtDonanimAra.Text.ToUpper()));
            dataGridView1.DataSource = veri;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPFBSOcket.Text) && !string.IsNullOrEmpty(cbConfig.Text) && !string.IsNullOrEmpty(txtPreffix.Text) && !string.IsNullOrEmpty(cbFamily.Text) && !string.IsNullOrEmpty(txtSuffix.Text))
            {
                var harnessResults = hrModelList.FirstOrDefault(x => x.HarnessModelName == $"{txtPreffix.Text}-{cbFamily.Text}-{txtSuffix.Text}");
                var result = donanim.AddConfigGetir(new OrHarnessConfig
                {
                    ConfigTork = cbConfig.Text,
                    PFBSocket = txtPFBSOcket.Text,
                    OrHarnessModelId = harnessResults.Id

                });
            }
            else
            {
                MessageBox.Show($"Tum Simgeleri Doldurun!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
