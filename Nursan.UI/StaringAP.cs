using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Personal.Valadation;
using Nursan.Validations.ValidationCode;

namespace Nursan.UI
{
    public partial class StaringAP : Form
    {
        private readonly UnitOfWork _repo;
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

        private string _vardiyaString;
       // private PersonalValidasyonu _personal;
       // private IEnumerable<UrPersonalTakib> urPersonalTakibs;
       // private UrPersonalTakib urPersonalTakib;
        //private IEnumerable<UrIstasyon> istasyonmList;
        //private IEnumerable<Nursan.Domain.Personal.Personal> personalLIst;
        //private Nursan.Domain.Personal.Personal personal;
        //private DateTime date;
        UretimOtomasyonContext uretim = new();
        SicilOkumaAP sicilForm;


        public StaringAP(UnitOfWork repo)
        {
            _repo = repo;
            InitializeComponent();

            //_personal = new PersonalValidasyonu(new UnitOfWorPersonal(new PersonalContext()), _repo);
            _vardiya = new UrVardiya();

        }
        public StaringAP()
        {

        }
        private void Staring_Load(object sender, EventArgs e)
        {

        }

        string names;
        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtBarcode.Text))
                {

                    if (txtBarcode.Text.Substring(0, 1) is not "*")
                    {
                        _vardiya.Name = txtBarcode.Text;
                        sicilForm = new SicilOkumaAP(_repo, txtBarcode.Text);
                        this.Hide();
                        sicilForm.ShowDialog();
                        // this.Dispose();
                        // this.Close();
                    }
                    else
                    {
                        txtBarcode.Clear();
                    }
                }
            }
        }

        private void SicilForm_TetikSicilOkuma(object? sender, EventArgs e)
        {
            Strating();
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
                frm = GetFormByName("Nursan.UI." + names);
                frm.ShowDialog();
                this.Dispose();
                this.Close();
            }
        }
        Form GetFormByName(string name)
        {
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (Type type in myAssembly.GetTypes())
            {
                if (type.BaseType != null && type.BaseType.FullName == "System.Windows.Forms.Form")
                {
                    if (type.FullName == name)
                    {
                        var form = Activator.CreateInstance(Type.GetType(name), _repo, _makine, _vardiya, _istasyonList, _modulerYapiList, _syBarcodeInputList, _syBarcodeOutList, _syPrinterList, _familyList) as Form;
                        form.Name = name;
                        form.Text = name;
                        return form;
                    }
                }
            }
            return null;

        }
    }
}
