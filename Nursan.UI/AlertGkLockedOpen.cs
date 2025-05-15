using System;
using System.Windows.Forms;
using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using System.Drawing;

namespace Nursan.UI
{
    public partial class AlertGkLockedOpen : Form
    {
        private readonly IUnitOfWork _repo;
        private OrHarnessModel _harnessModel;
        private string _firstBarcode;
        private int? _alertNumber;

        public AlertGkLockedOpen(IUnitOfWork repo, OrHarnessModel harnessModel)
        {
            InitializeComponent();
            _repo = repo;
            _harnessModel = harnessModel;
            labelWarning.Text = "ВНИМАНИЕ: АЛЕРТЪТ Е ЗАКЛЮЧЕН!";
            labelWarning.ForeColor = Color.DarkRed;
            labelInfo.Text = "Моля, сканирайте първо целия баркод (с ID), после баркод с Alert Number.";
            textBoxBarcode1.Focus();
            textBoxBarcode1.KeyDown += TextBoxBarcode1_KeyDown;
            textBoxBarcode2.KeyDown += TextBoxBarcode2_KeyDown;
        }

        private void TextBoxBarcode1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(textBoxBarcode1.Text))
            {
                textBoxBarcode2.Focus();
            }
        }

        private void TextBoxBarcode2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(textBoxBarcode2.Text))
            {
                ValidateAndSave();
            }
        }

        private void ValidateAndSave()
        {
            _firstBarcode = textBoxBarcode1.Text.Substring(1).Trim();
            if (!int.TryParse(textBoxBarcode2.Text.Trim(), out int alertNumber))
            {
                labelWarning.Text = "ГРЕШКА: Alert Number трябва да е число!";
                labelWarning.ForeColor = Color.DarkRed;
                textBoxBarcode2.Clear();
                textBoxBarcode2.Focus();
                return;
            }
            _alertNumber = alertNumber;

            // Проверка дали баркодовете съвпадат с харнес модела
            if (_harnessModel == null || !_firstBarcode.Contains(_harnessModel.HarnessModelName) || _harnessModel.AlertNumber != _alertNumber)
            {
                labelWarning.Text = "ГРЕШКА: Баркодовете не съвпадат с харнес модела или Alert Number не е коректен!";
                labelWarning.ForeColor = Color.DarkRed;
                textBoxBarcode2.Clear();
                textBoxBarcode2.Focus();
                return;
            }

            // Запис в OrAlertGk
            var alertGk = new OrAlertGk
            {
                AlertNumber = _alertNumber.Value,
                Tarih = DateTime.Now
            };
            _repo.GetRepository<OrAlertGk>().Add(alertGk);
            _repo.SaveChanges();

            labelWarning.Text = "АЛЕРТЪТ Е ОТКЛЮЧЕН!";
            labelWarning.ForeColor = Color.DarkGreen;
            labelInfo.Text = "Успешно отключихте алармата!";
            labelInfo.ForeColor = Color.DarkGreen;
            this.Refresh();
            System.Threading.Thread.Sleep(700); // Кратка пауза за визуален ефект
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AlertGkLockedOpen_Load(object sender, EventArgs e)
        {
            textBoxBarcode1.Focus();    
        }
    }
} 