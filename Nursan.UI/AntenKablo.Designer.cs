
namespace Nursan.UI

{
    partial class AntenKablo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AntenKablo));
            panel1 = new Panel();
            label2 = new Label();
            btnHome = new Button();
            panel2 = new Panel();
            label1 = new Label();
            panel3 = new Panel();
            listReferansSec = new ListView();
            panel4 = new Panel();
            textBox2 = new TextBox();
            label3 = new Label();
            panel5 = new Panel();
            textBox1 = new TextBox();
            label4 = new Label();
            panel6 = new Panel();
            listAntenCableIn = new ListBox();
            label5 = new Label();
            panel7 = new Panel();
            listAntenCabloOut = new ListBox();
            label6 = new Label();
            panel8 = new Panel();
            lstBiten = new ListBox();
            label7 = new Label();
            panel9 = new Panel();
            numSayi = new NumericUpDown();
            label8 = new Label();
            panel10 = new Panel();
            btnPrintConfig = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            panel8.SuspendLayout();
            panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSayi).BeginInit();
            panel10.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(45, 45, 48);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(btnHome);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1200, 60);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(20, 15);
            label2.Name = "label2";
            label2.Size = new Size(200, 30);
            label2.TabIndex = 1;
            label2.Text = "Anten Kablo Sistemi";
            // 
            // btnHome
            // 
            btnHome.BackColor = Color.FromArgb(0, 122, 204);
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnHome.ForeColor = Color.White;
            btnHome.Location = new Point(1100, 15);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(80, 30);
            btnHome.TabIndex = 0;
            btnHome.Text = "Ana Sayfa";
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(60, 60, 65);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 60);
            panel2.Name = "panel2";
            panel2.Size = new Size(1200, 40);
            panel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(0, 122, 204);
            label1.Location = new Point(20, 10);
            label1.Name = "label1";
            label1.Size = new Size(120, 21);
            label1.TabIndex = 0;
            label1.Text = "Seçilen Model:";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(37, 37, 38);
            panel3.Controls.Add(listReferansSec);
            panel3.Location = new Point(20, 120);
            panel3.Name = "panel3";
            panel3.Size = new Size(300, 400);
            panel3.TabIndex = 2;
            // 
            // listReferansSec
            // 
            listReferansSec.BackColor = Color.FromArgb(45, 45, 48);
            listReferansSec.BorderStyle = BorderStyle.None;
            listReferansSec.Dock = DockStyle.Fill;
            listReferansSec.Font = new Font("Segoe UI", 10F);
            listReferansSec.ForeColor = Color.White;
            listReferansSec.FullRowSelect = true;
            listReferansSec.HideSelection = false;
            listReferansSec.Location = new Point(0, 0);
            listReferansSec.Name = "listReferansSec";
            listReferansSec.Size = new Size(300, 400);
            listReferansSec.TabIndex = 0;
            listReferansSec.UseCompatibleStateImageBehavior = false;
            listReferansSec.View = View.Details;
            listReferansSec.SelectedIndexChanged += listView1_SelectedIndexChanged_1;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(37, 37, 38);
            panel4.Controls.Add(textBox2);
            panel4.Controls.Add(label3);
            panel4.Location = new Point(340, 120);
            panel4.Name = "panel4";
            panel4.Size = new Size(300, 80);
            panel4.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.FromArgb(45, 45, 48);
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Font = new Font("Segoe UI", 12F);
            textBox2.ForeColor = Color.White;
            textBox2.Location = new Point(10, 40);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(280, 29);
            textBox2.TabIndex = 1;
            textBox2.Text = "Arama...";
            textBox2.KeyUp += textBox2_KeyUp;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(0, 122, 204);
            label3.Location = new Point(10, 10);
            label3.Name = "label3";
            label3.Size = new Size(50, 19);
            label3.TabIndex = 0;
            label3.Text = "Arama:";
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(37, 37, 38);
            panel5.Controls.Add(textBox1);
            panel5.Controls.Add(label4);
            panel5.Location = new Point(340, 220);
            panel5.Name = "panel5";
            panel5.Size = new Size(300, 80);
            panel5.TabIndex = 4;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(45, 45, 48);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 12F);
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(10, 40);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(280, 29);
            textBox1.TabIndex = 1;
            textBox1.Text = "Barkod oku...";
            textBox1.KeyUp += textBox1_KeyUp;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(0, 122, 204);
            label4.Location = new Point(10, 10);
            label4.Name = "label4";
            label4.Size = new Size(100, 19);
            label4.TabIndex = 0;
            label4.Text = "Barkod Oku:";
            // 
            // panel6
            // 
            panel6.BackColor = Color.FromArgb(37, 37, 38);
            panel6.Controls.Add(listAntenCableIn);
            panel6.Controls.Add(label5);
            panel6.Location = new Point(660, 120);
            panel6.Name = "panel6";
            panel6.Size = new Size(250, 300);
            panel6.TabIndex = 5;
            // 
            // listAntenCableIn
            // 
            listAntenCableIn.BackColor = Color.FromArgb(45, 45, 48);
            listAntenCableIn.BorderStyle = BorderStyle.None;
            listAntenCableIn.Dock = DockStyle.Bottom;
            listAntenCableIn.Font = new Font("Segoe UI", 10F);
            listAntenCableIn.ForeColor = Color.White;
            listAntenCableIn.FormattingEnabled = true;
            listAntenCableIn.ItemHeight = 19;
            listAntenCableIn.Location = new Point(0, 30);
            listAntenCableIn.Name = "listAntenCableIn";
            listAntenCableIn.Size = new Size(250, 270);
            listAntenCableIn.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label5.ForeColor = Color.FromArgb(0, 122, 204);
            label5.Location = new Point(10, 10);
            label5.Name = "label5";
            label5.Size = new Size(120, 19);
            label5.TabIndex = 0;
            label5.Text = "Bekleyen Kablo:";
            // 
            // panel7
            // 
            panel7.BackColor = Color.FromArgb(37, 37, 38);
            panel7.Controls.Add(listAntenCabloOut);
            panel7.Controls.Add(label6);
            panel7.Location = new Point(930, 120);
            panel7.Name = "panel7";
            panel7.Size = new Size(250, 300);
            panel7.TabIndex = 6;
            // 
            // listAntenCabloOut
            // 
            listAntenCabloOut.BackColor = Color.FromArgb(45, 45, 48);
            listAntenCabloOut.BorderStyle = BorderStyle.None;
            listAntenCabloOut.Dock = DockStyle.Bottom;
            listAntenCabloOut.Font = new Font("Segoe UI", 10F);
            listAntenCabloOut.ForeColor = Color.White;
            listAntenCabloOut.FormattingEnabled = true;
            listAntenCabloOut.ItemHeight = 19;
            listAntenCabloOut.Location = new Point(0, 30);
            listAntenCabloOut.Name = "listAntenCabloOut";
            listAntenCabloOut.Size = new Size(250, 270);
            listAntenCabloOut.TabIndex = 1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label6.ForeColor = Color.FromArgb(0, 122, 204);
            label6.Location = new Point(10, 10);
            label6.Name = "label6";
            label6.Size = new Size(100, 19);
            label6.TabIndex = 0;
            label6.Text = "İşlenen Kablo:";
            // 
            // panel8
            // 
            panel8.BackColor = Color.FromArgb(37, 37, 38);
            panel8.Controls.Add(lstBiten);
            panel8.Controls.Add(label7);
            panel8.Location = new Point(660, 440);
            panel8.Name = "panel8";
            panel8.Size = new Size(520, 200);
            panel8.TabIndex = 7;
            // 
            // lstBiten
            // 
            lstBiten.BackColor = Color.FromArgb(45, 45, 48);
            lstBiten.BorderStyle = BorderStyle.None;
            lstBiten.Dock = DockStyle.Bottom;
            lstBiten.Font = new Font("Segoe UI", 10F);
            lstBiten.ForeColor = Color.White;
            lstBiten.FormattingEnabled = true;
            lstBiten.ItemHeight = 19;
            lstBiten.Location = new Point(0, 30);
            lstBiten.Name = "lstBiten";
            lstBiten.Size = new Size(520, 170);
            lstBiten.TabIndex = 1;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label7.ForeColor = Color.FromArgb(0, 122, 204);
            label7.Location = new Point(10, 10);
            label7.Name = "label7";
            label7.Size = new Size(80, 19);
            label7.TabIndex = 0;
            label7.Text = "Tamamlanan:";
            // 
            // panel9
            // 
            panel9.BackColor = Color.FromArgb(37, 37, 38);
            panel9.Controls.Add(numSayi);
            panel9.Controls.Add(label8);
            panel9.Location = new Point(340, 320);
            panel9.Name = "panel9";
            panel9.Size = new Size(300, 80);
            panel9.TabIndex = 8;
            // 
            // numSayi
            // 
            numSayi.BackColor = Color.FromArgb(45, 45, 48);
            numSayi.BorderStyle = BorderStyle.FixedSingle;
            numSayi.Font = new Font("Segoe UI", 12F);
            numSayi.ForeColor = Color.White;
            numSayi.Location = new Point(10, 40);
            numSayi.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numSayi.Name = "numSayi";
            numSayi.Size = new Size(280, 29);
            numSayi.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label8.ForeColor = Color.FromArgb(0, 122, 204);
            label8.Location = new Point(10, 10);
            label8.Name = "label8";
            label8.Size = new Size(40, 19);
            label8.TabIndex = 0;
            label8.Text = "Sayı:";
            // 
            // panel10
            // 
            panel10.BackColor = Color.FromArgb(37, 37, 38);
            panel10.Controls.Add(btnPrintConfig);
            panel10.Location = new Point(340, 420);
            panel10.Name = "panel10";
            panel10.Size = new Size(300, 60);
            panel10.TabIndex = 9;
            // 
            // btnPrintConfig
            // 
            btnPrintConfig.BackColor = Color.FromArgb(0, 122, 204);
            btnPrintConfig.Dock = DockStyle.Fill;
            btnPrintConfig.FlatAppearance.BorderSize = 0;
            btnPrintConfig.FlatStyle = FlatStyle.Flat;
            btnPrintConfig.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnPrintConfig.ForeColor = Color.White;
            btnPrintConfig.Location = new Point(0, 0);
            btnPrintConfig.Name = "btnPrintConfig";
            btnPrintConfig.Size = new Size(300, 60);
            btnPrintConfig.TabIndex = 0;
            btnPrintConfig.Text = "Yazdırma Ayarları";
            btnPrintConfig.UseVisualStyleBackColor = false;
            btnPrintConfig.Click += btnPrintConfig_Click;
            // 
            // AntenKablo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1200, 680);
            Controls.Add(panel10);
            Controls.Add(panel9);
            Controls.Add(panel8);
            Controls.Add(panel7);
            Controls.Add(panel6);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "AntenKablo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Anten Kablo Sistemi";
            FormClosing += AntenKablo_FormClosing;
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSayi).EndInit();
            panel10.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label2;
        private Button btnHome;
        private Panel panel2;
        private Label label1;
        private Panel panel3;
        private ListView listReferansSec;
        private Panel panel4;
        private TextBox textBox2;
        private Label label3;
        private Panel panel5;
        private TextBox textBox1;
        private Label label4;
        private Panel panel6;
        private ListBox listAntenCableIn;
        private Label label5;
        private Panel panel7;
        private ListBox listAntenCabloOut;
        private Label label6;
        private Panel panel8;
        private ListBox lstBiten;
        private Label label7;
        private Panel panel9;
        private NumericUpDown numSayi;
        private Label label8;
        private Panel panel10;
        private Button btnPrintConfig;
    }
}

