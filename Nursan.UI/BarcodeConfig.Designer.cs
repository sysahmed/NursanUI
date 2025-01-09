namespace Nursan.UI
{
    partial class BarcodeConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BarcodeConfig));
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            label8 = new Label();
            label12 = new Label();
            ndStartingSutring = new NumericUpDown();
            ndStopimgSubstring = new NumericUpDown();
            cbPrinter1 = new ComboBox();
            button6 = new Button();
            cbBrcodeSec = new ComboBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            label3 = new Label();
            label2 = new Label();
            label7 = new Label();
            ndPadlef = new NumericUpDown();
            label6 = new Label();
            txtregexInt = new TextBox();
            txtRegexString = new TextBox();
            label1 = new Label();
            label11 = new Label();
            txtBarcodeismi = new TextBox();
            label10 = new Label();
            label9 = new Label();
            txtParcalamaCar = new TextBox();
            ndUnunluk = new NumericUpDown();
            tableLayoutPanel4 = new TableLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            tableLayoutPanel5 = new TableLayoutPanel();
            listBarcodeCikis = new ListBox();
            listBarcode = new ListBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ndStartingSutring).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ndStopimgSubstring).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ndPadlef).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ndUnunluk).BeginInit();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 1, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1046, 604);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(label8, 0, 0);
            tableLayoutPanel3.Controls.Add(label12, 0, 1);
            tableLayoutPanel3.Controls.Add(ndStartingSutring, 1, 0);
            tableLayoutPanel3.Controls.Add(ndStopimgSubstring, 1, 1);
            tableLayoutPanel3.Controls.Add(cbPrinter1, 1, 2);
            tableLayoutPanel3.Controls.Add(button6, 0, 2);
            tableLayoutPanel3.Controls.Add(cbBrcodeSec, 0, 5);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(526, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 6;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel3.Size = new Size(517, 296);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Fill;
            label8.Location = new Point(3, 0);
            label8.Name = "label8";
            label8.Size = new Size(252, 49);
            label8.TabIndex = 3;
            label8.Text = "Starting Substring";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Dock = DockStyle.Fill;
            label12.Location = new Point(3, 49);
            label12.Name = "label12";
            label12.Size = new Size(252, 49);
            label12.TabIndex = 8;
            label12.Text = "Stoping Substring";
            label12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ndStartingSutring
            // 
            ndStartingSutring.Dock = DockStyle.Fill;
            ndStartingSutring.Location = new Point(261, 3);
            ndStartingSutring.Name = "ndStartingSutring";
            ndStartingSutring.Size = new Size(253, 23);
            ndStartingSutring.TabIndex = 11;
            // 
            // ndStopimgSubstring
            // 
            ndStopimgSubstring.Dock = DockStyle.Fill;
            ndStopimgSubstring.Location = new Point(261, 52);
            ndStopimgSubstring.Name = "ndStopimgSubstring";
            ndStopimgSubstring.Size = new Size(253, 23);
            ndStopimgSubstring.TabIndex = 12;
            // 
            // cbPrinter1
            // 
            cbPrinter1.BackColor = Color.Salmon;
            cbPrinter1.Dock = DockStyle.Fill;
            cbPrinter1.FormattingEnabled = true;
            cbPrinter1.Location = new Point(261, 101);
            cbPrinter1.Name = "cbPrinter1";
            cbPrinter1.Size = new Size(253, 23);
            cbPrinter1.TabIndex = 1;
            // 
            // button6
            // 
            button6.BackColor = SystemColors.ActiveBorder;
            button6.Dock = DockStyle.Fill;
            button6.Location = new Point(3, 101);
            button6.Name = "button6";
            button6.Size = new Size(252, 43);
            button6.TabIndex = 2;
            button6.Text = " Yazici  Sec";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // cbBrcodeSec
            // 
            cbBrcodeSec.Dock = DockStyle.Fill;
            cbBrcodeSec.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            cbBrcodeSec.FormattingEnabled = true;
            cbBrcodeSec.Items.AddRange(new object[] { "Okutma Barcode Sakla", "Basma Barcode Sakla" });
            cbBrcodeSec.Location = new Point(3, 248);
            cbBrcodeSec.Name = "cbBrcodeSec";
            cbBrcodeSec.Size = new Size(252, 33);
            cbBrcodeSec.TabIndex = 13;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(label3, 0, 6);
            tableLayoutPanel2.Controls.Add(label2, 0, 6);
            tableLayoutPanel2.Controls.Add(label7, 0, 4);
            tableLayoutPanel2.Controls.Add(ndPadlef, 1, 5);
            tableLayoutPanel2.Controls.Add(label6, 0, 3);
            tableLayoutPanel2.Controls.Add(txtregexInt, 1, 4);
            tableLayoutPanel2.Controls.Add(txtRegexString, 1, 3);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(label11, 0, 5);
            tableLayoutPanel2.Controls.Add(txtBarcodeismi, 1, 0);
            tableLayoutPanel2.Controls.Add(label10, 0, 1);
            tableLayoutPanel2.Controls.Add(label9, 0, 2);
            tableLayoutPanel2.Controls.Add(txtParcalamaCar, 1, 2);
            tableLayoutPanel2.Controls.Add(ndUnunluk, 1, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 7;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 15.15152F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 15.15152F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 15.15152F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 15.15152F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 15.15152F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 15.15152F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 9.090907F));
            tableLayoutPanel2.Size = new Size(517, 296);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label3.Location = new Point(3, 264);
            label3.Name = "label3";
            label3.Size = new Size(252, 32);
            label3.TabIndex = 13;
            label3.Text = "Barcode Okuma";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.Location = new Point(261, 264);
            label2.Name = "label2";
            label2.Size = new Size(253, 32);
            label2.TabIndex = 12;
            label2.Text = "Barcode Basma";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(3, 176);
            label7.Name = "label7";
            label7.Size = new Size(252, 44);
            label7.TabIndex = 2;
            label7.Text = "RegexInt";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ndPadlef
            // 
            ndPadlef.Dock = DockStyle.Fill;
            ndPadlef.Location = new Point(261, 223);
            ndPadlef.Name = "ndPadlef";
            ndPadlef.Size = new Size(253, 23);
            ndPadlef.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(3, 132);
            label6.Name = "label6";
            label6.Size = new Size(252, 44);
            label6.TabIndex = 1;
            label6.Text = "RegexString";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtregexInt
            // 
            txtregexInt.Dock = DockStyle.Fill;
            txtregexInt.Location = new Point(261, 179);
            txtregexInt.Name = "txtregexInt";
            txtregexInt.Size = new Size(253, 23);
            txtregexInt.TabIndex = 6;
            // 
            // txtRegexString
            // 
            txtRegexString.Dock = DockStyle.Fill;
            txtRegexString.Location = new Point(261, 135);
            txtRegexString.Name = "txtRegexString";
            txtRegexString.Size = new Size(253, 23);
            txtRegexString.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(252, 44);
            label1.TabIndex = 0;
            label1.Text = "Barcod Ismi";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = DockStyle.Fill;
            label11.Location = new Point(3, 220);
            label11.Name = "label11";
            label11.Size = new Size(252, 44);
            label11.TabIndex = 8;
            label11.Text = "PadLeft";
            label11.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtBarcodeismi
            // 
            txtBarcodeismi.Dock = DockStyle.Fill;
            txtBarcodeismi.Location = new Point(261, 3);
            txtBarcodeismi.Name = "txtBarcodeismi";
            txtBarcodeismi.Size = new Size(253, 23);
            txtBarcodeismi.TabIndex = 4;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Dock = DockStyle.Fill;
            label10.Location = new Point(3, 44);
            label10.Name = "label10";
            label10.Size = new Size(252, 44);
            label10.TabIndex = 10;
            label10.Text = "Uzunluk";
            label10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(3, 88);
            label9.Name = "label9";
            label9.Size = new Size(252, 44);
            label9.TabIndex = 10;
            label9.Text = "Parcalama Char";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtParcalamaCar
            // 
            txtParcalamaCar.Dock = DockStyle.Fill;
            txtParcalamaCar.Location = new Point(261, 91);
            txtParcalamaCar.MaxLength = 1;
            txtParcalamaCar.Name = "txtParcalamaCar";
            txtParcalamaCar.Size = new Size(253, 23);
            txtParcalamaCar.TabIndex = 9;
            // 
            // ndUnunluk
            // 
            ndUnunluk.Dock = DockStyle.Fill;
            ndUnunluk.Location = new Point(261, 47);
            ndUnunluk.Name = "ndUnunluk";
            ndUnunluk.Size = new Size(253, 23);
            ndUnunluk.TabIndex = 11;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = SystemColors.ActiveCaption;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(button1, 0, 0);
            tableLayoutPanel4.Controls.Add(button2, 1, 0);
            tableLayoutPanel4.Controls.Add(button3, 0, 1);
            tableLayoutPanel4.Controls.Add(button4, 1, 1);
            tableLayoutPanel4.Controls.Add(button5, 0, 2);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            tableLayoutPanel4.Location = new Point(526, 305);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 5;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.Size = new Size(517, 296);
            tableLayoutPanel4.TabIndex = 3;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(252, 53);
            button1.TabIndex = 0;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.Location = new Point(261, 3);
            button2.Name = "button2";
            button2.Size = new Size(253, 53);
            button2.TabIndex = 0;
            button2.Text = "Guncelle";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Location = new Point(3, 62);
            button3.Name = "button3";
            button3.Size = new Size(252, 53);
            button3.TabIndex = 0;
            button3.Text = "Delete";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Dock = DockStyle.Fill;
            button4.Location = new Point(261, 62);
            button4.Name = "button4";
            button4.Size = new Size(253, 53);
            button4.TabIndex = 0;
            button4.Text = "Text Delete";
            button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Dock = DockStyle.Fill;
            button5.Location = new Point(3, 121);
            button5.Name = "button5";
            button5.Size = new Size(252, 53);
            button5.TabIndex = 0;
            button5.Text = "Refresh";
            button5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(listBarcodeCikis, 1, 0);
            tableLayoutPanel5.Controls.Add(listBarcode, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 305);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Size = new Size(517, 296);
            tableLayoutPanel5.TabIndex = 4;
            // 
            // listBarcodeCikis
            // 
            listBarcodeCikis.Dock = DockStyle.Fill;
            listBarcodeCikis.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            listBarcodeCikis.FormattingEnabled = true;
            listBarcodeCikis.ItemHeight = 21;
            listBarcodeCikis.Location = new Point(261, 3);
            listBarcodeCikis.Name = "listBarcodeCikis";
            listBarcodeCikis.Size = new Size(253, 290);
            listBarcodeCikis.TabIndex = 3;
            // 
            // listBarcode
            // 
            listBarcode.Dock = DockStyle.Fill;
            listBarcode.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            listBarcode.FormattingEnabled = true;
            listBarcode.ItemHeight = 21;
            listBarcode.Location = new Point(3, 3);
            listBarcode.Name = "listBarcode";
            listBarcode.Size = new Size(252, 290);
            listBarcode.TabIndex = 2;
            // 
            // BarcodeConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1046, 604);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "BarcodeConfig";
            Text = "BarcodeConfig";
            Load += BarcodeConfig_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ndStartingSutring).EndInit();
            ((System.ComponentModel.ISupportInitialize)ndStopimgSubstring).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ndPadlef).EndInit();
            ((System.ComponentModel.ISupportInitialize)ndUnunluk).EndInit();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox txtRegexString;
        private TextBox txtregexInt;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label10;
        private TextBox txtParcalamaCar;
        private Label label1;
        private TextBox txtBarcodeismi;
        private NumericUpDown ndStartingSutring;
        private Label label11;
        private Label label12;
        private NumericUpDown ndPadlef;
        private Label label9;
        private NumericUpDown ndStopimgSubstring;
        private ListBox listBarcode;
        private TableLayoutPanel tableLayoutPanel4;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private NumericUpDown ndUnunluk;
        private ComboBox cbPrinter1;
        private Button button6;
        private Label label3;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel5;
        private ListBox listBarcodeCikis;
        private ComboBox cbBrcodeSec;
    }
}