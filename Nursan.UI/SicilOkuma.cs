using Nursan.Business.Services;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Personal.Valadation;
using Nursan.Validations.ValidationCode;

namespace Nursan.UI
{
    public partial class SicilOkuma : Form
    {
        //private Point MouseXY;
        //private IEnumerable<Nursan.Domain.Personal.Personal> personalLIst;
        //private DateTime date;
        private string _vardiya;
        //private UrPersonalTakib urPersonalTakib;
        private int _ScreenNumber;
        public event EventHandler TetikSicilOkuma;
        private PersonalValidasyonu _personal;
        private List<UrPersonalTakib> urPersonalTakibs;
        private UrPersonalTakib urPersonalTakibsTek;
        private Nursan.Domain.Personal.Personal personal;
        private UrIstasyon istasyonce;
        private TorkService tork;
        public SicilOkuma(string vardiya)
        {
            InitializeComponent();
           _vardiya = vardiya;
            _ScreenNumber = 0;
            Nursan.Domain.Personal.Personal personal;
            _personal = new PersonalValidasyonu(new UnitOfWorPersonal(new PersonalContext()), new UnitOfWork(new UretimOtomasyonContext()));
            tork = new TorkService(new UnitOfWork(new UretimOtomasyonContext()), new UrVardiya { Name = vardiya });
            istasyonce = tork.GetIstasyon();
            urPersonalTakibs = _personal.GetPersonalTakib(istasyonce).Data;
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    ProcessEnterKeyPress();
                }
            }
            catch (Exception)
            {
                HandleException();
            }
        }
        private void GetSicilSayisal()
        {

            var result = _personal.GetPersonalTakib(istasyonce).Data;
            foreach (var item in result)
            {
                // Проверяваме дали персоналът е в фабриката (LAST_DIR = true)
                var personal = _personal.GetPersonal(item.Sicil).Data;
                
                // Добавяме в листа само ако персоналът е в фабриката (LAST_DIR = true)
                // Ако LAST_DIR е null или false, не добавяме в листа
                if (personal?.LAST_DIR == true)
                {
                    listBox1.Items.Add($"{item.Sicil}-{item.FullName}-{GitSytemDeAyiklaVesay(item.Sicil)}-В ФАБРИКАТА");
                }
            }
        }
        private int GitSytemDeAyiklaVesay(string? sicil)
        {

            //sayim = new SayiIzlemeSIcilBagizliService(istasyonce);
            var result = SayiIzlemeSIcilBagizliService.SayiHesapla(sicil, _vardiya);
            return ((int)istasyonce.Realadet + result);
        }

        private void ProcessEnterKeyPress()
        {
            string inputText = textBox1.Text.Trim();

            if (inputText.StartsWith("*"))
            {
                ProcessStarPrefixedInput(inputText);
            }
            else if (inputText.StartsWith('_'))
            {
                foreach (var item in urPersonalTakibs)
                {
                    _personal.DeletePersonalTakib(item, item.DayOfYear);
                }
                listBox1.Items.Clear();
                textBox1.Clear();
            }
            else
            {
                label1.Text = $"Sicil Okuttunz!!!  - {istasyonce.Name}";
                label1.ForeColor = Color.Red;
                textBox1.Clear();
            }
        }
        private void ProcessStarPrefixedInput(string inputText)
        {
            if (istasyonce != null)
            {
              personal  = GetPersonalInfo(inputText);

                if (personal != null)
                {
                    ProcessPersonalInfo(personal);
                }
                else
                {
                    HandleSicilNotFound();
                }
            }
            else
            {
                textBox1.Clear();
            }
        }
        private Nursan.Domain.Personal.Personal GetPersonalInfo(string inputText)
        {
            string sicilNumber = inputText.Substring(1);
            var result= _personal.GetPersonal(sicilNumber).Data;
            return result;
        }
        private void ProcessPersonalInfo(Nursan.Domain.Personal.Personal personal)
        {
            DateTime date = OtherTools.GetValuesDatetime();
            bool r1 = GitPersonalBak(istasyonce, personal.USER_CODE);

            if (r1 && personal.LAST_DIR == true)
            {
                ProcessValidPersonalInfo(personal, date);
            }
            else if (personal.LAST_DIR == false || personal.LAST_DIR == null)
            {
                // Персоналът не е в фабриката (LAST_DIR = false или null)
                HandlePersonNotInFactory(personal);
            }
            else
            {
                HandleInvalidPersonalInfo(personal.LAST_DIR);
            }
        }
        private void ProcessValidPersonalInfo(Nursan.Domain.Personal.Personal personal, DateTime date)
        {
            UrPersonalTakib urPersonalTakib = _personal.GetPersonalAndSicilTakibTek(personal.USER_CODE).Data;

            if (urPersonalTakib == null)
            {
                AddPersonalTakib(personal, date);
            }
            else
            {
                UpdatePersonalTakib(urPersonalTakib, date);
            }
        }
        private void AddPersonalTakib(Nursan.Domain.Personal.Personal personal, DateTime date)
        {
            _personal.ADDPersonalTakib(new Domain.Entity.UrPersonalTakib
            {
                Sicil = personal.USER_CODE,
                FullName = $"{personal.FIRST_NAME} {personal.LAST_NAME}",
                Department = personal.DEPARTMENT,
                UrIstasyonId = istasyonce.Id,
                DayOfYear = $"{istasyonce.Id}*{date.Year}{date.Month}{date.Day}",
                CreateDate = date,
                UpdateDate = date
            });

            UpdateUIOnSuccess(personal);
        }
        private void UpdatePersonalTakib(UrPersonalTakib urPersonalTakib, DateTime date)
        {
            urPersonalTakib.UrIstasyonId = istasyonce.Id;
            urPersonalTakib.UpdateDate = date;
            urPersonalTakib.DayOfYear = $"{istasyonce.Id}*{date.Year}{date.Month}{date.Day}";

            _personal.UpdatePersonalTakib(urPersonalTakib);
            UpdateUIOnSuccess(_personal.GetPersonal(urPersonalTakib.Sicil).Data);
        }
        private void UpdateUIOnSuccess(Nursan.Domain.Personal.Personal personal)
        {
            textBox1.Clear();
            // Добавяме в листа само ако персоналът е в фабриката (LAST_DIR = true)
            // Ако LAST_DIR е null или false, не добавяме в листа
            if (personal?.LAST_DIR == true)
            {
                listBox1.Items.Add($"{personal.USER_CODE}-{personal.FIRST_NAME} {personal.LAST_NAME}-В ФАБРИКАТА");
            }
            label1.Text = $"Sicil Okuttunz!!!  - {istasyonce.Name}";
            label1.ForeColor = Color.Lime;
        }
        private void HandleInvalidPersonalInfo(bool? lastDir)
        {
            textBox1.Clear();
            string message = (bool)lastDir
                ? "Bu Sicil Systemde Var!!!"
                : "Bu Sicil Fabrikada Gozukmuyor!!!";

            label1.Text = $"{message} - {istasyonce.Name}";
            label1.ForeColor = Color.Red;
        }
        private void HandleSicilNotFound()
        {
            textBox1.Clear();
            label1.Text = $"Bu Sicil Systemde Gozukmuyor!!! - {istasyonce.Name}";
            label1.ForeColor = Color.Red;
        }

        private void HandlePersonNotInFactory(Nursan.Domain.Personal.Personal personal)
        {
            textBox1.Clear();
            label1.Text = $"Персоналът {personal.FIRST_NAME} {personal.LAST_NAME} НЕ Е В ФАБРИКАТА!!! - {istasyonce.Name}";
            label1.ForeColor = Color.Red;
        }
        private void HandleException()
        {
            this.Hide();
            this.TetikSicilOkuma(this, new EventArgs());
            textBox1.Clear();
            this.Close();
        }
        private bool GitPersonalBak(UrIstasyon station, string v)
        {
            urPersonalTakibsTek = _personal.GetPersonalAndSicilTakib(station, v).Data;
            if (urPersonalTakibsTek != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void SicilOkuma_Load(object sender, EventArgs e)
        {
            // При всяко отваряне на формата проверяваме статуса на всички сиджили
            CheckAllPersonalFactoryStatus();
            
            listBox1.Items.Clear();
            base.Bounds = Screen.AllScreens[this._ScreenNumber].Bounds;
            
            //System.Windows.Forms.Cursor.Hide();
            base.TopMost = true;
            foreach (var item in urPersonalTakibs)
            {
                // Проверяваме дали персоналът е в фабриката (LAST_DIR = true)
                var personal = _personal.GetPersonal(item.Sicil).Data;
                
                // Добавяме в листа само ако персоналът е в фабриката (LAST_DIR = true)
                // Ако LAST_DIR е null или false, не добавяме в листа
                if (personal?.LAST_DIR == true)
                {
                    listBox1.Items.Add($"{item.Sicil}-{item.FullName}-В ФАБРИКАТА");
                }
            }
        }
        /// <summary>
        /// Проверява статуса на всички персонали дали са в фабриката
        /// </summary>
        private void CheckAllPersonalFactoryStatus()
        {
            try
            {
                var allPersonalTakib = _personal.GetPersonalTakib(istasyonce).Data;
                foreach (var item in allPersonalTakib)
                {
                    var personal = _personal.GetPersonal(item.Sicil).Data;
                    if (personal != null)
                    {
                        // Логваме статуса на персонала
                        Console.WriteLine($"Сиджил: {item.Sicil}, Име: {item.FullName}, В фабриката: {personal.LAST_DIR}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при проверка на статуса на персоналите: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Hide();
            //this.TetikSicilOkuma(this, new EventArgs());
            this.Close();
        }
    }
}
