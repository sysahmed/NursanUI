using Nursan.Domain.NursanBarcode;
using Nursan.Validations.Interface;
namespace Nursan.UI.Kasalama
{
    public partial class Kasalama : Form
    {
        private System.Windows.Forms.Timer focusTimer; // Декларираме таймера като поле на класа

        public Kasalama()
        {
            InitializeComponent();

            // Инициализиране на таймера
            focusTimer = new System.Windows.Forms.Timer();
            focusTimer.Interval = 10; // 10 ms забавяне
            focusTimer.Tick += FocusTimer_Tick;
        }

        private void Kasalama_Load(object sender, EventArgs e)
        {
            textBox1.Clear();
            this.BeginInvoke((Action)(() =>
            {
                textBox1.Focus();
            }));
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listBox1.Items.Add(textBox1.Text); // Добавяне на текста в списъка
                textBox1.Clear(); // Изчистване на полето
                focusTimer.Start(); // Стартираме таймера за задаване на фокус
            }
        }

        // Събитие за таймера
        private void FocusTimer_Tick(object sender, EventArgs e)
        {
            focusTimer.Stop(); // Спиране на таймера
            textBox1.Focus();  // Задаване на фокуса
        }

       
    }
}
