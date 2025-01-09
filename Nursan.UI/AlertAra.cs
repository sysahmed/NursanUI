using Nursan.Domain.Entity;
using System.Data;

namespace Nursan.UI
{
    public partial class AlertAra : Form
    {
        public AlertAra()
        {
            InitializeComponent();

        }
        UretimOtomasyonContext fs = new(); public string donanim_referasn;
        public int? alertid; public string release;
        private void Form2_Load(object sender, EventArgs e)
        {
            if (txtAlertNo.Text != "")
            {
                dATAGRID(Convert.ToInt32(txtAlertNo.Text));
            }
            else
            {
                var result = fs.OrAlerts.Select(x => new { x.Id, x.AlertNumber, x.Name, x.AlertAccess, x.UpdateDate });
                dataGridView1.DataSource = result;
            }
        }

        private void BtnAlertKaydet_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtAlertNo.Text);
            OrAlert tp = new OrAlert();
            tp.Activ = true;
            tp.AlertAccess = true;
            tp.UpdateDate = DateTime.Now;
            var al = fs.OrAlerts.FirstOrDefault(x => x.AlertNumber == id);
            //if (al==null)
            //{


            //    try
            //    {
            //        tp.AlertNumber = id;
            //        tp.AlertAccess = true;

            //    }
            //    catch (Exception ex)
            //    {


            //    }
            //    fs.OrAlerts.Add(tp);
            //    fs.SaveChanges(); 
            //}
            OrAlert tpl = new();
            try
            {
                tpl.AlertNumber = Convert.ToInt32(txtAlertNo.Text);
                tpl.Name = txtAlertAtama.Text;
                tpl.Activ = true;
                tpl.AlertAccess = true;
                tpl.UpdateDate = DateTime.Now;

            }
            catch (Exception ex)
            {


            }
            fs.OrAlerts.Add(tpl);
            fs.SaveChanges();
            dATAGRID(Convert.ToInt32(txtAlertNo.Text)); txtAlertAtama.Clear();
        }
        public void dATAGRID(int? gelnid)
        {
            dataGridView1.DataSource = fs.OrAlerts.Where(x => x.AlertNumber == gelnid).Select(x => new { x.Id, x.AlertNumber, x.Name, x.AlertAccess, x.UpdateDate });
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (txtAlertNo.Text != "")
            {
                dATAGRID(Convert.ToInt32(txtAlertNo.Text));
            }
            else
            {
                dataGridView1.DataSource = fs.OrAlerts.Select(x => new { x.Id, x.AlertNumber, x.Name, x.AlertAccess, x.UpdateDate });
            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtAlertNo.Text);
            OrAlert tp = new OrAlert();
            var al = fs.OrAlerts.Where(x => x.AlertNumber == id);
            foreach (var item in al)
            {
                try
                {
                    if (checkBox1.Checked == true)
                    {
                        item.AlertAccess = true;
                    }
                    else
                    {
                        item.AlertAccess = false;
                    }

                }
                catch (Exception ex)
                {


                }
            }
            // fs.Alerts.InsertOnSubmit(tp);
            fs.SaveChanges();
            dataGridView1.DataSource = fs.OrAlerts.Select(x => new { x.Id, x.AlertNumber, x.Name, x.AlertAccess, x.UpdateDate });
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                lblIDAlert.Text = row.Cells["Id"].Value.ToString();
                txtAlertNo.Text = release = row.Cells["AlertNumber"].Value.ToString();
                string itemce = row.Cells["AlertAccess"].Value.ToString();
                if (itemce == "True")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
        }

        private void btnALertSil_Click(object sender, EventArgs e)
        {
            var alert = fs.OrAlerts.FirstOrDefault(x => x.Id == int.Parse(lblIDAlert.Text));
            var resulr = fs.OrAlerts.Remove(alert);
            dataGridView1.DataSource = fs.OrAlerts.Select(x => new { x.Id, x.AlertNumber, x.Name, x.AlertAccess, x.UpdateDate });
        }
    }
}
