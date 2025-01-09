using Nursan.Domain.Entity;
using System.Data;

namespace Nursan.UI
{
    public partial class AlertTanitma : Form
    {
        UretimOtomasyonContext fuse;
        public AlertTanitma()
        {
            InitializeComponent();
            fuse = new();
        }
        private string donanim_referasn;
        private string alertid = "0"; private string release;

        private void Button1_Click(object sender, EventArgs e)
        {
            using (UretimOtomasyonContext fs = new())
            {
                foreach (var item in listBox1.Items)
                {
                    OrHarnessModel tpl = fs.OrHarnessModels.SingleOrDefault(x => x.HarnessModelName == item.ToString());
                    try
                    {
                        tpl.Release = txtRerease.Text;
                        tpl.AlertNumber = int.Parse(txtAlertNumara.Text);
                        tpl.UpdateDate = DateTime.Today;
                        fs.SaveChanges();
                        dataGridView1.Refresh();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.IndexOf("FOREIGN KEY") != 0)
                        {
                            MessageBox.Show("Lutfen Dogru Alert Girin Boyle Alert Yok!", "Hata", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Bir Hata Olustu", "Hata", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        }
                    }
                }
                textBox1.Clear();
                comboBox1.Text = ""; txtAlertNumara.Clear(); txtRerease.Clear();
                listBox1.Items.Clear();
                ; dataGridView1.DataSource = fs.OrHarnessModels.ToList();
            }// dataGridView1.DataSource = null; dataGridView1.DataSource = fuse.OrHarnessModels;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = fuse.OrHarnessModels.ToList();
            //fuse.OrHarnessModels;
        }
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                try
                {

                    txtAlertNumara.Text = alertid = row.Cells["AlertNumber"].Value.ToString();
                    lblDoananimReferans.Text = donanim_referasn = row.Cells["HarnessModelName"].Value.ToString();

                    if (string.IsNullOrEmpty(row.Cells["Release"].Value.ToString()))
                    {
                        release = "0";
                    }
                    else
                    {
                        release = row.Cells["Release"].Value.ToString();
                    }
                    if (string.IsNullOrEmpty(row.Cells["AlertNumber"].Value.ToString()))
                    {
                        txtAlertNumara.Text = alertid = "0";
                    }
                    else
                    {
                        txtAlertNumara.Text = alertid = row.Cells["AlertNumber"].Value.ToString();
                    }
                    // == null ? "1": row.Cells["Release"].Value.ToString();
                    txtRerease.Text = release;
                    listBox1.Items.Add(donanim_referasn);
                }
                catch (Exception ex)
                {
                    lblDoananimReferans.Text = donanim_referasn = row.Cells["HarnessModelName"].Value.ToString();
                    listBox1.Items.Add(donanim_referasn);
                }
            }
            dataGridView1.ClearSelection();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            AlertAra frm = new();
            if (txtAlertNumara.Text != "" && txtRerease.Text != "")
            {

                frm.alertid = Convert.ToInt32(frm.txtAlertNo.Text = txtAlertNumara.Text);
                frm.release = txtRerease.Text;
                frm.lblIDAlert.Text = lblDoananimReferans.Text;
            }
            frm.Show();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            string system = comboBox1.Text + " " + textBox1.Text;
            try
            {
                //var veriler1 = fuse.OrHarnessModels.Where(x => x.HarnessModelName.Contains(textBox3.Text.ToUpper().Trim()));
                var veriler1 = fuse.OrHarnessModels.Where(x => x.HarnessModelName.StartsWith(textBox3.Text) && x.HarnessModelName.EndsWith(textBox1.Text));
                var veriler2 = veriler1.Where(x => x.HarnessModelName.EndsWith(textBox1.Text.ToUpper().Trim()));
                dataGridView1.DataSource = veriler1.ToList();
            }
            catch (Exception ex)
            {


            }

        }
        private void Button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.SelectedItems.Count; i++)
                listBox1.Items.Remove(listBox1.SelectedItems[i]);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                dataGridView1.DataSource = fuse.OrHarnessModels.Where(x => x.HarnessModelName.Contains(textBox1.Text)).ToList();
            }
        }
    }
}
