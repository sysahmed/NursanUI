namespace Nursan.UI
{
    partial class Revork
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Revork));
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblIdDonanim = new Label();
            tableLayoutPanel8 = new TableLayoutPanel();
            btnGiris = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            lblHataCode = new Label();
            lblMessage = new Label();
            tableLayoutPanel4 = new TableLayoutPanel();
            txtBarcodeReader = new TextBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            lblFaultName = new Label();
            lblFaultHarnessLocation = new Label();
            lblFaultCabel = new Label();
            lblOperator = new Label();
            lblFaultSetLocation = new Label();
            lblFaultReson = new Label();
            lblExplanation = new Label();
            txtFaultName = new TextBox();
            txtFaultHarnessLocation = new TextBox();
            txtFaultCabloE = new TextBox();
            txtOperator = new TextBox();
            txtFaultSetLocation = new TextBox();
            txtExplanation = new TextBox();
            cbFaultReason = new ComboBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            listBox1 = new ListBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            girenRevork = new DataGridView();
            cikanRevork = new DataGridView();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)girenRevork).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cikanRevork).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.2194786F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 82.7805252F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 1, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel6, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel7, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.Size = new Size(1179, 619);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = SystemColors.ActiveCaption;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(lblIdDonanim, 0, 1);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel8, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(197, 117);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // lblIdDonanim
            // 
            lblIdDonanim.AutoSize = true;
            lblIdDonanim.Dock = DockStyle.Fill;
            lblIdDonanim.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblIdDonanim.Location = new Point(3, 58);
            lblIdDonanim.Name = "lblIdDonanim";
            lblIdDonanim.Size = new Size(191, 59);
            lblIdDonanim.TabIndex = 2;
            lblIdDonanim.Text = "Barcode ID";
            lblIdDonanim.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 2;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.Controls.Add(btnGiris, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(3, 3);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 2;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.Size = new Size(191, 52);
            tableLayoutPanel8.TabIndex = 0;
            // 
            // btnGiris
            // 
            btnGiris.Dock = DockStyle.Fill;
            btnGiris.FlatStyle = FlatStyle.Flat;
            btnGiris.Location = new Point(0, 0);
            btnGiris.Margin = new Padding(0);
            btnGiris.Name = "btnGiris";
            btnGiris.Size = new Size(95, 26);
            btnGiris.TabIndex = 0;
            btnGiris.Text = "Giris";
            btnGiris.UseVisualStyleBackColor = true;
            btnGiris.Click += btnGiris_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = SystemColors.ActiveBorder;
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Controls.Add(lblHataCode, 0, 1);
            tableLayoutPanel3.Controls.Add(lblMessage, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(206, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(970, 117);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // lblHataCode
            // 
            lblHataCode.AutoSize = true;
            lblHataCode.Dock = DockStyle.Fill;
            lblHataCode.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblHataCode.Location = new Point(3, 58);
            lblHataCode.Name = "lblHataCode";
            lblHataCode.Size = new Size(964, 59);
            lblHataCode.TabIndex = 3;
            lblHataCode.Text = "Hata Code";
            lblHataCode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblMessage.Location = new Point(3, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(964, 58);
            lblMessage.TabIndex = 1;
            lblMessage.Text = ":";
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.IndianRed;
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Controls.Add(txtBarcodeReader, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 126);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 4;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.Size = new Size(197, 117);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // txtBarcodeReader
            // 
            txtBarcodeReader.Dock = DockStyle.Fill;
            txtBarcodeReader.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtBarcodeReader.Location = new Point(3, 32);
            txtBarcodeReader.Name = "txtBarcodeReader";
            txtBarcodeReader.Size = new Size(191, 27);
            txtBarcodeReader.TabIndex = 0;
            txtBarcodeReader.TextAlign = HorizontalAlignment.Center;
            txtBarcodeReader.KeyUp += txtBarcodeReader_KeyUp;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.LightCoral;
            tableLayoutPanel5.ColumnCount = 10;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30.0946369F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 2.902208F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 2.39747643F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 4.542587F));
            tableLayoutPanel5.Controls.Add(lblFaultName, 0, 0);
            tableLayoutPanel5.Controls.Add(lblFaultHarnessLocation, 1, 0);
            tableLayoutPanel5.Controls.Add(lblFaultCabel, 2, 0);
            tableLayoutPanel5.Controls.Add(lblOperator, 3, 0);
            tableLayoutPanel5.Controls.Add(lblFaultSetLocation, 4, 0);
            tableLayoutPanel5.Controls.Add(lblFaultReson, 5, 0);
            tableLayoutPanel5.Controls.Add(lblExplanation, 6, 0);
            tableLayoutPanel5.Controls.Add(txtFaultName, 0, 1);
            tableLayoutPanel5.Controls.Add(txtFaultHarnessLocation, 1, 1);
            tableLayoutPanel5.Controls.Add(txtFaultCabloE, 2, 1);
            tableLayoutPanel5.Controls.Add(txtOperator, 3, 1);
            tableLayoutPanel5.Controls.Add(txtFaultSetLocation, 4, 1);
            tableLayoutPanel5.Controls.Add(txtExplanation, 6, 1);
            tableLayoutPanel5.Controls.Add(cbFaultReason, 5, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(206, 126);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 4;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.Size = new Size(970, 117);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // lblFaultName
            // 
            lblFaultName.AutoSize = true;
            lblFaultName.Dock = DockStyle.Fill;
            lblFaultName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFaultName.Location = new Point(3, 0);
            lblFaultName.Name = "lblFaultName";
            lblFaultName.Size = new Size(91, 29);
            lblFaultName.TabIndex = 0;
            lblFaultName.Text = "Hata Kodu";
            lblFaultName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFaultHarnessLocation
            // 
            lblFaultHarnessLocation.AutoSize = true;
            lblFaultHarnessLocation.Dock = DockStyle.Fill;
            lblFaultHarnessLocation.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFaultHarnessLocation.Location = new Point(100, 0);
            lblFaultHarnessLocation.Name = "lblFaultHarnessLocation";
            lblFaultHarnessLocation.Size = new Size(91, 29);
            lblFaultHarnessLocation.TabIndex = 0;
            lblFaultHarnessLocation.Text = "Bolge";
            lblFaultHarnessLocation.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFaultCabel
            // 
            lblFaultCabel.AutoSize = true;
            lblFaultCabel.Dock = DockStyle.Fill;
            lblFaultCabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFaultCabel.Location = new Point(197, 0);
            lblFaultCabel.Name = "lblFaultCabel";
            lblFaultCabel.Size = new Size(91, 29);
            lblFaultCabel.TabIndex = 0;
            lblFaultCabel.Text = "Kablo/Goz";
            lblFaultCabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblOperator
            // 
            lblOperator.AutoSize = true;
            lblOperator.Dock = DockStyle.Fill;
            lblOperator.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblOperator.Location = new Point(294, 0);
            lblOperator.Name = "lblOperator";
            lblOperator.Size = new Size(91, 29);
            lblOperator.TabIndex = 0;
            lblOperator.Text = "Operator";
            lblOperator.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFaultSetLocation
            // 
            lblFaultSetLocation.AutoSize = true;
            lblFaultSetLocation.Dock = DockStyle.Fill;
            lblFaultSetLocation.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFaultSetLocation.Location = new Point(391, 0);
            lblFaultSetLocation.Name = "lblFaultSetLocation";
            lblFaultSetLocation.Size = new Size(91, 29);
            lblFaultSetLocation.TabIndex = 0;
            lblFaultSetLocation.Text = "TespitYeri";
            lblFaultSetLocation.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFaultReson
            // 
            lblFaultReson.AutoSize = true;
            lblFaultReson.Dock = DockStyle.Fill;
            lblFaultReson.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFaultReson.Location = new Point(488, 0);
            lblFaultReson.Name = "lblFaultReson";
            lblFaultReson.Size = new Size(91, 29);
            lblFaultReson.TabIndex = 0;
            lblFaultReson.Text = "Hata Sebebi";
            lblFaultReson.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblExplanation
            // 
            lblExplanation.AutoSize = true;
            lblExplanation.Dock = DockStyle.Fill;
            lblExplanation.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblExplanation.Location = new Point(585, 0);
            lblExplanation.Name = "lblExplanation";
            lblExplanation.Size = new Size(286, 29);
            lblExplanation.TabIndex = 0;
            lblExplanation.Text = "Aciklama";
            lblExplanation.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtFaultName
            // 
            txtFaultName.Dock = DockStyle.Fill;
            txtFaultName.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtFaultName.Location = new Point(3, 32);
            txtFaultName.Name = "txtFaultName";
            txtFaultName.Size = new Size(91, 27);
            txtFaultName.TabIndex = 1;
            txtFaultName.TextAlign = HorizontalAlignment.Center;
            txtFaultName.KeyUp += txtFaultName_KeyUp;
            // 
            // txtFaultHarnessLocation
            // 
            txtFaultHarnessLocation.Dock = DockStyle.Fill;
            txtFaultHarnessLocation.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtFaultHarnessLocation.Location = new Point(100, 32);
            txtFaultHarnessLocation.Name = "txtFaultHarnessLocation";
            txtFaultHarnessLocation.Size = new Size(91, 27);
            txtFaultHarnessLocation.TabIndex = 1;
            txtFaultHarnessLocation.TextAlign = HorizontalAlignment.Center;
            txtFaultHarnessLocation.KeyUp += txtFaultHarnessLocation_KeyUp;
            // 
            // txtFaultCabloE
            // 
            txtFaultCabloE.Dock = DockStyle.Fill;
            txtFaultCabloE.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtFaultCabloE.Location = new Point(197, 32);
            txtFaultCabloE.Name = "txtFaultCabloE";
            txtFaultCabloE.Size = new Size(91, 27);
            txtFaultCabloE.TabIndex = 1;
            txtFaultCabloE.TextAlign = HorizontalAlignment.Center;
            txtFaultCabloE.KeyUp += txtFaultCabloE_KeyUp;
            // 
            // txtOperator
            // 
            txtOperator.Dock = DockStyle.Fill;
            txtOperator.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtOperator.Location = new Point(294, 32);
            txtOperator.Name = "txtOperator";
            txtOperator.Size = new Size(91, 27);
            txtOperator.TabIndex = 1;
            txtOperator.TextAlign = HorizontalAlignment.Center;
            txtOperator.KeyUp += txtOperator_KeyUp;
            // 
            // txtFaultSetLocation
            // 
            txtFaultSetLocation.Dock = DockStyle.Fill;
            txtFaultSetLocation.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtFaultSetLocation.Location = new Point(391, 32);
            txtFaultSetLocation.Name = "txtFaultSetLocation";
            txtFaultSetLocation.Size = new Size(91, 27);
            txtFaultSetLocation.TabIndex = 1;
            txtFaultSetLocation.TextAlign = HorizontalAlignment.Center;
            txtFaultSetLocation.KeyUp += txtFaultSetLocation_KeyUp;
            // 
            // txtExplanation
            // 
            txtExplanation.Dock = DockStyle.Fill;
            txtExplanation.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            txtExplanation.Location = new Point(585, 32);
            txtExplanation.Name = "txtExplanation";
            txtExplanation.Size = new Size(286, 27);
            txtExplanation.TabIndex = 1;
            txtExplanation.TextAlign = HorizontalAlignment.Center;
            txtExplanation.KeyUp += txtExplanation_KeyUp;
            // 
            // cbFaultReason
            // 
            cbFaultReason.Dock = DockStyle.Fill;
            cbFaultReason.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold);
            cbFaultReason.FormattingEnabled = true;
            cbFaultReason.Items.AddRange(new object[] { "MONTAJ", "BAGA", "SRB", "KLIPTEST", "ELTEST", "TASIMA-ASKI", "DIGER" });
            cbFaultReason.Location = new Point(488, 32);
            cbFaultReason.Name = "cbFaultReason";
            cbFaultReason.Size = new Size(91, 28);
            cbFaultReason.TabIndex = 2;
            cbFaultReason.SelectedIndexChanged += cbFaultReason_SelectedIndexChanged;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.BackColor = Color.FromArgb(192, 192, 255);
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 14F));
            tableLayoutPanel6.Controls.Add(listBox1, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 249);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Size = new Size(197, 367);
            tableLayoutPanel6.TabIndex = 4;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 21;
            listBox1.Location = new Point(1, 2);
            listBox1.Margin = new Padding(1, 2, 1, 2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(195, 179);
            listBox1.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.BackColor = Color.DimGray;
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 14F));
            tableLayoutPanel7.Controls.Add(girenRevork, 0, 0);
            tableLayoutPanel7.Controls.Add(cikanRevork, 0, 1);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(206, 249);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.Size = new Size(970, 367);
            tableLayoutPanel7.TabIndex = 5;
            // 
            // girenRevork
            // 
            girenRevork.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            girenRevork.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            girenRevork.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            girenRevork.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            girenRevork.DefaultCellStyle = dataGridViewCellStyle2;
            girenRevork.Dock = DockStyle.Fill;
            girenRevork.Location = new Point(3, 3);
            girenRevork.Name = "girenRevork";
            girenRevork.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            girenRevork.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            girenRevork.Size = new Size(964, 177);
            girenRevork.TabIndex = 0;
            girenRevork.CellClick += girenRevork_CellContentClick;
            girenRevork.CellContentClick += girenRevork_CellContentClick;
            // 
            // cikanRevork
            // 
            cikanRevork.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cikanRevork.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            cikanRevork.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            cikanRevork.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            cikanRevork.DefaultCellStyle = dataGridViewCellStyle4;
            cikanRevork.Dock = DockStyle.Fill;
            cikanRevork.GridColor = Color.YellowGreen;
            cikanRevork.Location = new Point(3, 186);
            cikanRevork.Name = "cikanRevork";
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            cikanRevork.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            cikanRevork.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            cikanRevork.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cikanRevork.Size = new Size(964, 178);
            cikanRevork.TabIndex = 2;
            // 
            // Revork
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(1179, 619);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Revork";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Revork";
            WindowState = FormWindowState.Maximized;
            Load += Revork_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)girenRevork).EndInit();
            ((System.ComponentModel.ISupportInitialize)cikanRevork).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private Label lblFaultName;
        private Label lblFaultHarnessLocation;
        private Label lblFaultCabel;
        private Label lblOperator;
        private Label lblFaultSetLocation;
        private Label lblFaultReson;
        private Label lblExplanation;
        private TextBox txtFaultName;
        private TextBox txtFaultHarnessLocation;
        private TextBox txtFaultCabloE;
        private TextBox txtOperator;
        private TextBox txtFaultSetLocation;
        private TextBox txtExplanation;
        private ComboBox cbFaultReason;
        private TableLayoutPanel tableLayoutPanel6;
        private TableLayoutPanel tableLayoutPanel7;
        private TextBox txtBarcodeReader;
        private TableLayoutPanel tableLayoutPanel8;
        private Button btnGiris;
        private Label lblMessage;
        private DataGridView girenRevork;
        private DataGridView cikanRevork;
        private ListBox listBox1;
        private Label lblIdDonanim;
        private Label lblHataCode;
    }
}