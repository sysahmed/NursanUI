namespace Nursan.UI
{
    partial class SicilOkumaAP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SicilOkumaAP));
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            textBox1 = new TextBox();
            listBox1 = new ListBox();
            button1 = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(textBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(listBox1, 0, 2);
            tableLayoutPanel1.Controls.Add(button1, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 31.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 48.7725029F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20.1309338F));
            tableLayoutPanel1.Size = new Size(1347, 630);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(33, 37, 41);
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(40, 167, 69);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(1341, 184);
            label1.TabIndex = 0;
            label1.Text = "Сканирай сицил номер...";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(255, 248, 220);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Dock = DockStyle.Fill;
            textBox1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            textBox1.ForeColor = Color.FromArgb(220, 53, 69);
            textBox1.Location = new Point(3, 187);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Въведи сицил номер...";
            textBox1.Size = new Size(1341, 36);
            textBox1.TabIndex = 1;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.KeyUp += textBox1_KeyUp;
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.FromArgb(248, 249, 250);
            listBox1.BorderStyle = BorderStyle.FixedSingle;
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            listBox1.ForeColor = Color.FromArgb(33, 37, 41);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(3, 227);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(1341, 281);
            listBox1.TabIndex = 2;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(220, 53, 69);
            button1.Dock = DockStyle.Fill;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(3, 514);
            button1.Name = "button1";
            button1.Size = new Size(1341, 113);
            button1.TabIndex = 3;
            button1.Text = "ЗАТВОРИ";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // SicilOkumaAP
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(1347, 630);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SicilOkumaAP";
            Text = "Nursan - Сицил система (AP)";
            WindowState = FormWindowState.Maximized;
            Load += SicilOkuma_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TextBox textBox1;
        private ListBox listBox1;
        private Button button1;
    }
}