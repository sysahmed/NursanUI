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
                try
                {
                    ValidateAndSave();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    MessageBox.Show($"ГРЕШКА: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}", "Изключение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Непозната грешка: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ValidateAndSave()
        {
            if (string.IsNullOrWhiteSpace(textBoxBarcode1.Text) || textBoxBarcode1.Text.Length <= 1)
            {
                labelWarning.Text = "ГРЕШКА: Barcode1 е празен или твърде кратък!";
                labelWarning.ForeColor = Color.DarkRed;
                textBoxBarcode1.Clear();
                textBoxBarcode2.Clear();
                return;
            }
            string barcode1 = textBoxBarcode1.Text?.Trim() ?? string.Empty;
            string barcode2 = textBoxBarcode2.Text?.Trim() ?? string.Empty;
            if (barcode1.Length <= 1)
            {
                labelWarning.Text = "ГРЕШКА: Barcode1 е твърде кратък!";
                labelWarning.ForeColor = Color.DarkRed;
                return;
            }
            if(barcode1.StartsWith("#"))
            _firstBarcode = barcode1.Substring(1);
            _firstBarcode = barcode1;

            if (!int.TryParse(barcode2, out int alertNumber))
            {
                labelWarning.Text = "ГРЕШКА: Alert Number трябва да е число!";
                labelWarning.ForeColor = Color.DarkRed;
                textBoxBarcode2.Clear();
                textBoxBarcode2.Focus();
                return;
            }

            _alertNumber = alertNumber;

            if (_harnessModel == null)
            {
                labelWarning.Text = "ГРЕШКА: Няма зареден харнес модел!";
                labelWarning.ForeColor = Color.DarkRed;
                textBoxBarcode2.Clear();
                textBoxBarcode2.Focus();
                return;
            }

            if (string.IsNullOrEmpty(_harnessModel.HarnessModelName))
            {
                labelWarning.Text = "ГРЕШКА: HarnessModelName е празен!";
                labelWarning.ForeColor = Color.DarkRed;
                return;
            }

            if (!_firstBarcode.Contains(_harnessModel.HarnessModelName) || _harnessModel.AlertNumber != _alertNumber)
            {
                labelWarning.Text = "ГРЕШКА: Баркодовете не съвпадат с харнес модела или Alert Number не е коректен!";
                labelWarning.ForeColor = Color.DarkRed;
                textBoxBarcode2.Clear();
                textBoxBarcode2.Focus();
                return;
            }

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
            System.Threading.Thread.Sleep(700);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        //private void ValidateAndSave()
        //{
        //    _firstBarcode = textBoxBarcode1.Text.Substring(1).Trim();
        //    if (!int.TryParse(textBoxBarcode2.Text.Trim(), out int alertNumber))
        //    {
        //        labelWarning.Text = "ГРЕШКА: Alert Number трябва да е число!";
        //        labelWarning.ForeColor = Color.DarkRed;
        //        textBoxBarcode2.Clear();
        //        textBoxBarcode2.Focus();
        //        return;
        //    }
        //    _alertNumber = alertNumber;

        //    // Проверка дали баркодовете съвпадат с харнес модела
        //    if (_harnessModel == null && !_firstBarcode.Contains(_harnessModel.HarnessModelName) && _harnessModel.AlertNumber != _alertNumber)
        //    {
        //        labelWarning.Text = "ГРЕШКА: Баркодовете не съвпадат с харнес модела или Alert Number не е коректен!";
        //        labelWarning.ForeColor = Color.DarkRed;
        //        textBoxBarcode2.Clear();
        //        textBoxBarcode2.Focus();
        //        return;
        //    }

        //    // Запис в OrAlertGk
        //    var alertGk = new OrAlertGk
        //    {
        //        AlertNumber = _alertNumber.Value,
        //        Tarih = DateTime.Now
        //    };
        //    _repo.GetRepository<OrAlertGk>().Add(alertGk);
        //    _repo.SaveChanges();

        //    labelWarning.Text = "АЛЕРТЪТ Е ОТКЛЮЧЕН!";
        //    labelWarning.ForeColor = Color.DarkGreen;
        //    labelInfo.Text = "Успешно отключихте алармата!";
        //    labelInfo.ForeColor = Color.DarkGreen;
        //    this.Refresh();
        //    System.Threading.Thread.Sleep(700); // Кратка пауза за визуален ефект
        //    this.DialogResult = DialogResult.OK;
        //    this.Close();
        //}

        private void AlertGkLockedOpen_Load(object sender, EventArgs e)
        {
            textBoxBarcode1.Focus();    
        }
    }
} 