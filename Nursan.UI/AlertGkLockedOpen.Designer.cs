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
            labelWarning.BackColor = Color.Red;
            labelWarning.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelWarning.ForeColor = SystemColors.ButtonHighlight;
            labelWarning.Location = new Point(18, 14);
            labelWarning.Name = "labelWarning";
            labelWarning.Size = new Size(375, 24);
            labelWarning.TabIndex = 0;
            labelWarning.Text = "ВНИМАНИЕ: АЛЕРТЪТ Е ЗАКЛЮЧЕН!";
            // 
            // labelInfo
            // 
            labelInfo.BackColor = Color.Red;
            labelInfo.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelInfo.ForeColor = SystemColors.ButtonHighlight;
            labelInfo.Location = new Point(18, 47);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new Size(412, 79);
            labelInfo.TabIndex = 1;
            labelInfo.Text = "Сканирайте първо целия баркод (с ID), после баркод с Alert Number.";
            // 
            // textBoxBarcode1
            // 
            textBoxBarcode1.Font = new Font("Microsoft Sans Serif", 16F);
            textBoxBarcode1.Location = new Point(18, 129);
            textBoxBarcode1.Name = "textBoxBarcode1";
            textBoxBarcode1.PlaceholderText = "Сканирайте целия баркод (с ID)";
            textBoxBarcode1.Size = new Size(412, 32);
            textBoxBarcode1.TabIndex = 2;
            // 
            // textBoxBarcode2
            // 
            textBoxBarcode2.Font = new Font("Microsoft Sans Serif", 16F);
            textBoxBarcode2.Location = new Point(18, 176);
            textBoxBarcode2.Name = "textBoxBarcode2";
            textBoxBarcode2.PlaceholderText = "Сканирайте баркод с Alert Number";
            textBoxBarcode2.Size = new Size(412, 32);
            textBoxBarcode2.TabIndex = 3;
            // 
            // AlertGkLockedOpen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Red;
            ClientSize = new Size(455, 237);
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