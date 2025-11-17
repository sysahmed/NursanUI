using System.ComponentModel;
using System.Windows.Forms;

namespace Nursan.UI
{
    partial class BoltCameraConfigForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tableLayoutPanel = new TableLayoutPanel();
            lblModel = new Label();
            txtHarnessModelName = new TextBox();
            lblRtsp = new Label();
            txtRtspUrl = new TextBox();
            lblSteps = new Label();
            numSteps = new NumericUpDown();
            grid = new DataGridView();
            panelButtons = new Panel();
            btnFillDefaults = new Button();
            btnGenerateSql = new Button();
            btnClose = new Button();
            tableLayoutPanel.SuspendLayout();
            ((ISupportInitialize)numSteps).BeginInit();
            ((ISupportInitialize)grid).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Controls.Add(lblModel, 0, 0);
            tableLayoutPanel.Controls.Add(txtHarnessModelName, 1, 0);
            tableLayoutPanel.Controls.Add(lblRtsp, 2, 0);
            tableLayoutPanel.Controls.Add(txtRtspUrl, 3, 0);
            tableLayoutPanel.Controls.Add(lblSteps, 0, 1);
            tableLayoutPanel.Controls.Add(numSteps, 1, 1);
            tableLayoutPanel.Controls.Add(grid, 0, 2);
            tableLayoutPanel.Controls.Add(panelButtons, 0, 3);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 4;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.Size = new Size(1000, 620);
            tableLayoutPanel.TabIndex = 0;
            // 
            // lblModel
            // 
            lblModel.AutoSize = true;
            lblModel.Dock = DockStyle.Fill;
            lblModel.Location = new Point(3, 0);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(134, 40);
            lblModel.TabIndex = 0;
            lblModel.Text = "HarnessModelName:";
            lblModel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtHarnessModelName
            // 
            txtHarnessModelName.Dock = DockStyle.Fill;
            txtHarnessModelName.Location = new Point(143, 8);
            txtHarnessModelName.Margin = new Padding(3, 8, 3, 3);
            txtHarnessModelName.Name = "txtHarnessModelName";
            txtHarnessModelName.Size = new Size(354, 23);
            txtHarnessModelName.TabIndex = 1;
            // 
            // lblRtsp
            // 
            lblRtsp.AutoSize = true;
            lblRtsp.Dock = DockStyle.Fill;
            lblRtsp.Location = new Point(503, 0);
            lblRtsp.Name = "lblRtsp";
            lblRtsp.Size = new Size(134, 40);
            lblRtsp.TabIndex = 2;
            lblRtsp.Text = "RTSP URL:";
            lblRtsp.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtRtspUrl
            // 
            txtRtspUrl.Dock = DockStyle.Fill;
            txtRtspUrl.Location = new Point(643, 8);
            txtRtspUrl.Margin = new Padding(3, 8, 3, 3);
            txtRtspUrl.Name = "txtRtspUrl";
            txtRtspUrl.PlaceholderText = "rtsp://admin:admin@10.168.0.211:554/Streaming/Channels/101";
            txtRtspUrl.Size = new Size(354, 23);
            txtRtspUrl.TabIndex = 3;
            // 
            // lblSteps
            // 
            lblSteps.AutoSize = true;
            lblSteps.Dock = DockStyle.Fill;
            lblSteps.Location = new Point(3, 40);
            lblSteps.Name = "lblSteps";
            lblSteps.Size = new Size(134, 40);
            lblSteps.TabIndex = 4;
            lblSteps.Text = "Брой болтове:";
            lblSteps.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numSteps
            // 
            numSteps.Dock = DockStyle.Left;
            numSteps.Location = new Point(143, 48);
            numSteps.Margin = new Padding(3, 8, 3, 3);
            numSteps.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
            numSteps.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSteps.Name = "numSteps";
            numSteps.Size = new Size(80, 23);
            numSteps.TabIndex = 5;
            numSteps.Value = new decimal(new int[] { 3, 0, 0, 0 });
            numSteps.ValueChanged += numSteps_ValueChanged;
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanel.SetColumnSpan(grid, 4);
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(3, 83);
            grid.Name = "grid";
            grid.RowHeadersVisible = false;
            grid.Size = new Size(994, 484);
            grid.TabIndex = 6;
            // 
            // panelButtons
            // 
            tableLayoutPanel.SetColumnSpan(panelButtons, 4);
            panelButtons.Controls.Add(btnFillDefaults);
            panelButtons.Controls.Add(btnGenerateSql);
            panelButtons.Controls.Add(btnClose);
            panelButtons.Dock = DockStyle.Fill;
            panelButtons.Location = new Point(3, 573);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(994, 44);
            panelButtons.TabIndex = 7;
            // 
            // btnFillDefaults
            // 
            btnFillDefaults.Anchor = AnchorStyles.Left;
            btnFillDefaults.Location = new Point(12, 9);
            btnFillDefaults.Name = "btnFillDefaults";
            btnFillDefaults.Size = new Size(160, 27);
            btnFillDefaults.TabIndex = 0;
            btnFillDefaults.Text = "Попълни 1..N (ROI=0)";
            btnFillDefaults.UseVisualStyleBackColor = true;
            btnFillDefaults.Click += btnFillDefaults_Click;
            // 
            // btnGenerateSql
            // 
            btnGenerateSql.Anchor = AnchorStyles.Right;
            btnGenerateSql.Location = new Point(664, 9);
            btnGenerateSql.Name = "btnGenerateSql";
            btnGenerateSql.Size = new Size(190, 27);
            btnGenerateSql.TabIndex = 1;
            btnGenerateSql.Text = "Копирай SQL INSERT-и";
            btnGenerateSql.UseVisualStyleBackColor = true;
            btnGenerateSql.Click += btnGenerateSql_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Right;
            btnClose.Location = new Point(870, 9);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(110, 27);
            btnClose.TabIndex = 2;
            btnClose.Text = "Затвори";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // BoltCameraConfigForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 620);
            Controls.Add(tableLayoutPanel);
            Name = "BoltCameraConfigForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Настройка на камера/болтове";
            Load += BoltCameraConfigForm_Load;
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((ISupportInitialize)numSteps).EndInit();
            ((ISupportInitialize)grid).EndInit();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel;
        private Label lblModel;
        private TextBox txtHarnessModelName;
        private Label lblRtsp;
        private TextBox txtRtspUrl;
        private Label lblSteps;
        private NumericUpDown numSteps;
        private DataGridView grid;
        private Panel panelButtons;
        private Button btnGenerateSql;
        private Button btnClose;
        private Button btnFillDefaults;
    }
}


