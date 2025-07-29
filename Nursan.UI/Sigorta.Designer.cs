
namespace Nursan.UI

{
    partial class Sigorta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sigorta));
            label1 = new Label();
            pictureBox1 = new PictureBox();
            btnSeting = new Button();
            listView1 = new ListView();
            lstBiten = new ListView();
            dataGridView1 = new DataGridView();
            label2 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            button1 = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            cbFamily = new ComboBox();
            textBox1 = new TextBox();
            numericUpDown1 = new NumericUpDown();
            lblCount = new Label();
            numericUpDownPrintCount = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPrintCount).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(3, 37);
            label1.Name = "label1";
            label1.Size = new Size(1124, 38);
            label1.TabIndex = 1;
            label1.Text = "REFERANS";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = Properties.Resources.nursan_logo;
            pictureBox1.Location = new Point(3, 4);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1124, 29);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // btnSeting
            // 
            btnSeting.BackColor = Color.FromArgb(0, 122, 204);
            btnSeting.Dock = DockStyle.Fill;
            btnSeting.FlatAppearance.BorderSize = 0;
            btnSeting.FlatStyle = FlatStyle.Flat;
            btnSeting.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnSeting.ForeColor = Color.White;
            btnSeting.Location = new Point(1395, 41);
            btnSeting.Margin = new Padding(3, 4, 3, 4);
            btnSeting.Name = "btnSeting";
            btnSeting.Size = new Size(135, 30);
            btnSeting.TabIndex = 5;
            btnSeting.Text = "Seting";
            btnSeting.TextAlign = ContentAlignment.MiddleLeft;
            btnSeting.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSeting.UseVisualStyleBackColor = false;
            // 
            // listView1
            // 
            listView1.BackColor = Color.FromArgb(45, 45, 48);
            listView1.BorderStyle = BorderStyle.None;
            listView1.Dock = DockStyle.Fill;
            listView1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            listView1.ForeColor = Color.Lime;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.LabelWrap = false;
            listView1.Location = new Point(3, 4);
            listView1.Margin = new Padding(3, 4, 3, 4);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new Size(1169, 193);
            listView1.Sorting = SortOrder.Ascending;
            listView1.TabIndex = 3;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.List;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged_1;
            // 
            // lstBiten
            // 
            lstBiten.BackColor = Color.FromArgb(30, 30, 30);
            lstBiten.BorderStyle = BorderStyle.None;
            lstBiten.Dock = DockStyle.Fill;
            lstBiten.Font = new Font("Segoe UI", 14F, FontStyle.Bold | FontStyle.Strikeout);
            lstBiten.ForeColor = Color.Gray;
            lstBiten.FullRowSelect = true;
            lstBiten.GridLines = true;
            lstBiten.LabelWrap = false;
            lstBiten.Location = new Point(3, 205);
            lstBiten.Margin = new Padding(3, 4, 3, 4);
            lstBiten.MultiSelect = false;
            lstBiten.Name = "lstBiten";
            lstBiten.Size = new Size(1169, 205);
            lstBiten.Sorting = SortOrder.Ascending;
            lstBiten.TabIndex = 4;
            lstBiten.UseCompatibleStateImageBehavior = false;
            lstBiten.View = View.List;
            // 
            // dataGridView1
            // 
            dataGridView1.AccessibleRole = AccessibleRole.TitleBar;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(60, 60, 60);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.FromArgb(45, 45, 48);
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(30, 30, 30);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.GridColor = Color.FromArgb(60, 60, 60);
            dataGridView1.Location = new Point(3, 509);
            dataGridView1.Margin = new Padding(3, 4, 3, 4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1533, 327);
            dataGridView1.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            label2.ForeColor = Color.Lime;
            label2.Location = new Point(79, 337);
            label2.Name = "label2";
            label2.Size = new Size(64, 25);
            label2.TabIndex = 8;
            label2.Text = "label2";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(30, 30, 30);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(dataGridView1, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50.3268F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 39.73856F));
            tableLayoutPanel1.Size = new Size(1539, 840);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.FromArgb(45, 45, 48);
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.7033F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.2967F));
            tableLayoutPanel2.Controls.Add(lstBiten, 0, 1);
            tableLayoutPanel2.Controls.Add(button1, 1, 0);
            tableLayoutPanel2.Controls.Add(listView1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 87);
            tableLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 48.57143F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 51.42857F));
            tableLayoutPanel2.Size = new Size(1533, 414);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 122, 204);
            button1.Dock = DockStyle.Fill;
            button1.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 100, 180);
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 140, 220);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Image = Properties.Resources.icons8_barcode_ios_16_96;
            button1.ImageAlign = ContentAlignment.BottomCenter;
            button1.Location = new Point(1178, 4);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Padding = new Padding(3, 4, 3, 4);
            button1.Size = new Size(352, 193);
            button1.TabIndex = 2;
            button1.Text = "ETIKET BAS";
            button1.TextAlign = ContentAlignment.TopCenter;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.FromArgb(30, 30, 30);
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 73.69963F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7.765568F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.37729F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.08486F));
            tableLayoutPanel3.Controls.Add(label1, 0, 1);
            tableLayoutPanel3.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel3.Controls.Add(btnSeting, 3, 1);
            tableLayoutPanel3.Controls.Add(cbFamily, 2, 1);
            tableLayoutPanel3.Controls.Add(textBox1, 1, 1);
            tableLayoutPanel3.Controls.Add(numericUpDown1, 1, 0);
            tableLayoutPanel3.Controls.Add(lblCount, 2, 0);
            tableLayoutPanel3.Controls.Add(numericUpDownPrintCount, 3, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 4);
            tableLayoutPanel3.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(1533, 75);
            tableLayoutPanel3.TabIndex = 9;
            // 
            // cbFamily
            // 
            cbFamily.BackColor = Color.FromArgb(60, 60, 60);
            cbFamily.Dock = DockStyle.Fill;
            cbFamily.FlatStyle = FlatStyle.Flat;
            cbFamily.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            cbFamily.ForeColor = Color.White;
            cbFamily.FormattingEnabled = true;
            cbFamily.Location = new Point(1252, 41);
            cbFamily.Margin = new Padding(3, 4, 3, 4);
            cbFamily.Name = "cbFamily";
            cbFamily.Size = new Size(137, 29);
            cbFamily.TabIndex = 7;
            cbFamily.SelectedIndexChanged += cbKonveyor_SelectedIndexChanged;
            cbFamily.KeyUp += cbFamily_KeyUp;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(60, 60, 60);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Dock = DockStyle.Fill;
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(1133, 41);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(113, 29);
            textBox1.TabIndex = 8;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyUp += textBox1_KeyUp;
            // 
            // numericUpDown1
            // 
            numericUpDown1.BackColor = Color.FromArgb(60, 60, 60);
            numericUpDown1.BorderStyle = BorderStyle.None;
            numericUpDown1.Dock = DockStyle.Fill;
            numericUpDown1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            numericUpDown1.ForeColor = Color.White;
            numericUpDown1.Location = new Point(1133, 4);
            numericUpDown1.Margin = new Padding(3, 4, 3, 4);
            numericUpDown1.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(113, 29);
            numericUpDown1.TabIndex = 9;
            // 
            // lblCount
            // 
            lblCount.BackColor = Color.FromArgb(45, 45, 48);
            lblCount.Dock = DockStyle.Fill;
            lblCount.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblCount.ForeColor = Color.White;
            lblCount.Location = new Point(1252, 0);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(137, 37);
            lblCount.TabIndex = 10;
            lblCount.Text = "Count";
            lblCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // numericUpDownPrintCount
            // 
            numericUpDownPrintCount.BackColor = Color.FromArgb(60, 60, 60);
            numericUpDownPrintCount.BorderStyle = BorderStyle.None;
            numericUpDownPrintCount.Dock = DockStyle.Fill;
            numericUpDownPrintCount.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            numericUpDownPrintCount.ForeColor = Color.White;
            numericUpDownPrintCount.Location = new Point(1395, 4);
            numericUpDownPrintCount.Margin = new Padding(3, 4, 3, 4);
            numericUpDownPrintCount.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numericUpDownPrintCount.Name = "numericUpDownPrintCount";
            numericUpDownPrintCount.Size = new Size(135, 29);
            numericUpDownPrintCount.TabIndex = 1;
            // 
            // Sigorta
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1539, 840);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(label2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "Sigorta";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sigorta Sistemi";
            WindowState = FormWindowState.Maximized;
            FormClosed += AnaSayfa_FormClosed;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPrintCount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Button btnSeting;
        private PictureBox pictureBox1;
        private ListView listView1;
        private ListView lstBiten;
        private DataGridView dataGridView1;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private ComboBox cbKonveyor;
        private ComboBox cbFamily;
        private TextBox textBox1;
        private NumericUpDown numericUpDown1;
        public Button button1;
        private NumericUpDown numericUpDownPrintCount;
        private Label lblCount;
    }
}

