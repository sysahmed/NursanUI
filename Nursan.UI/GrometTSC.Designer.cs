namespace Nursan.UI
{
    partial class GrometTSC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElTest));
            notifyIcon = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            lblVersion = new Label();
            txtRevork = new TextBox();
            lblCountProductions = new Label();
            btnAriza = new Button();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon
            // 
            resources.ApplyResources(notifyIcon, "notifyIcon");
            notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
            notifyIcon.MouseUp += notifyIcon_MouseUp;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, closeToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(contextMenuStrip1, "contextMenuStrip1");
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(openToolStripMenuItem, "openToolStripMenuItem");
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            resources.ApplyResources(closeToolStripMenuItem, "closeToolStripMenuItem");
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // button1
            // 
            resources.ApplyResources(button1, "button1");
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            resources.ApplyResources(button2, "button2");
            button2.Name = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // lblVersion
            // 
            resources.ApplyResources(lblVersion, "lblVersion");
            lblVersion.Name = "lblVersion";
            lblVersion.Click += label2_Click;
            // 
            // txtRevork
            // 
            resources.ApplyResources(txtRevork, "txtRevork");
            txtRevork.Name = "txtRevork";
            // 
            // lblCountProductions
            // 
            lblCountProductions.BackColor = Color.Transparent;
            lblCountProductions.FlatStyle = FlatStyle.Flat;
            resources.ApplyResources(lblCountProductions, "lblCountProductions");
            lblCountProductions.ForeColor = Color.Lime;
            lblCountProductions.Name = "lblCountProductions";
            // 
            // btnAriza
            // 
            btnAriza.AutoEllipsis = true;
            btnAriza.BackColor = Color.Transparent;
            resources.ApplyResources(btnAriza, "btnAriza");
            btnAriza.ForeColor = Color.Lime;
            btnAriza.Name = "btnAriza";
            btnAriza.TabStop = false;
            btnAriza.UseVisualStyleBackColor = false;
            // 
            // ElTest
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnAriza);
            Controls.Add(lblCountProductions);
            Controls.Add(txtRevork);
            Controls.Add(lblVersion);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ElTest";
            TopMost = true;
            TransparencyKey = Color.Transparent;
            Load += GrometTSC_Load;
            Move += ElTest_Move;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem;
        private Button button1;
        private Button button2;
        private Label label1;
        private Label lblVersion;
        private TextBox txtRevork;
        private Button btnAriza;
        public Label lblCountProductions;
    }
}