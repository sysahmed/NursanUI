using Nursan.Business.Services;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;

using Nursan.Persistanse.UnitOfWork;
using Nursan.Personal.Valadation;
using Nursan.Validations.ValidationCode;

namespace Nursan.UI
{
    public partial class SicilOkumaAP : Form
    {
        private Point MouseXY;

        private int _ScreenNumber;
        private static UnitOfWork _repo;
        private readonly UretimOtomasyonContext _context;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<UrModulerYapi> _modulerYapiList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        private static List<SyBarcodeOut> _syBarcodeOutList;
        private static List<SyPrinter> _syPrinterList;
        private static List<OrFamily> _familyList;
        private static MakineOpsionGetir _mkOpsion;

        private PersonalValidasyonu _personal;
        private string _vardiyaString;
        private List<UrPersonalTakib> urPersonalTakibs;
        private UrPersonalTakib urPersonalTakibsTek;
        //private IEnumerable<UrIstasyon> istasyonmList;

        private Nursan.Domain.Personal.Personal personalTek;
        private DateTime date;
        UretimOtomasyonContext uretim = new();
        string names;
        private static SicilOkumaAP instance;
        private Dictionary<string, Form> openForms = new Dictionary<string, Form>();
        private UrIstasyon istasyonce;
       SayiIzlemeSIcilBagizliService sayim; TorkService tork;
        public SicilOkumaAP(UnitOfWork repo, string vardiyaString)
        {
            _repo = repo;
            _vardiyaString = vardiyaString;
            _vardiya = new UrVardiya();
            _ScreenNumber = 0;

            _personal = new PersonalValidasyonu(new UnitOfWorPersonal(new PersonalContext()), new UnitOfWork(new UretimOtomasyonContext()));
            tork = new TorkService(new UnitOfWork(new UretimOtomasyonContext()), new UrVardiya { Name = _vardiyaString });
            istasyonce = tork.GetIstasyon();
           
            urPersonalTakibs = _personal.GetPersonalTakib(istasyonce).Data;
            date = OtherTools.GetValuesDatetime();
            InitializeComponent();
        }
        Form GetFormByName(string name, params object[] parameters)
        {
            if (openForms.TryGetValue(name, out Form existingForm))
            {
                // Формата вече е отворена, показваме я и я преместваме най-отгоре
                existingForm.Show();
                existingForm.BringToFront();
                return existingForm;
            }
            else
            {
                // Формата не е отворена, създаваме нова инстанция и я добавяме в колекцията
                try
                {
                    var form = CreateFormInstance(name, parameters);
                    openForms[name] = form;
                    //form.Show(); 
                    return form;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        private Form CreateFormInstance(string name, params object[] parameters)
        {
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            Type type = myAssembly.GetTypes().FirstOrDefault(t => t.BaseType != null && t.BaseType.FullName == "System.Windows.Forms.Form" && t.FullName == name);

            if (type != null)
            {
                // Проверка и извикване на публичния конструктор с параметри
                var constructor = type.GetConstructor(parameters.Select(p => p.GetType()).ToArray());
                if (constructor != null)
                {
                    return constructor.Invoke(parameters) as Form;
                }
            }

            return null;
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
        private void ProcessEnterKeyPress()
        {
            string inputText = textBox1.Text.Trim();

            if (inputText.StartsWith("*"))
            {
                ProcessStarPrefixedInput(inputText);
            }
            else if (inputText == _vardiyaString)
            {
                Strating();
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
                Nursan.Domain.Personal.Personal personal = GetPersonalInfo(inputText);

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
            return _personal.GetPersonal(sicilNumber).Data;
        }
        private void ProcessPersonalInfo(Nursan.Domain.Personal.Personal personal)
        {
            DateTime date = OtherTools.GetValuesDatetime();
            bool r1 = GitPersonalBak(istasyonce, personal.USER_CODE);

            if (r1 && personal.LAST_DIR == true)
            {
                ProcessValidPersonalInfo(personal, date);
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
            listBox1.Items.Add($"{personal.USER_CODE}-{personal.FIRST_NAME} {personal.LAST_NAME}");
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
        private void HandleException()
        {
            this.Hide();
            //this.TetikSicilOkuma(this, new EventArgs());
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
        private void Strating()
        {
            _mkOpsion = new MakineOpsionGetir(_context, _repo, out _makine, out _vardiya, out _istasyonList, out _modulerYapiList, out _syBarcodeInputList, out _syBarcodeOutList, out _syPrinterList, out _familyList, _vardiyaString);
            Form frm;
            var result = _istasyonList.Where(x => x.MashinId == _makine.Id);
            if (_vardiya is not null)
            {
                try
                {
                    names = _modulerYapiList.SingleOrDefault(x => x.Id == result.Select(x => x.ModulerYapiId).First()).Etap;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Object reference not set to an instance of an object." || ex.Message == "Sequence contains no elements")
                    {
                        names = "Revork";
                    }
                    else
                    {
                        return;
                    }
                }
                this.Hide();
                object[] verConst = new object[] { _repo, _makine, _vardiya, _istasyonList, _modulerYapiList, _syBarcodeInputList, _syBarcodeOutList, _syPrinterList, _familyList };
                frm = GetFormByName("Nursan.UI." + names, verConst);
                frm.Show();
            }
        }
        private void SicilOkuma_Load(object sender, EventArgs e)
        {
            GetSicilSayisal();
        }
        private int GitSytemDeAyiklaVesay(string? sicil)
        {
            var result = SayiIzlemeSIcilBagizliService.SayiHesapla(sicil, _vardiyaString);
            return ((int)istasyonce.Realadet + result);
        }
        private void GetSicilSayisal()
        {
            listBox1.Items.Clear();

            var result = _personal.GetPersonalTakib(istasyonce).Data;
            foreach (var item in result)
            {
                listBox1.Items.Add($"{item.Sicil}-{item.FullName}-{GitSytemDeAyiklaVesay(item.Sicil)}");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

    }
}
