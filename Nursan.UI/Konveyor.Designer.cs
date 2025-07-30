namespace Nursan.UI
{
    partial class Konveyor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Konveyor));
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel2 = new Panel();
            lblMessage = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            listBox1 = new ListBox();
            txtBarcode = new TextBox();
            lblToplam = new Label();
            tableLayoutPanel5 = new TableLayoutPanel();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            lblOrtalama = new Label();
            label5 = new Label();
            lblToplama = new Label();
            label3 = new Label();
            lblVardiya = new Label();
            label1 = new Label();
            panel11 = new Panel();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(30, 30, 30);
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1347, 630);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.FromArgb(30, 30, 30);
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel2, 0, 1);
            tableLayoutPanel2.Controls.Add(lblMessage, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(205, 2);
            tableLayoutPanel2.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            tableLayoutPanel2.Size = new Size(1139, 626);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(45, 45, 45);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 95);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(1133, 529);
            panel2.TabIndex = 1;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.BackColor = Color.FromArgb(20, 20, 20);
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblMessage.ForeColor = Color.FromArgb(0, 255, 128);
            lblMessage.Location = new Point(4, 0);
            lblMessage.Margin = new Padding(4, 0, 4, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(1131, 93);
            lblMessage.TabIndex = 2;
            lblMessage.Text = "Gelen Barcode Okutun";
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.FromArgb(30, 30, 30);
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 18F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel5, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 2);
            tableLayoutPanel3.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 34.14365F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 65.85635F));
            tableLayoutPanel3.Size = new Size(196, 626);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.FromArgb(40, 40, 40);
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(listBox1, 0, 2);
            tableLayoutPanel4.Controls.Add(txtBarcode, 0, 1);
            tableLayoutPanel4.Controls.Add(lblToplam, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 2);
            tableLayoutPanel4.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 12.54125F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 54.45544F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tableLayoutPanel4.Size = new Size(190, 209);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.FromArgb(60, 60, 60);
            listBox1.BorderStyle = BorderStyle.None;
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            listBox1.ForeColor = Color.FromArgb(255, 255, 255);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(3, 97);
            listBox1.Margin = new Padding(3, 2, 3, 2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(184, 110);
            listBox1.TabIndex = 3;
            // 
            // txtBarcode
            // 
            txtBarcode.BackColor = Color.FromArgb(60, 60, 60);
            txtBarcode.BorderStyle = BorderStyle.FixedSingle;
            txtBarcode.Dock = DockStyle.Fill;
            txtBarcode.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            txtBarcode.ForeColor = Color.FromArgb(255, 255, 255);
            txtBarcode.Location = new Point(3, 71);
            txtBarcode.Margin = new Padding(3, 2, 3, 2);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(184, 36);
            txtBarcode.TabIndex = 1;
            txtBarcode.TextAlign = HorizontalAlignment.Center;
            txtBarcode.KeyUp += txtBarcode_KeyUp;
            // 
            // lblToplam
            // 
            lblToplam.AutoSize = true;
            lblToplam.BackColor = Color.FromArgb(70, 70, 70);
            lblToplam.Dock = DockStyle.Fill;
            lblToplam.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblToplam.ForeColor = Color.FromArgb(255, 255, 128);
            lblToplam.Location = new Point(3, 0);
            lblToplam.Name = "lblToplam";
            lblToplam.Size = new Size(184, 69);
            lblToplam.TabIndex = 0;
            lblToplam.Text = "Donanim";
            lblToplam.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.FromArgb(40, 40, 40);
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Controls.Add(label9, 0, 8);
            tableLayoutPanel5.Controls.Add(label8, 0, 7);
            tableLayoutPanel5.Controls.Add(label7, 0, 6);
            tableLayoutPanel5.Controls.Add(lblOrtalama, 0, 5);
            tableLayoutPanel5.Controls.Add(label5, 0, 4);
            tableLayoutPanel5.Controls.Add(lblToplama, 0, 3);
            tableLayoutPanel5.Controls.Add(label3, 0, 2);
            tableLayoutPanel5.Controls.Add(lblVardiya, 0, 1);
            tableLayoutPanel5.Controls.Add(label1, 0, 0);
            tableLayoutPanel5.Controls.Add(panel11, 0, 9);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 216);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 10;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.Size = new Size(190, 407);
            tableLayoutPanel5.TabIndex = 1;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.FromArgb(50, 50, 50);
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label9.ForeColor = Color.FromArgb(0, 255, 128);
            label9.Location = new Point(3, 320);
            label9.Name = "label9";
            label9.Size = new Size(184, 40);
            label9.TabIndex = 0;
            label9.Text = "000";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            label9.Visible = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.FromArgb(50, 50, 50);
            label8.Dock = DockStyle.Fill;
            label8.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label8.ForeColor = Color.FromArgb(0, 255, 128);
            label8.Location = new Point(3, 280);
            label8.Name = "label8";
            label8.Size = new Size(184, 40);
            label8.TabIndex = 0;
            label8.Text = "000";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            label8.Visible = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(50, 50, 50);
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label7.ForeColor = Color.FromArgb(0, 255, 128);
            label7.Location = new Point(3, 240);
            label7.Name = "label7";
            label7.Size = new Size(184, 40);
            label7.TabIndex = 0;
            label7.Text = "000";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            label7.Visible = false;
            // 
            // lblOrtalama
            // 
            lblOrtalama.AutoSize = true;
            lblOrtalama.BackColor = Color.FromArgb(50, 50, 50);
            lblOrtalama.Dock = DockStyle.Fill;
            lblOrtalama.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblOrtalama.ForeColor = Color.FromArgb(0, 255, 128);
            lblOrtalama.Location = new Point(3, 200);
            lblOrtalama.Name = "lblOrtalama";
            lblOrtalama.Size = new Size(184, 40);
            lblOrtalama.TabIndex = 0;
            lblOrtalama.Text = "000";
            lblOrtalama.TextAlign = ContentAlignment.MiddleCenter;
            lblOrtalama.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(50, 50, 50);
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label5.ForeColor = Color.FromArgb(255, 255, 255);
            label5.Location = new Point(3, 160);
            label5.Name = "label5";
            label5.Size = new Size(184, 40);
            label5.TabIndex = 0;
            label5.Text = "ORTALAMA";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            label5.Visible = false;
            // 
            // lblToplama
            // 
            lblToplama.AutoSize = true;
            lblToplama.BackColor = Color.FromArgb(50, 50, 50);
            lblToplama.Dock = DockStyle.Fill;
            lblToplama.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblToplama.ForeColor = Color.FromArgb(0, 255, 128);
            lblToplama.Location = new Point(3, 120);
            lblToplama.Name = "lblToplama";
            lblToplama.Size = new Size(184, 40);
            lblToplama.TabIndex = 0;
            lblToplama.Text = "000";
            lblToplama.TextAlign = ContentAlignment.MiddleCenter;
            lblToplama.Visible = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(50, 50, 50);
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(255, 255, 255);
            label3.Location = new Point(3, 80);
            label3.Name = "label3";
            label3.Size = new Size(184, 40);
            label3.TabIndex = 0;
            label3.Text = "TOPLAM";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.Visible = false;
            // 
            // lblVardiya
            // 
            lblVardiya.AutoSize = true;
            lblVardiya.BackColor = Color.FromArgb(50, 50, 50);
            lblVardiya.Dock = DockStyle.Fill;
            lblVardiya.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblVardiya.ForeColor = Color.FromArgb(0, 255, 128);
            lblVardiya.Location = new Point(3, 40);
            lblVardiya.Name = "lblVardiya";
            lblVardiya.Size = new Size(184, 40);
            lblVardiya.TabIndex = 0;
            lblVardiya.Text = "000";
            lblVardiya.TextAlign = ContentAlignment.MiddleCenter;
            lblVardiya.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(50, 50, 50);
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(255, 255, 255);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(184, 40);
            label1.TabIndex = 0;
            label1.Text = "VARDIYA SAYI";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Visible = false;
            // 
            // panel11
            // 
            panel11.BackColor = Color.FromArgb(60, 60, 60);
            panel11.Dock = DockStyle.Fill;
            panel11.Location = new Point(3, 363);
            panel11.Name = "panel11";
            panel11.Size = new Size(184, 41);
            panel11.TabIndex = 9;
            // 
            // Konveyor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1347, 630);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "Konveyor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UretimOtmasyon";
            WindowState = FormWindowState.Maximized;
            FormClosing += Konveyor_FormClosing;
            FormClosed += Konveyor_FormClosed;
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private TextBox txtBarcode;
        private Label lblToplam;
        private Label lblMessage;
        private TableLayoutPanel tableLayoutPanel5;
        private Panel panel11;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label lblOrtalama;
        private Label label5;
        private Label lblToplama;
        private Label label3;
        private Label lblVardiya;
        private Label label1;
        private ListBox listBox1;
    }
}

