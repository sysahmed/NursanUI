namespace Nursan.UI
{
    public partial class ProgressBar : Form
    {
        public Delegate GeriDOnus;
        public event EventHandler Event;
        public ProgressBar()
        {
            InitializeComponent();
        }
        //DonanimTanitma frm = new DonanimTanitma();
        private void ProgressBar_Load(object sender, EventArgs e)
        {
            timer1.Start();
            //frm.Visible = false;
            //frm.Show();
            //Thread.Sleep(3000);
            //frm.Visible = true;
        }
        int move;
        private void timer1_Tick(object sender, EventArgs e)
        {
            panelSlider.Left += 10;
            if (panelSlider.Left > 250)
            {
                panelSlider.Left = 0;
            }
            else if (panelSlider.Left < 0)
            {
                move = 10;
            }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
