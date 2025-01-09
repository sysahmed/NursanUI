namespace Nursan.UI.Kasalama
{
    partial class Kasalama
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
            tableLayoutPanel1 = new TableLayoutPanel();
            listBox1 = new ListBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            textBox1 = new TextBox();
            lblMessages2 = new Label();
            lblMessages = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23.0195179F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76.980484F));
            tableLayoutPanel1.Controls.Add(listBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(lblMessages, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.0233917F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 83.97661F));
            tableLayoutPanel1.Size = new Size(1219, 513);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 32;
            listBox1.Location = new Point(2, 84);
            listBox1.Margin = new Padding(2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(276, 427);
            listBox1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(textBox1, 0, 1);
            tableLayoutPanel2.Controls.Add(lblMessages2, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(2, 2);
            tableLayoutPanel2.Margin = new Padding(2);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 57.1428566F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 42.8571434F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 21F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 16F));
            tableLayoutPanel2.Size = new Size(276, 78);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            textBox1.Location = new Point(2, 25);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(272, 29);
            textBox1.TabIndex = 0;
            textBox1.KeyUp += textBox1_KeyUp;
            // 
            // lblMessages2
            // 
            lblMessages2.AutoSize = true;
            lblMessages2.Dock = DockStyle.Fill;
            lblMessages2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblMessages2.Location = new Point(2, 0);
            lblMessages2.Margin = new Padding(2, 0, 2, 0);
            lblMessages2.Name = "lblMessages2";
            lblMessages2.Size = new Size(272, 23);
            lblMessages2.TabIndex = 2;
            lblMessages2.Text = "label1";
            lblMessages2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblMessages
            // 
            lblMessages.AutoSize = true;
            lblMessages.Dock = DockStyle.Fill;
            lblMessages.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblMessages.Location = new Point(282, 0);
            lblMessages.Margin = new Padding(2, 0, 2, 0);
            lblMessages.Name = "lblMessages";
            lblMessages.Size = new Size(935, 82);
            lblMessages.TabIndex = 2;
            lblMessages.Text = "label1";
            lblMessages.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Kasalama
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1219, 513);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(2);
            Name = "Kasalama";
            Text = "Kasalama";
            Load += Kasalama_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private ListBox listBox1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblMessages2;
        private Label lblMessages;
        public TextBox textBox1;
    }
}