using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Nursan.UI
{
    public partial class BoltCameraConfigForm : Form
    {
        private class BoltRow
        {
            public int StepIndex { get; set; }
            public int RoiX { get; set; }
            public int RoiY { get; set; }
            public int RoiWidth { get; set; }
            public int RoiHeight { get; set; }
            public decimal? TorqueTarget { get; set; }
            public decimal? TorqueMin { get; set; }
            public decimal? TorqueMax { get; set; }
            public decimal? AngleTarget { get; set; }
            public decimal? AngleMin { get; set; }
            public decimal? AngleMax { get; set; }
            public string Notes { get; set; }
        }

        private readonly BindingSource _binding = new BindingSource();

        public BoltCameraConfigForm()
        {
            InitializeComponent();
        }

        private void BoltCameraConfigForm_Load(object sender, EventArgs e)
        {
            PrepareGrid();
            ApplyStepCount((int)numSteps.Value);
        }

        private void PrepareGrid()
        {
            _binding.DataSource = new List<BoltRow>();
            grid.DataSource = _binding;

            grid.Columns.Clear();
            grid.AutoGenerateColumns = false;

            grid.Columns.Add(CreateTextColumn("StepIndex", "Стъпка", 60));
            grid.Columns.Add(CreateTextColumn("RoiX", "ROI X", 70));
            grid.Columns.Add(CreateTextColumn("RoiY", "ROI Y", 70));
            grid.Columns.Add(CreateTextColumn("RoiWidth", "ROI W", 70));
            grid.Columns.Add(CreateTextColumn("RoiHeight", "ROI H", 70));
            grid.Columns.Add(CreateTextColumn("TorqueTarget", "Torque Target", 90));
            grid.Columns.Add(CreateTextColumn("TorqueMin", "Torque Min", 90));
            grid.Columns.Add(CreateTextColumn("TorqueMax", "Torque Max", 90));
            grid.Columns.Add(CreateTextColumn("AngleTarget", "Angle Target", 90));
            grid.Columns.Add(CreateTextColumn("AngleMin", "Angle Min", 90));
            grid.Columns.Add(CreateTextColumn("AngleMax", "Angle Max", 90));
            grid.Columns.Add(CreateTextColumn("Notes", "Бележки", 150));
        }

        private DataGridViewTextBoxColumn CreateTextColumn(string dataProperty, string header, int width)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = dataProperty,
                HeaderText = header,
                Width = width,
                MinimumWidth = 50,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            return col;
        }

        private void numSteps_ValueChanged(object sender, EventArgs e)
        {
            ApplyStepCount((int)numSteps.Value);
        }

        private void ApplyStepCount(int count)
        {
            var list = (List<BoltRow>)_binding.DataSource;
            if (list == null) return;

            list.Clear();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new BoltRow
                {
                    StepIndex = i,
                    RoiX = 0,
                    RoiY = 0,
                    RoiWidth = 0,
                    RoiHeight = 0,
                    Notes = $"Болт {i}"
                });
            }
            _binding.ResetBindings(false);
        }

        private void btnFillDefaults_Click(object sender, EventArgs e)
        {
            ApplyStepCount((int)numSteps.Value);
        }

        private void btnGenerateSql_Click(object sender, EventArgs e)
        {
            string model = (txtHarnessModelName.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(model))
            {
                MessageBox.Show("Моля, въведете HarnessModelName.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHarnessModelName.Focus();
                return;
            }

            string rtsp = (txtRtspUrl.Text ?? string.Empty).Trim();
            var list = (List<BoltRow>)_binding.DataSource;
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("Няма дефинирани стъпки.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("-- INSERT-и за dbo.OrBoltProcess (копирай и изпълни ръчно)");
            sb.AppendLine("BEGIN TRAN;");
            foreach (var row in list)
            {
                sb.AppendLine("INSERT INTO dbo.OrBoltProcess (HarnessModelName, StepIndex, RoiX, RoiY, RoiWidth, RoiHeight, CameraRtspUrl, TorqueTarget, TorqueMin, TorqueMax, AngleTarget, AngleMin, AngleMax, IsEnabled, Notes, CreatedBy)");
                sb.Append("VALUES (");
                sb.Append($"N'{EscapeSql(model)}', {row.StepIndex}, {row.RoiX}, {row.RoiY}, {row.RoiWidth}, {row.RoiHeight}, ");
                sb.Append(string.IsNullOrEmpty(rtsp) ? "NULL, " : $"N'{EscapeSql(rtsp)}', ");
                sb.Append(FormatNullable(row.TorqueTarget) + ", ");
                sb.Append(FormatNullable(row.TorqueMin) + ", ");
                sb.Append(FormatNullable(row.TorqueMax) + ", ");
                sb.Append(FormatNullable(row.AngleTarget) + ", ");
                sb.Append(FormatNullable(row.AngleMin) + ", ");
                sb.Append(FormatNullable(row.AngleMax) + ", ");
                sb.Append("1, ");
                sb.Append(string.IsNullOrWhiteSpace(row.Notes) ? "NULL, " : $"N'{EscapeSql(row.Notes)}', ");
                sb.Append("N'system');");
                sb.AppendLine();
            }
            sb.AppendLine("COMMIT;");

            try
            {
                Clipboard.SetText(sb.ToString());
                MessageBox.Show("SQL INSERT командите са копирани в клипборда.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                // Ако Clipboard не успее, покажи в диалог
                using var dlg = new Form
                {
                    Text = "SQL INSERT-и",
                    Width = 900,
                    Height = 600,
                    StartPosition = FormStartPosition.CenterParent
                };
                var txt = new TextBox
                {
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Both,
                    Dock = DockStyle.Fill,
                    Text = sb.ToString()
                };
                dlg.Controls.Add(txt);
                dlg.ShowDialog(this);
            }
        }

        private static string EscapeSql(string input)
        {
            return (input ?? string.Empty).Replace("'", "''");
        }

        private static string FormatNullable(decimal? value)
        {
            return value.HasValue ? value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : "NULL";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


