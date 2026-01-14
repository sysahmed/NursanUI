namespace Nursan.Licenzing
{
    partial class Lisanslama
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lisanslama));
            tableLayoutPanel1 = new TableLayoutPanel();
            lblInstructions = new Label();
            txtSeriNomer = new TextBox();
            pictureBoxQr = new PictureBox();
            txtActivaciq = new TextBox();
            btnActiv = new Button();
            timerLicenseCheck = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQr).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblInstructions, 0, 0);
            tableLayoutPanel1.Controls.Add(txtSeriNomer, 0, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxQr, 0, 2);
            tableLayoutPanel1.Controls.Add(txtActivaciq, 0, 3);
            tableLayoutPanel1.Controls.Add(btnActiv, 0, 4);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tableLayoutPanel1.Size = new Size(466, 513);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // lblInstructions
            // 
            lblInstructions.AutoSize = true;
            lblInstructions.Dock = DockStyle.Fill;
            lblInstructions.Font = new Font("Segoe UI", 10F);
            lblInstructions.ForeColor = Color.DimGray;
            lblInstructions.Location = new Point(3, 0);
            lblInstructions.Margin = new Padding(3, 0, 3, 6);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(460, 38);
            lblInstructions.TabIndex = 0;
            lblInstructions.Text = "Сканирайте QR кода със служебното приложение или копирайте данните по-долу.";
            // 
            // txtSeriNomer
            // 
            txtSeriNomer.Dock = DockStyle.Fill;
            txtSeriNomer.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtSeriNomer.Location = new Point(3, 47);
            txtSeriNomer.Multiline = true;
            txtSeriNomer.Name = "txtSeriNomer";
            txtSeriNomer.ReadOnly = true;
            txtSeriNomer.ScrollBars = ScrollBars.Vertical;
            txtSeriNomer.Size = new Size(460, 148);
            txtSeriNomer.TabIndex = 1;
            // 
            // pictureBoxQr
            // 
            pictureBoxQr.BackColor = Color.White;
            pictureBoxQr.Dock = DockStyle.Fill;
            pictureBoxQr.Location = new Point(3, 201);
            pictureBoxQr.Name = "pictureBoxQr";
            pictureBoxQr.Size = new Size(460, 148);
            pictureBoxQr.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxQr.TabIndex = 2;
            pictureBoxQr.TabStop = false;
            // 
            // txtActivaciq
            // 
            txtActivaciq.Dock = DockStyle.Fill;
            txtActivaciq.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            txtActivaciq.Location = new Point(3, 355);
            txtActivaciq.Name = "txtActivaciq";
            txtActivaciq.PlaceholderText = "Въведете кода, получен от приложението";
            txtActivaciq.Size = new Size(460, 39);
            txtActivaciq.TabIndex = 3;
            // 
            // btnActiv
            // 
            btnActiv.Dock = DockStyle.Fill;
            btnActiv.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btnActiv.Location = new Point(3, 425);
            btnActiv.Name = "btnActiv";
            btnActiv.Size = new Size(460, 85);
            btnActiv.TabIndex = 4;
            btnActiv.Text = "Активирай";
            btnActiv.UseVisualStyleBackColor = true;
            btnActiv.Click += btnActiv_Click;
            // 
            // timerLicenseCheck
            // 
            timerLicenseCheck.Interval = 3000;
            timerLicenseCheck.Tick += TimerLicenseCheck_Tick;
            // 
            // Lisanslama
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(466, 513);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Lisanslama";
            Text = "Lisanslama";
            Load += Lisanslama_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQr).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label lblInstructions;
        private TextBox txtSeriNomer;
        private PictureBox pictureBoxQr;
        private TextBox txtActivaciq;
        private Button btnActiv;
        private System.Windows.Forms.Timer timerLicenseCheck;
    }
}