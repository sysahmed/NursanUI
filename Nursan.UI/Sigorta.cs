using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nursan.Business.Services;
using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Domain.ModelsDisposible;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.Opsionlar;
using Nursan.Validations.SortedList;
using Nursan.Validations.ValidationCode;
using System.IO.Ports;

namespace Nursan.UI
{
    public partial class Sigorta : Form
    {
        //UretimOtomasyonContext contex;
        //private readonly string _name;
        //private UrModulerYapi _modulerYapi;
        //private List<UrIstasyon> _istasyonList;
        //private List<SyBarcodeInput> _syBarcodeINPUTList;//= new List<BarcodeINPUT>();
        //private List<UrModulerYapi> _modulerYapiList;// = new UrModulerYapi();
        //static ListViewItem listce;
        //IzDonanimHedef izDonanimHedef;
        //HarnesDonanimHedef harnesDonanimHedef;
        //List<IzDonanimHedef> donanimHedesECOND;
        //XMLIslemi xmlim;
        //SerialPort seril;
        private UrVardiya _vardiya;// = new UrVardiya();
        private OpMashin _makine;// = new OpMashin();
        private UrIstasyon _urIstasyon;//= new List<UrIstasyon>();
        private List<SyPrinter> _syPrtinterList;
        private List<SyBarcodeOut> _syBarcodeOUTList;// = new List<BarcodeOUT>();
        List<OrHarnessModel> harnesModelList;
        public event System.IO.Ports.SerialDataReceivedEventHandler DataReceived;
        List<OrHarnessModel> harnesModelListSecond;
        List<IzDonanimHedef> donanimHedefs;//= new List<DonanimHedef>();
        private readonly UnitOfWork _repo;
        List<OrFamily> familyList;
        GenrateIdDonanimManager generateIdDonanimManager;
        string konveyor = string.Empty;
        public static string _sicil;
        delegate void SetTextCallback(string text);
        List<HarnesDonanimHedef> harnesDonanimHedefsList;
        DirectPrinting directPrintin;
        TorkService torkServices;
        ToplamV769Services toplamV769Services;
        UrFabrika fabrika;
        public Sigorta(string sicil, UnitOfWork repo)
        {
            InitializeComponent();
            _repo = repo;
            toplamV769Services = new ToplamV769Services(_repo); harnesModelListSecond = new List<OrHarnessModel>();
        }
        public Sigorta(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> family)
        {
            InitializeComponent();
            //contex = new();
            //_modulerYapiList = modulerYapiList;
            //// _modulerYapi = modulerYapi;
            //_istasyonList = istasyonList;
            //_syBarcodeINPUTList = syBarcodeInputList;
            //// _family = family;
            //donanimHedesECOND = new List<IzDonanimHedef>();
            //harnesDonanimHedef = new HarnesDonanimHedef();
            //izDonanimHedef = new IzDonanimHedef();
            //xmlim = new XMLIslemi();
            //seril = new SerialPort();
            harnesDonanimHedefsList = new List<HarnesDonanimHedef>();
            _syPrtinterList = syPrinterList;
            _syBarcodeOUTList = syBarcodeOutList;
            _makine = makine;
            _vardiya = vardiya;
            _urIstasyon = istasyonList.FirstOrDefault(x => x.MashinId == _makine.Id & x.VardiyaId == vardiya.Id);
            _repo = repo;
            donanimHedefs = new List<IzDonanimHedef>();
            int lstw = listView1.Size.Width;
            int lsth = listView1.Size.Height;
            int btnx = button1.Location.X;
            int btny = button1.Location.Y;
            harnesModelListSecond = new List<OrHarnessModel>();
            button1.Location = new System.Drawing.Point(lstw + 25, btny);
            generateIdDonanimManager = new GenrateIdDonanimManager(_repo);
            torkServices = new TorkService(repo, vardiya);
            toplamV769Services = new ToplamV769Services(_repo);
            fabrika = _repo.GetRepository<UrFabrika>().Get(x => x.Id == _urIstasyon.FabrikaId).Data;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            //key.SetValue("NbgSystem", "\"" + Application.ExecutablePath + "\"");
            string pcismi = Environment.MachineName;
            VeriGetir();
        }

        #region Buton
        private void button1_Click_1(object sender, EventArgs e)
        {
            int sifir = 0;
            cbFamily.Items.Clear();
            listView1.Items.Clear();
            if (numericUpDownPrintCount.Value > 0)
            {
                for (int i = 0; i < numericUpDownPrintCount.Value; i++)
                {
                    int sayi = XMLIslemi.XmlBarkodSaniye();
                    Thread.Sleep(sayi);
                    if (label1.Text != "REFERANS")
                    {
                        string[] split1 = label1.Text.Split(" ");
                        string[] split2 = split1[1].Split("/");
                        var gl = harnesDonanimHedefsList.FirstOrDefault(x => x.harnessModel == $"{split1[0]}");

                        gl.Adet = gl.Adet + 1;
                        numericUpDown1.Value = numericUpDown1.Value + 1;
                        if (int.Parse(split2[1].ToString()) <= gl.Adet)
                        {
                            var harnesResult = harnesModelList.FirstOrDefault(x => x.HarnessModelName == gl.harnessModel);
                            var donanimHedece = donanimHedefs.FirstOrDefault(x => x.HarnesModelId == harnesResult.Id);
                            try
                            {
                                donanimHedece.Adet = gl.Adet = Convert.ToInt32(numericUpDown1.Value);
                                numericUpDown1.Value = Convert.ToDecimal(donanimHedece.Adet);
                                donanimHedece.Hedef = gl.Hedef;
                                _repo.GetRepository<IzDonanimHedef>().Update(donanimHedece);
                                label1.Text = $"{split1[0]} {split2[0]}/{donanimHedece.Adet}";
                                if (BarodeBas(split1, gl))
                                {
                                    VeriGetir();
                                }
                                else
                                {
                                    MessageBox.Show("Bir Hata Olustu! ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Bir Hata Olustu! " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lutfen Refeferans Secin!", "Referans Yok!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        VeriGetir();
                        MessageBox.Show("Lutfen Refeferans Secin!", "Referans Yok!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

            }
            else
            {
                if (label1.Text != "REFERANS")
                {
                    string[] split1 = label1.Text.Split(" ");
                    string[] split2 = split1[1].Split("/");
                    var gl = harnesDonanimHedefsList.FirstOrDefault(x => x.harnessModel == $"{split1[0]}");

                    gl.Adet = gl.Adet + 1;
                    numericUpDown1.Value = numericUpDown1.Value + 1;
                    if (int.Parse(split2[1].ToString()) <= gl.Adet)
                    {
                        var harnesResult = harnesModelList.FirstOrDefault(x => x.HarnessModelName == gl.harnessModel);
                        var donanimHedece = donanimHedefs.FirstOrDefault(x => x.HarnesModelId == harnesResult.Id);
                        try
                        {
                            donanimHedece.Adet = gl.Adet = Convert.ToInt32(numericUpDown1.Value);
                            numericUpDown1.Value = Convert.ToDecimal(donanimHedece.Adet);
                            donanimHedece.Hedef = gl.Hedef;
                            _repo.GetRepository<IzDonanimHedef>().Update(donanimHedece);
                            label1.Text = $"{split1[0]} {split2[0]}/{donanimHedece.Adet}";
                            if (BarodeBas(split1, gl))
                            {
                                VeriGetir();
                            }
                            else
                            {
                                MessageBox.Show("Bir Hata Olustu! ", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Bir Hata Olustu! " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lutfen Refeferans Secin!", "Referans Yok!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    VeriGetir();
                    MessageBox.Show("Lutfen Refeferans Secin!", "Referans Yok!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }


        }

        private bool BarodeBas(string[] split1, HarnesDonanimHedef gl)
        {
            try
            {
                var harness = harnesModelList.FirstOrDefault(x => x.HarnessModelName == gl.harnessModel);
                string[] fuix = StringSpanConverter.SplitWithoutAllocationReturnArray(harness.HarnessModelName.AsSpan(), '-');
                var istasyonNew = torkServices.GetByIstasyonSigorta(harness.FamilyId).Where(x => x.ModulerYapi.Id == 1);
                var istasyonNewEx = istasyonNew.Count() > 0 ? istasyonNew.FirstOrDefault(x => x.FabrikaId == _urIstasyon.FabrikaId) : _urIstasyon;
                var result = _repo.GetRepository<IzGenerateId>().Add(new IzGenerateId
                {
                    HarnesModelId = harness.Id,
                    UrIstasyonId = istasyonNewEx.Id,
                    Barcode = harness.HarnessModelName,
                    ReferasnLeght = harness.HarnessModelName.Length,
                    DonanimIdLeght = 8,
                    DonanimTorkReferansId = harness.OrHarnessConfigId,
                    AlertNumber = harness.AlertNumber,
                });

                result.Data.Barcode = harness.HarnessModelName + result.Data.Id.ToString().PadLeft((int)_syBarcodeOUTList.FirstOrDefault(x => x.Id == _urIstasyon.SyBarcodeOutId).PadLeft, '0');
                generateIdDonanimManager.UpdateIDGenerate(result.Data);
                toplamV769Services.AddToplamV769(new IzToplamV769
                {
                    IdDonanim = result.Data.Id.ToString(),
                    Referans = harness.HarnessModelName,
                    Sigorta = $"{Environment.MachineName}{_vardiya.Name}",
                    Sigortavar = $"{Environment.MachineName}{_vardiya.Name}",
                    Sigortagec = true,
                    Sigortadate = TarihHesapla.GetSystemDate(),
                    Alert = (int)harness.AlertNumber,
                    Fabrika = fabrika.Id,
                    SideCode = harness.SideCode,
                    CustomId = harness.CustomerID,
                    Konveyorb = true

                });
                if (result != null)
                {
                    var barcodeOut = _syBarcodeOUTList.FirstOrDefault(x => x.Id == _urIstasyon.SyBarcodeOutId);
                    barcodeOut.BarcodeIcerik = result.Data.Barcode;

                    barcodeOut.family = fuix[1];
                    barcodeOut.suffix = fuix[2];
                    barcodeOut.prefix = fuix[0];
                    barcodeOut.Sira1 = split1[1].Split("/")[1];
                    barcodeOut.IdDonanim = result.Data.Id.ToString().PadLeft((int)_syBarcodeOUTList.FirstOrDefault(x => x.Id == _urIstasyon.SyBarcodeOutId).PadLeft, '0');
                    barcodeOut.CreateDate = TarihHesapla.GetSystemDate();

                    var isDonanim = torkServices.AddDonanimCountSigorta(new SyBarcodeInput
                    {
                        BarcodeIcerik = barcodeOut.BarcodeIcerik,
                        RegexInt = barcodeOut.RegexInt,
                        RegexString = barcodeOut.RegexString,
                        ParcalamaChar = barcodeOut.ParcalamaChar,
                        PadLeft = barcodeOut.PadLeft,
                        OzelChar = barcodeOut.OzelChar,
                        StartingSubstring = barcodeOut.StartingSubstring,
                        StopingSubstring = barcodeOut.StopingSubstring
                    }, result.Data, istasyonNewEx);
                    //torkServices.AddDPaketlemeId(isDonanim.Data);
                    directPrintin = new DirectPrinting(barcodeOut, result.Data, _syPrtinterList.First());
                    directPrintin.KucukEtiketBas("V");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            VeriGetir();
        }

        private void AnaSayfa_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Other
        public void VeriGetir()
        {
            try
            {

                int sifir = 0;
                harnesDonanimHedefsList = null;
                harnesDonanimHedefsList = new List<HarnesDonanimHedef>();
                cbFamily.Items.Clear();
                listView1.Items.Clear();
                lstBiten.Items.Clear();
                donanimHedefs = _repo.GetRepository<IzDonanimHedef>().GetAll(x => x.Hedef > 0).Data;
                harnesModelList = _repo.GetRepository<OrHarnessModel>().GetAll(null).Data;
                familyList = _repo.GetRepository<OrFamily>().GetAll(null).Data;
                foreach (var item in donanimHedefs)
                {
                    var veri = harnesModelList.FirstOrDefault(x => x.Id == item.HarnesModelId);
                    harnesModelListSecond.Add(veri);
                }
                //var groupVeri = donanimHedefs.GroupBy(x => x.FamilyId);
                foreach (var item in familyList)
                {
                    cbFamily.Items.Add(item.FamilyName);
                }
                if (konveyor == "")

                {
                    konveyor = familyList.FirstOrDefault(x => x.Id == _urIstasyon.FamilyId).FamilyName;
                }

                var familyId = familyList.SingleOrDefault(x => x.FamilyName == konveyor);
                using (UretimOtomasyonContext db = new())
                {
                    var data = (from p in db.IzDonanimHedefs
                                join sr in db.OrHarnessModels
                      on p.HarnesModelId equals sr.Id into lJ
                                from res in lJ.DefaultIfEmpty()
                                where res.FamilyId == familyId.Id
                                select new
                                {
                                    res.Id,
                                    res.HarnessModelName,
                                    p.Adet,
                                    p.Hedef,
                                    p.IstasyonId,
                                }).OrderBy(x => x.HarnessModelName).ToList();
                    foreach (var i in data)
                    {

                        harnesDonanimHedefsList.Add(new HarnesDonanimHedef
                        {
                            harnessModel = i.HarnessModelName,
                            Id = i.Id,
                            IstasyonId = i.IstasyonId,
                            Hedef = i.Hedef,
                            Adet = i.Adet
                        });
                        //harnesDonanimHedef = null;
                    }
                    dataGridView1.DataSource = harnesDonanimHedefsList;
                    listView1.Items.Clear();
                    lstBiten.Items.Clear();
                    foreach (var i in harnesDonanimHedefsList)
                    {
                        if (i.Adet >= i.Hedef)
                        {
                            lstBiten.Items.Add($"{i.harnessModel} {i.Hedef}/{i.Adet}");
                        }

                        else
                        {
                            listView1.Items.Add($"{i.harnessModel} {i.Hedef}/{i.Adet}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void GitButonuAtivEt(object source, EventArgs e)
        {
            button1.Enabled = true;
        }
        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count <= 0)
            {
                return;
            }
            int intselectedindex = listView1.SelectedIndices[0];
            if (intselectedindex >= 0)
            {
                //listce = listView1.Items[intselectedindex];
                String text = listView1.Items[intselectedindex].Text;
                label1.Text = text;
                try
                {
                    string[] parca = label1.Text.Split('/');
                    numericUpDown1.Value = int.Parse(parca[1]);
                }
                catch (Exception ex)
                {

                }
                //_repo.GetRepository<IzDonanimHedef>().Get(x => x.ReferansName == label1.Text);
            }
        }
        #endregion

        private void cbKonveyor_SelectedIndexChanged(object sender, EventArgs e)
        {
            konveyor = cbFamily.Text;
            VeriGetir();
        }

        private void cbFamily_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                konveyor = cbFamily.Text;
                VeriGetir();
                cbFamily.Text = "";
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1.Clear();
                //VeriGetirETME(textBox1.Text);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            int sifir = 0;
            harnesDonanimHedefsList = null;
            harnesDonanimHedefsList = new List<HarnesDonanimHedef>();
            cbFamily.Items.Clear();
            listView1.Items.Clear();
            lstBiten.Items.Clear();
            donanimHedefs = _repo.GetRepository<IzDonanimHedef>().GetAll(x => x.Hedef > 0).Data;
            harnesModelList = _repo.GetRepository<OrHarnessModel>().GetAll(null).Data;
            familyList = _repo.GetRepository<OrFamily>().GetAll(null).Data;
            foreach (var item in donanimHedefs)
            {
                var veri = harnesModelList.SingleOrDefault(x => x.Id == item.HarnesModelId);
                harnesModelListSecond.Add(veri);
            }
            //var groupVeri = donanimHedefs.GroupBy(x => x.FamilyId);
            foreach (var item in familyList)
            {
                cbFamily.Items.Add(item.FamilyName);
            }
            if (konveyor == "")

            {
                konveyor = familyList.FirstOrDefault(x => x.Id == _urIstasyon.FamilyId).FamilyName;
            }

            var familyId = familyList.SingleOrDefault(x => x.FamilyName == konveyor);
            using (UretimOtomasyonContext db = new())
            {
                var data = (from p in db.IzDonanimHedefs
                            join sr in db.OrHarnessModels
                  on p.HarnesModelId equals sr.Id into lJ
                            from res in lJ.DefaultIfEmpty()
                            where res.FamilyId == familyId.Id && res.HarnessModelName.Contains(textBox1.Text)
                            select new
                            {
                                res.Id,
                                res.HarnessModelName,
                                p.Adet,
                                p.Hedef,
                                p.IstasyonId,
                            }).ToList();
                foreach (var i in data)
                {

                    harnesDonanimHedefsList.Add(new HarnesDonanimHedef
                    {
                        harnessModel = i.HarnessModelName,
                        Id = i.Id,
                        IstasyonId = i.IstasyonId,
                        Hedef = i.Hedef,
                        Adet = i.Adet
                    });
                    //harnesDonanimHedef = null;
                }
                dataGridView1.DataSource = harnesDonanimHedefsList;
                listView1.Items.Clear();
                lstBiten.Items.Clear();
                foreach (var i in harnesDonanimHedefsList)
                {
                    if (i.Adet >= i.Hedef)
                    {
                        lstBiten.Items.Add($"{i.harnessModel} {i.Hedef}/{i.Adet}");
                    }

                    else
                    {
                        listView1.Items.Add($"{i.harnessModel} {i.Hedef}/{i.Adet}");
                    }
                }
            }
        }
    }
}


