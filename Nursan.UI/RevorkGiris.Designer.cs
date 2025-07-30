namespace Nursan.UI
{
    partial class RevorkGiris
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
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            txtBarcodeReader = new TextBox();
            Quality = new Label();
            listBox1 = new ListBox();
            btnReworkOut = new Button();
            cbModulerYapi = new ComboBox();
            closingForm = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(30, 30, 30);
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72.68878F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27.31122F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(txtBarcodeReader, 0, 1);
            tableLayoutPanel1.Controls.Add(Quality, 0, 0);
            tableLayoutPanel1.Controls.Add(listBox1, 0, 2);
            tableLayoutPanel1.Controls.Add(btnReworkOut, 1, 1);
            tableLayoutPanel1.Controls.Add(cbModulerYapi, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.45295048F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 71.61085F));
            tableLayoutPanel1.Size = new Size(1347, 627);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // txtBarcodeReader
            // 
            txtBarcodeReader.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtBarcodeReader.BackColor = Color.FromArgb(60, 60, 60);
            txtBarcodeReader.BorderStyle = BorderStyle.None;
            txtBarcodeReader.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            txtBarcodeReader.ForeColor = Color.White;
            txtBarcodeReader.Location = new Point(3, 128);
            txtBarcodeReader.Multiline = true;
            txtBarcodeReader.Name = "txtBarcodeReader";
            txtBarcodeReader.Size = new Size(973, 46);
            txtBarcodeReader.TabIndex = 0;
            txtBarcodeReader.TextAlign = HorizontalAlignment.Center;
            txtBarcodeReader.KeyUp += txtBarcodeReader_KeyUp;
            // 
            // Quality
            // 
            Quality.AutoSize = true;
            Quality.BackColor = Color.FromArgb(45, 45, 48);
            Quality.Dock = DockStyle.Fill;
            Quality.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold);
            Quality.ForeColor = Color.Lime;
            Quality.Location = new Point(3, 0);
            Quality.Name = "Quality";
            Quality.Size = new Size(973, 125);
            Quality.TabIndex = 1;
            Quality.Text = "Revork Giris Sistemi";
            Quality.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.FromArgb(60, 60, 60);
            listBox1.BorderStyle = BorderStyle.None;
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
            listBox1.ForeColor = Color.White;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 32;
            listBox1.Location = new Point(2, 179);
            listBox1.Margin = new Padding(2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(975, 446);
            listBox1.TabIndex = 2;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // btnReworkOut
            // 
            btnReworkOut.BackColor = Color.FromArgb(0, 122, 204);
            btnReworkOut.Dock = DockStyle.Fill;
            btnReworkOut.FlatAppearance.BorderSize = 0;
            btnReworkOut.FlatStyle = FlatStyle.Flat;
            btnReworkOut.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReworkOut.ForeColor = Color.White;
            btnReworkOut.Location = new Point(982, 128);
            btnReworkOut.Name = "btnReworkOut";
            btnReworkOut.Size = new Size(362, 46);
            btnReworkOut.TabIndex = 3;
            btnReworkOut.Text = "Systemden Cikar";
            btnReworkOut.UseVisualStyleBackColor = false;
            btnReworkOut.Visible = false;
            btnReworkOut.Click += btnReworkOut_Click;
            // 
            // cbModulerYapi
            // 
            cbModulerYapi.BackColor = Color.FromArgb(60, 60, 60);
            cbModulerYapi.FlatStyle = FlatStyle.Flat;
            cbModulerYapi.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cbModulerYapi.ForeColor = Color.White;
            cbModulerYapi.FormattingEnabled = true;
            cbModulerYapi.Location = new Point(982, 180);
            cbModulerYapi.Name = "cbModulerYapi";
            cbModulerYapi.Size = new Size(362, 40);
            cbModulerYapi.TabIndex = 4;
            cbModulerYapi.Visible = false;
            // 
            // RevorkGiris
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1347, 627);
            Controls.Add(tableLayoutPanel1);
            KeyPreview = true;
            Name = "RevorkGiris";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Revork Giris Sistemi";
            WindowState = FormWindowState.Maximized;
            Load += RevorkGiris_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer closingForm;
        private TextBox txtBarcodeReader;
        private Label Quality;
        private ListBox listBox1;
        private Button btnReworkOut;
        private ComboBox cbModulerYapi;
    }
}