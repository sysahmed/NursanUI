namespace Nursan.UI
{
    partial class Staring
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Staring));
            txtBarcode = new TextBox();
            fileSystemWatcher1 = new FileSystemWatcher();
            tableLayoutPanel1 = new TableLayoutPanel();
            pictureBox1 = new PictureBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            pictureBox2 = new PictureBox();
            lblMessages = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // txtBarcode
            // 
            txtBarcode.BorderStyle = BorderStyle.FixedSingle;
            txtBarcode.Dock = DockStyle.Top;
            txtBarcode.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            txtBarcode.Location = new Point(1065, 550);
            txtBarcode.Margin = new Padding(4, 3, 4, 3);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "Сканирай баркод за вход...";
            txtBarcode.Size = new Size(329, 40);
            txtBarcode.TabIndex = 0;
            txtBarcode.KeyUp += txtBarcode_KeyUp;
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56.5019F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.9467678F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.55133F));
            tableLayoutPanel1.Controls.Add(pictureBox1, 1, 1);
            tableLayoutPanel1.Controls.Add(txtBarcode, 1, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(lblMessages, 1, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20.833334F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 49.2283936F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel1.Size = new Size(1879, 1080);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.ErrorImage = Properties.Resources.avatar_2x;
            pictureBox1.Image = Properties.Resources.avatar_2x;
            pictureBox1.InitialImage = Properties.Resources.avatar_2x;
            pictureBox1.Location = new Point(1065, 328);
            pictureBox1.Margin = new Padding(4, 5, 4, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(329, 214);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.7941551F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40.7878036F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel2.Controls.Add(pictureBox2, 1, 2);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(4, 5);
            tableLayoutPanel2.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel2.Size = new Size(1053, 313);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Fill;
            pictureBox2.ErrorImage = Properties.Resources.nursan_logo;
            pictureBox2.Image = Properties.Resources.nursan_logo;
            pictureBox2.Location = new Point(275, 213);
            pictureBox2.Margin = new Padding(4, 5, 4, 5);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(421, 95);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // lblMessages
            // 
            lblMessages.AutoSize = true;
            lblMessages.Dock = DockStyle.Fill;
            lblMessages.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblMessages.ForeColor = Color.FromArgb(220, 53, 69);
            lblMessages.Location = new Point(1065, 0);
            lblMessages.Margin = new Padding(4, 0, 4, 0);
            lblMessages.Name = "lblMessages";
            lblMessages.Size = new Size(329, 323);
            lblMessages.TabIndex = 3;
            lblMessages.Text = "Очаквам баркод...";
            lblMessages.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 42F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(220, 53, 69);
            label1.Location = new Point(4, 547);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(1053, 533);
            label1.TabIndex = 5;
            label1.Text = "NURSAN\nПРОИЗВОДСТВЕНА СИСТЕМА";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Staring
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            BackgroundImage = Properties.Resources.FROM1;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1879, 1080);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "Staring";
            Text = "Nursan - Вход в системата";
            WindowState = FormWindowState.Maximized;
            Load += Staring_Load;
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtBarcode;
        private FileSystemWatcher fileSystemWatcher1;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox1;
        private TableLayoutPanel tableLayoutPanel2;
        private PictureBox pictureBox2;
        private Label lblMessages;
        private Label label1;
    }
}