namespace Nursan.Licenzing
{
    public partial class Lisanslama : Form
    {
        KilitYaratma klt = new KilitYaratma();
        RegistrileriAra reg = new RegistrileriAra(); FingerPrint fng = new FingerPrint();
        public Lisanslama()
        {
            InitializeComponent();
        }

        private void btnActiv_Click(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToString("yyy-MM-dd");
            string gelsi = fng.Value();
            if (gelsi == txtActivaciq.Text)
            {
                //reg.regYaz(date,klt.Value());
                if (reg.regYaz(date, klt.Value()) == "hello")
                {
                    if (MessageBox.Show("Активацията е успешна.", "Активация:", MessageBoxButtons.OK,
                            MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        Hide();
                    }
                }
            }
            else
            {
                MessageBox.Show("Кодът за активиране е грешен!", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void Lisanslama_Load(object sender, EventArgs e)
        {
            txtSeriNomer.Text = reg.regBac(klt.Value());
            if (txtSeriNomer.Text == "yes!!!")
            {

            }
            else
            {
                MessageBox.Show("Трябва да въведете продуктов ключ!", "Грешка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
        }
    }
}