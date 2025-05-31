using Nursan.Business.Services;
using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Personal.Valadation;
using Nursan.Validations.ValidationCode;
using System.ComponentModel;


namespace Nursan.UI
{
    public class ScreenSaverForm : Form
    {
        private Point MouseXY;

        private int _ScreenNumber;

        //private XMLIslemi islem = new XMLIslemi();

        private IContainer components = null;
        public event EventHandler TetikSicil;
        public Label lblMessage;
        private System.Windows.Forms.Timer timer1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Button button1;
        private ListBox listBox1;
        public PictureBox pictureBox1;
        PersonalValidasyonu pers;
        string _vardiya;
        private List<UrPersonalTakib> urPersonalTakibs;
        private UrIstasyon istasyonce;
        DateTime date;
        private Button button2;
        //SayiIzlemeSIcilBagizliService sayim;
        TorkService tork;
        public ScreenSaverForm(int scrn, string vardiya)
        {
            this.InitializeComponent();
            _ScreenNumber = scrn;
            _vardiya = vardiya;
            pers = new PersonalValidasyonu(new UnitOfWorPersonal(new PersonalContext()), new UnitOfWork(new UretimOtomasyonContext()));
            this.timer1.Interval = XMLIslemi.XmlScreenSaniye();
            timer1.Enabled = true;
            tork = new TorkService(new UnitOfWork(new UretimOtomasyonContext()),new UrVardiya {Name=vardiya } );
            istasyonce =tork.GetIstasyon();
            urPersonalTakibs = pers.GetPersonalTakib(istasyonce).Data;
            date = OtherTools.GetValuesDatetime();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ScreenSaverForm));
            lblMessage = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            pictureBox1 = new PictureBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            listBox1 = new ListBox();
            ((ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Font = new Font("Microsoft Sans Serif", 48F, FontStyle.Bold, GraphicsUnit.Point);
            lblMessage.ForeColor = Color.Lime;
            lblMessage.Location = new Point(3, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(1270, 120);
            lblMessage.TabIndex = 0;
            lblMessage.Text = "Text";
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            lblMessage.UseWaitCursor = true;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 10000;
            timer1.Tick += timer1_Tick;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(3, 333);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1270, 365);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.UseWaitCursor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblMessage, 0, 0);
            tableLayoutPanel1.Controls.Add(pictureBox1, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 17.2185421F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 29.9337749F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 52.7152328F));
            tableLayoutPanel1.Size = new Size(1276, 701);
            tableLayoutPanel1.TabIndex = 2;
            tableLayoutPanel1.UseWaitCursor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 657F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel2.Controls.Add(listBox1, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 123);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(1270, 204);
            tableLayoutPanel2.TabIndex = 2;
            tableLayoutPanel2.UseWaitCursor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48.92788F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 51.0721245F));
            tableLayoutPanel3.Controls.Add(button1, 0, 0);
            tableLayoutPanel3.Controls.Add(button2, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(607, 198);
            tableLayoutPanel3.TabIndex = 0;
            tableLayoutPanel3.UseWaitCursor = true;

            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Arial Black", 21.75F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = Color.Lime;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(290, 192);
            button1.TabIndex = 0;
            button1.Text = "Sicil Okut";
            button1.UseVisualStyleBackColor = true;
            button1.UseWaitCursor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Arial Black", 21.75F, FontStyle.Bold, GraphicsUnit.Point);
            button2.ForeColor = Color.Red;
            button2.Location = new Point(299, 3);
            button2.Name = "button2";
            button2.Size = new Size(305, 192);
            button2.TabIndex = 1;
            button2.Text = "EXIT";
            button2.UseVisualStyleBackColor = true;
            button2.UseWaitCursor = true;
            button2.Click += button2_Click;
            // 
            // listBox1
            // 
            listBox1.BackColor = SystemColors.MenuText;
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Arial", 21.75F, FontStyle.Bold, GraphicsUnit.Point);
            listBox1.ForeColor = Color.Lime;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 34;
            listBox1.Location = new Point(616, 3);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(651, 198);
            listBox1.TabIndex = 1;
            listBox1.UseWaitCursor = true;
            // 
            // ScreenSaverForm
            // 
            AutoScaleBaseSize = new Size(6, 16);
            BackColor = Color.Black;
            ClientSize = new Size(1276, 701);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ScreenSaverForm";
            Text = "ReportNBG-Screen";
            WindowState = FormWindowState.Maximized;
            Load += ScreenSaverForm_Load;
            ((ISupportInitialize)pictureBox1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void OnMouseEvent(object sender, MouseEventArgs e)
        {
            //if (!this.MouseXY.IsEmpty)
            //{
            //    if (this.MouseXY != new Point(e.X, e.Y))
            //    {
            //        base.Close();
            //    }
            //    if (e.Clicks > 0)
            //    {
            //        base.Close();
            //    }
            //}
            //this.MouseXY = new Point(e.X, e.Y);
        }

        private void ScreenSaverForm_KeyDown(object sender, KeyEventArgs e)
        {
            base.Close();
        }
        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            try
            {
                base.Bounds = Screen.AllScreens[this._ScreenNumber].Bounds;
                base.TopMost = true;

                var veriler = urPersonalTakibs?.Count() == 0 ? null : urPersonalTakibs;
                if (veriler != null)
                {
                    string[] parca = veriler.First().DayOfYear.Split('*');
                    string datece = $"{date.Year}{date.Month}{date.Day}";

                    listBox1.Items.Clear();

                    foreach (var item in urPersonalTakibs)
                    {
                        string itemText = $"{item.Sicil}-{item.FullName}-{GitSytemDeAyiklaVesay(item.Sicil)}";
                        listBox1.Items.Add(itemText);
                    }

                    var mainForm = Application.OpenForms.OfType<ElTest>().FirstOrDefault();
                    if (mainForm != null && mainForm.lblCountProductions != null)
                    {
                        if (mainForm.InvokeRequired)
                        {
                            mainForm.Invoke(new Action(() =>
                            {
                                //mainForm.lblCountProductions.AutoSize = true;
                                //mainForm.lblCountProductions.MaximumSize = new Size(mainForm.Width - 20, 0);
                                mainForm.lblCountProductions.Text = string.Join(" | ", listBox1.Items.Cast<string>());
                                
                                // Преоразмеряваме формата според лейбъла
                                int newWidth = mainForm.lblCountProductions.Width + 40;
                                mainForm.Width = Math.Max(newWidth, mainForm.MinimumSize.Width);
                            }));
                        }
                        else
                        {
                            //mainForm.lblCountProductions.AutoSize = true;
                            //mainForm.lblCountProductions.MaximumSize = new Size(mainForm.Width - 20, 0);
                            mainForm.lblCountProductions.Text = string.Join(" | ", listBox1.Items.Cast<string>());
                            
                            // Преоразмеряваме формата според лейбъла
                            int newWidth = mainForm.lblCountProductions.Width + 40;
                            mainForm.Width = Math.Max(newWidth, mainForm.MinimumSize.Width);
                        }
                    }
                }
                else
                {
                    timer1.Stop();
                }

                timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при зареждане на формата: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GitSytemDeAyiklaVesay(string? sicil)
        {
            var result = SayiIzlemeSIcilBagizliService.SayiHesapla(sicil, _vardiya);
            return  ((int)istasyonce.Realadet + result);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
                var items = listBox1.Items.Cast<string>().ToList();
                var mainForm = Application.OpenForms.OfType<ElTest>().FirstOrDefault();
                if (mainForm != null && mainForm.lblCountProductions != null)
                {
                    if (mainForm.InvokeRequired)
                    {
                        mainForm.Invoke(new Action(() =>
                        {
                            //mainForm.lblCountProductions.AutoSize = true;
                            //mainForm.lblCountProductions.MaximumSize = new Size(mainForm.Width - 20, 0);
                            mainForm.lblCountProductions.Text = string.Join(Environment.NewLine, items);
                            
                            // Преоразмеряваме формата според лейбъла
                            int newWidth = mainForm.lblCountProductions.Width + 40;
                            mainForm.Width = Math.Max(newWidth, mainForm.MinimumSize.Width);
                        }));
                    }
                    else
                    {
                        //mainForm.lblCountProductions.AutoSize = true;
                        //mainForm.lblCountProductions.MaximumSize = new Size(mainForm.Width - 20, 0);
                        mainForm.lblCountProductions.Text = string.Join(Environment.NewLine, items);
                        
                        // Преоразмеряваме формата според лейбъла
                        int newWidth = mainForm.lblCountProductions.Width + 40;
                        mainForm.Width = Math.Max(newWidth, mainForm.MinimumSize.Width);
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при затваряне на формата: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TetikSicil(this, new EventArgs());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
    }
}