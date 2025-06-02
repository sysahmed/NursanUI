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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lisanslama));
            tableLayoutPanel1 = new TableLayoutPanel();
            txtSeriNomer = new TextBox();
            txtActivaciq = new TextBox();
            btnActiv = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(txtSeriNomer, 0, 0);
            tableLayoutPanel1.Controls.Add(txtActivaciq, 0, 1);
            tableLayoutPanel1.Controls.Add(btnActiv, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 66.03475F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 7.74091625F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 26.3823071F));
            tableLayoutPanel1.Size = new Size(466, 513);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // txtSeriNomer
            // 
            txtSeriNomer.Dock = DockStyle.Fill;
            txtSeriNomer.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtSeriNomer.Location = new Point(3, 3);
            txtSeriNomer.Multiline = true;
            txtSeriNomer.Name = "txtSeriNomer";
            txtSeriNomer.Size = new Size(460, 332);
            txtSeriNomer.TabIndex = 0;
            // 
            // txtActivaciq
            // 
            txtActivaciq.Dock = DockStyle.Fill;
            txtActivaciq.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            txtActivaciq.Location = new Point(3, 341);
            txtActivaciq.Name = "txtActivaciq";
            txtActivaciq.Size = new Size(460, 39);
            txtActivaciq.TabIndex = 1;
            // 
            // btnActiv
            // 
            btnActiv.Dock = DockStyle.Fill;
            btnActiv.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btnActiv.Location = new Point(3, 380);
            btnActiv.Name = "btnActiv";
            btnActiv.Size = new Size(460, 130);
            btnActiv.TabIndex = 2;
            btnActiv.Text = "Active";
            btnActiv.UseVisualStyleBackColor = true;
            btnActiv.Click += btnActiv_Click;
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
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TextBox txtSeriNomer;
        private TextBox txtActivaciq;
        private Button btnActiv;
    }
}