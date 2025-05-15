namespace Nursan.UI
{
    partial class AlertGkLockedOpen
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.TextBox textBoxBarcode1;
        private System.Windows.Forms.TextBox textBoxBarcode2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            labelWarning = new Label();
            labelInfo = new Label();
            textBoxBarcode1 = new TextBox();
            textBoxBarcode2 = new TextBox();
            SuspendLayout();
            // 
            // labelWarning
            // 
            labelWarning.AutoSize = true;
            labelWarning.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelWarning.ForeColor = Color.DarkRed;
            labelWarning.Location = new Point(18, 14);
            labelWarning.Name = "labelWarning";
            labelWarning.Size = new Size(375, 24);
            labelWarning.TabIndex = 0;
            labelWarning.Text = "ВНИМАНИЕ: АЛЕРТЪТ Е ЗАКЛЮЧЕН!";
            // 
            // labelInfo
            // 
            labelInfo.AutoSize = true;
            labelInfo.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelInfo.Location = new Point(18, 47);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new Size(594, 20);
            labelInfo.TabIndex = 1;
            labelInfo.Text = "Сканирайте първо целия баркод (с ID), после баркод с Alert Number.";
            // 
            // textBoxBarcode1
            // 
            textBoxBarcode1.Font = new Font("Microsoft Sans Serif", 16F);
            textBoxBarcode1.Location = new Point(21, 84);
            textBoxBarcode1.Name = "textBoxBarcode1";
            textBoxBarcode1.PlaceholderText = "Сканирайте целия баркод (с ID)";
            textBoxBarcode1.Size = new Size(412, 32);
            textBoxBarcode1.TabIndex = 2;
            // 
            // textBoxBarcode2
            // 
            textBoxBarcode2.Font = new Font("Microsoft Sans Serif", 16F);
            textBoxBarcode2.Location = new Point(21, 131);
            textBoxBarcode2.Name = "textBoxBarcode2";
            textBoxBarcode2.PlaceholderText = "Сканирайте баркод с Alert Number";
            textBoxBarcode2.Size = new Size(412, 32);
            textBoxBarcode2.TabIndex = 3;
            // 
            // AlertGkLockedOpen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(455, 188);
            Controls.Add(textBoxBarcode2);
            Controls.Add(textBoxBarcode1);
            Controls.Add(labelInfo);
            Controls.Add(labelWarning);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AlertGkLockedOpen";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Отключване на аларма (GK)";
            Load += AlertGkLockedOpen_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
} 