
namespace Nursan.UI

{
    partial class AntenKablo
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AntenKablo));
            label1 = new Label();
            pictureBox1 = new PictureBox();
            btnPrintConfig = new Button();
            listReferansSec = new ListView();
            lstBiten = new ListView();
            dataGridView1 = new DataGridView();
            label2 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            listAntenCabloOut = new ListBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            textBox1 = new TextBox();
            listAntenCableIn = new ListBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            btnHome = new Button();
            cbFamily = new ComboBox();
            numSayi = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSayi).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold);
            label1.ForeColor = Color.Yellow;
            label1.Location = new Point(3, 33);
            label1.Name = "label1";
            label1.Size = new Size(1113, 34);
            label1.TabIndex = 1;
            label1.Text = "REFERANS";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = Properties.Resources.nursan_logo;
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1113, 27);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // btnPrintConfig
            // 
            btnPrintConfig.BackColor = Color.Transparent;
            btnPrintConfig.Dock = DockStyle.Fill;
            btnPrintConfig.FlatStyle = FlatStyle.Flat;
            btnPrintConfig.Font = new Font("Nirmala UI", 9.75F, FontStyle.Bold);
            btnPrintConfig.ForeColor = Color.FromArgb(0, 126, 249);
            btnPrintConfig.Location = new Point(1122, 3);
            btnPrintConfig.Name = "btnPrintConfig";
            btnPrintConfig.Size = new Size(133, 27);
            btnPrintConfig.TabIndex = 5;
            btnPrintConfig.Text = "Print Config";
            btnPrintConfig.TextAlign = ContentAlignment.MiddleRight;
            btnPrintConfig.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnPrintConfig.UseVisualStyleBackColor = false;
            // 
            // listReferansSec
            // 
            listReferansSec.BackColor = Color.FromArgb(128, 255, 128);
            listReferansSec.Dock = DockStyle.Fill;
            listReferansSec.Font = new Font("Century", 26.25F, FontStyle.Bold);
            listReferansSec.FullRowSelect = true;
            listReferansSec.GridLines = true;
            listReferansSec.LabelWrap = false;
            listReferansSec.Location = new Point(330, 3);
            listReferansSec.MultiSelect = false;
            listReferansSec.Name = "listReferansSec";
            listReferansSec.Size = new Size(1066, 252);
            listReferansSec.Sorting = SortOrder.Ascending;
            listReferansSec.TabIndex = 3;
            listReferansSec.UseCompatibleStateImageBehavior = false;
            listReferansSec.View = View.List;
            listReferansSec.SelectedIndexChanged += listView1_SelectedIndexChanged_1;
            // 
            // lstBiten
            // 
            lstBiten.BackColor = Color.FromArgb(24, 22, 34);
            lstBiten.Dock = DockStyle.Fill;
            lstBiten.Font = new Font("Century", 26.25F, FontStyle.Bold | FontStyle.Strikeout);
            lstBiten.ForeColor = SystemColors.InactiveCaption;
            lstBiten.FullRowSelect = true;
            lstBiten.GridLines = true;
            lstBiten.LabelWrap = false;
            lstBiten.Location = new Point(3, 615);
            lstBiten.MultiSelect = false;
            lstBiten.Name = "lstBiten";
            lstBiten.Size = new Size(1399, 114);
            lstBiten.Sorting = SortOrder.Ascending;
            lstBiten.TabIndex = 4;
            lstBiten.UseCompatibleStateImageBehavior = false;
            lstBiten.View = View.List;
            // 
            // dataGridView1
            // 
            dataGridView1.AccessibleRole = AccessibleRole.TitleBar;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = Color.LightGray;
            dataGridViewCellStyle1.ForeColor = Color.Red;
            dataGridViewCellStyle1.SelectionForeColor = Color.Red;
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.AppWorkspace;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.GridColor = SystemColors.ActiveCaption;
            dataGridView1.Location = new Point(330, 261);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1066, 269);
            dataGridView1.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(128, 255, 128);
            label2.Location = new Point(48, 151);
            label2.Name = "label2";
            label2.Size = new Size(64, 25);
            label2.TabIndex = 8;
            label2.Text = "label2";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Controls.Add(lstBiten, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 73.72549F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.3398685F));
            tableLayoutPanel1.Size = new Size(1405, 732);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.3972244F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.6027756F));
            tableLayoutPanel2.Controls.Add(listAntenCabloOut, 0, 1);
            tableLayoutPanel2.Controls.Add(listReferansSec, 10, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel4, 0, 0);
            tableLayoutPanel2.Controls.Add(dataGridView1, 1, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 76);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 48.57143F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 51.42857F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
            tableLayoutPanel2.Size = new Size(1399, 533);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // listAntenCabloOut
            // 
            listAntenCabloOut.BackColor = Color.Yellow;
            listAntenCabloOut.Dock = DockStyle.Fill;
            listAntenCabloOut.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            listAntenCabloOut.FormattingEnabled = true;
            listAntenCabloOut.ItemHeight = 32;
            listAntenCabloOut.Location = new Point(3, 259);
            listAntenCabloOut.Margin = new Padding(3, 1, 3, 1);
            listAntenCabloOut.Name = "listAntenCabloOut";
            listAntenCabloOut.Size = new Size(321, 273);
            listAntenCabloOut.TabIndex = 8;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(textBox1, 0, 0);
            tableLayoutPanel4.Controls.Add(listAntenCableIn, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tableLayoutPanel4.Size = new Size(321, 252);
            tableLayoutPanel4.TabIndex = 5;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Dock = DockStyle.Fill;
            textBox1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            textBox1.Location = new Point(0, 11);
            textBox1.Margin = new Padding(0, 11, 0, 11);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(321, 28);
            textBox1.TabIndex = 0;
            textBox1.KeyUp += textBox1_KeyUp;
            // 
            // listAntenCableIn
            // 
            listAntenCableIn.BackColor = Color.FromArgb(255, 128, 255);
            listAntenCableIn.Dock = DockStyle.Fill;
            listAntenCableIn.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            listAntenCableIn.FormattingEnabled = true;
            listAntenCableIn.ItemHeight = 32;
            listAntenCableIn.Location = new Point(0, 50);
            listAntenCableIn.Margin = new Padding(0);
            listAntenCableIn.Name = "listAntenCableIn";
            listAntenCableIn.Size = new Size(321, 202);
            listAntenCableIn.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.Controls.Add(btnHome, 2, 0);
            tableLayoutPanel3.Controls.Add(label1, 0, 1);
            tableLayoutPanel3.Controls.Add(btnPrintConfig, 1, 0);
            tableLayoutPanel3.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel3.Controls.Add(cbFamily, 1, 1);
            tableLayoutPanel3.Controls.Add(numSayi, 2, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(1399, 67);
            tableLayoutPanel3.TabIndex = 9;
            // 
            // btnHome
            // 
            btnHome.BackColor = Color.Transparent;
            btnHome.Dock = DockStyle.Fill;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Nirmala UI", 9.75F, FontStyle.Bold);
            btnHome.ForeColor = Color.FromArgb(0, 126, 249);
            btnHome.Location = new Point(1261, 3);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(135, 27);
            btnHome.TabIndex = 5;
            btnHome.Text = "Home";
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;
            // 
            // cbFamily
            // 
            cbFamily.BackColor = Color.FromArgb(128, 128, 255);
            cbFamily.Dock = DockStyle.Fill;
            cbFamily.FlatStyle = FlatStyle.Flat;
            cbFamily.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            cbFamily.ForeColor = Color.OrangeRed;
            cbFamily.FormattingEnabled = true;
            cbFamily.Location = new Point(1122, 36);
            cbFamily.Name = "cbFamily";
            cbFamily.Size = new Size(133, 29);
            cbFamily.TabIndex = 7;
            cbFamily.SelectedIndexChanged += cbKonveyor_SelectedIndexChanged;
            cbFamily.KeyUp += cbFamily_KeyUp;
            // 
            // numSayi
            // 
            numSayi.BackColor = Color.FromArgb(128, 128, 255);
            numSayi.Dock = DockStyle.Fill;
            numSayi.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            numSayi.Location = new Point(1261, 36);
            numSayi.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numSayi.Name = "numSayi";
            numSayi.Size = new Size(135, 29);
            numSayi.TabIndex = 8;
            // 
            // AntenKablo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 30, 54);
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1405, 732);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(label2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AntenKablo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NbgSystem";
            WindowState = FormWindowState.Maximized;
            FormClosing += AntenKablo_FormClosing;
            FormClosed += AnaSayfa_FormClosed;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSayi).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Button btnPrintConfig;
        private PictureBox pictureBox1;
        private ListView listReferansSec;
        private ListView lstBiten;
        private DataGridView dataGridView1;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private ComboBox cbKonveyor;
        private ComboBox cbFamily;
        private TableLayoutPanel tableLayoutPanel4;
        private ListBox listAntenCabloOut;
        private ListBox listAntenCableIn;
        public TextBox textBox1;
        private Button btnHome;
        private NumericUpDown numSayi;
    }
}

