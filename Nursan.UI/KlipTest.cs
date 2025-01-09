using Nursan.Business.Manager;
using Nursan.Core.Printing;
using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;
using System.Text.RegularExpressions;

namespace Nursan.UI
{
    public partial class KlipTest : Form
    {
        private string deger;
        FileSystemWatcher watcher1;
        FileSystemWatcher watcher2;
        FileSystemWatcher watcher3;
        private Messaglama mes = new Messaglama();
        private XMLIslemi xmlis = new XMLIslemi();
        TorkService TorkService;
        static string[] pathc = { "C:\\Kliptest\\", "C:\\Kliptest_2\\", "C:\\Klt\\", "C:\\DEMO\\", "C:\\_Kliptest\\", };
        EltestValidasyonlari _elTest;
        static string format = $"*.txt";
        ScreenSaverForm scren;
        SicilOkuma sicil;

        UnitOfWork _repo;
        public KlipTest(UnitOfWork repo)
        {
            InitializeComponent();

            GitDirektoryBac();
            this.watcher1 = new FileSystemWatcher(pathc[0], format);
            this.watcher1.Created += new FileSystemEventHandler(this.Watcher1_Create);
            this.watcher2 = new FileSystemWatcher(pathc[1], format);
            this.watcher2.Created += new FileSystemEventHandler(this.Watcher2_Created);
            this.watcher3 = new FileSystemWatcher(pathc[2], format);
            this.watcher3.Created += new FileSystemEventHandler(this.Watcher3_Created);
            _elTest = new EltestValidasyonlari(repo);
            _repo = repo;
            //' lblVersion.Text += Environment.MachineName;
            lblVersion.Text = $"KlipTest {Environment.Version}";
        }
        private void GitDirektoryBac()
        {
            foreach (var item in pathc)
            {
                if (!Directory.Exists(item))
                {
                    Directory.CreateDirectory(item);
                }
            }

        }
        private void Watcher1_Create(object sender, FileSystemEventArgs e)
        {
            try
            {
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());
                Watcher1(veri.Path, veri.Filter);
            }
            catch (Exception)
            {

            }
        }
        private void Watcher2_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());
                Watcher2(veri.Path, veri.Filter);
            }
            catch (Exception)
            {

            }

        }
        private void Watcher3_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());
                Watcher3(veri.Path, veri.Filter);
            }
            catch (Exception)
            {

            }
        }
        private void Watcher1(string Path, string Format)
        {

            FileInfo[] files = (new DirectoryInfo(Path).GetFiles(Format));
            FileInfo[] fileInfoArray = files;

            for (int i = 0; i < (int)fileInfoArray.Length; i++)
            {
                FileInfo fileInfo = fileInfoArray[i];
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                this.deger = fileInfo.Name.ToUpper();
                try
                {
                    File.Copy(string.Concat(Path, fileInfo.Name), string.Concat(pathc[4].ToString(), fileInfo.Name), true);
                    var result = _elTest.GitSystemeYukle($"{fileInfo.Name.ToUpper()}");
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
                catch (ErrorExceptionHandller ex)
                {
                    mes.messanger(ex.Message);
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
            }

        }
        private void Watcher2(string Path, string Format)
        {
            FileInfo[] files = (new DirectoryInfo(Path).GetFiles(Format));
            FileInfo[] fileInfoArray = files;

            for (int i = 0; i < (int)fileInfoArray.Length; i++)
            {
                FileInfo fileInfo = fileInfoArray[i];
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                this.deger = fileInfo.Name.ToUpper();
                try
                {
                    File.Copy(string.Concat(Path, fileInfo.Name), string.Concat(pathc[4].ToString(), fileInfo.Name + "Start"), true);
                    GitSystemeDesktopKapa(fileInfo.Name.ToUpper());
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
                catch (ErrorExceptionHandller ex)
                {
                    mes.messanger(ex.Message);
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
            }
        }
        private void Watcher3(string Path, string Format)
        {
            FileInfo[] files = (new DirectoryInfo(Path).GetFiles(Format));
            FileInfo[] fileInfoArray = files;
            for (int i = 0; i < (int)fileInfoArray.Length; i++)
            {
                FileInfo fileInfo = fileInfoArray[i];
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                this.deger = fileInfo.Name.ToUpper();
                try
                {
                    File.Copy(string.Concat(Path, fileInfo.Name), string.Concat(pathc[4].ToString(), fileInfo.Name), true);
                    var result = _elTest.GithataYukle($"{Path}{fileInfo.Name.ToUpper()}");
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
                catch (ErrorExceptionHandller ex)
                {
                    mes.messanger(ex.Message);
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
            }

        }
        private void ElTest_Load(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == base.WindowState)
            {
                this.notifyIcon.Visible = true;
                base.Hide();
            }
            else if (base.WindowState == FormWindowState.Normal)
            {
                this.notifyIcon.Visible = false;
                this.BackColor = Color.Lime;
                base.WindowState = FormWindowState.Minimized;
            }

            WatcherStart();
            TaskBaraAl();
        }
        public void WatcherStart()
        {
            this.watcher1.EnableRaisingEvents = true;
            this.watcher2.EnableRaisingEvents = true;
            this.watcher3.EnableRaisingEvents = true;
            Watcher1(pathc[0], format);
            Watcher2(pathc[1], format);
            Watcher3(pathc[2], format);
        }
        public void WatcherStop()
        {
            this.watcher1.EnableRaisingEvents = false;
            this.watcher2.EnableRaisingEvents = false;
            this.watcher3.EnableRaisingEvents = false;
        }
        public SyBarcodeOut GitParcalama(SyBarcodeOut barcodece)
        {
            string[] oarca = barcodece.BarcodeIcerik.Split('_');
            string[] poarca = oarca[0].Split('-');
            barcodece.prefix = poarca[0];
            barcodece.family = poarca[1];
            barcodece.suffix = Regex.Replace(poarca[2], "[^a-z,A-Z,@,^,/,]", "");
            barcodece.IdDonanim = oarca[1];
            barcodece.Name = oarca[2];
            return barcodece;
        }
        private void GitSystemeDesktopKapa(string name)
        {
            FileInfo[] files = (new DirectoryInfo("C:\\kliptest_2")).GetFiles("*.txt");
            for (int i = 0; i < (int)files.Length; i++)
            {
                FileInfo fileInfo = files[i];
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                this.deger = fileInfo.Name.ToUpper();
                try
                {

                    string[] strArrays = this.deger.Split(new char[] { '\u005F' });
                    var gelenDegerler = GitParcalama(new SyBarcodeOut { BarcodeIcerik = deger });
                    TorkService = new TorkService(_repo, new UrVardiya() { Name = gelenDegerler.Name });
                    var idBak = TorkService.GitSytemeSayiBac(new SyBarcodeInput() { BarcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}" });
                    scren = new ScreenSaverForm(0, strArrays[2].ToString());
                    sicil = new SicilOkuma(strArrays[2].ToString());
                    scren.TetikSicil += Scre_TetikSicil;
                    switch (idBak.Message)
                    {
                        case "Donanim Okunmus!":
                            FormCalistir(scren, strArrays, " Donanim Test Gecmis! \n\r Lutfen Baska Donanim Okutun!", "\\Pictures\\error.png", color: Color.Red);
                            break;
                        case "Donanimi Bi oceki Istasyona Yonlendirin!":
                            FormCalistir(scren, strArrays, idBak.Message, "\\Pictures\\error.png", Color.Red);
                            break;
                        case "Donanimi ID Systemde Yok":
                            FormCalistir(scren, strArrays, idBak.Message, "\\Pictures\\error.png", Color.Red); break;
                        case "Donanimin ElTest Programi Kilitli!!!":
                            FormCalistir(scren, strArrays, idBak.Message, "\\Pictures\\error.png", Color.Red); break;

                        case "Donanimin Alerti Kilitli!!!":
                            FormCalistir(scren, strArrays, idBak.Message, "\\Pictures\\error.png", Color.Red); break;

                        case "OK":
                            FormCalistir(scren, strArrays, " Donanımı Test Alabilirsiniz!", "\\Pictures\\success.png", Color.Lime); break;
                        default:
                            FormCalistir(scren, strArrays, " TestMasasindan Gelen Veriler Tanimlanamadi! MASABAKIM ve KALITE-YE donun!", "\\Pictures\\error.png", Color.Red);
                            scren.lblMessage.ForeColor = Color.Red;
                            scren.lblMessage.Text = string.Concat(new string[] { strArrays[1], " ", strArrays[0], Environment.NewLine, " TestMasasindan Gelen Veriler Tanimlanamadi! MASABAKIM ve KALITE-YE donun!" });
                            scren.pictureBox1.Image = Image.FromFile(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\Pictures\\error.png"));
                            scren.ShowDialog(); break;
                    }
                }
                catch (Exception ex)
                {
                    File.Delete(string.Concat("C:\\kliptest_2\\", fileInfo.Name));
                }
                File.Delete(string.Concat("C:\\kliptest_2\\", fileInfo.Name));
            }
            static void FormCalistir(ScreenSaverForm screenSaverForm, string[] strArrays, string message, string path, Color color)
            {
                screenSaverForm.lblMessage.ForeColor = color;
                screenSaverForm.lblMessage.Text = string.Concat(new string[] { strArrays[1], " ", strArrays[0], Environment.NewLine, message });
                screenSaverForm.pictureBox1.Image = Image.FromFile(string.Concat(AppDomain.CurrentDomain.BaseDirectory, path));
                screenSaverForm.ShowDialog();
            }
        }
        private void Scre_TetikSicil(object? sender, EventArgs e)
        {
            sicil.ShowDialog();
        }
        public void TaskBaraAl()
        {
            if (FormWindowState.Minimized == base.WindowState)
            {
                notifyIcon.Text = "Sistem calisyot";

                notifyIcon.Visible = true;
                base.Hide();

            }
            else if (base.WindowState == FormWindowState.Normal)
            {
                notifyIcon.Visible = false;

            }
            this.BackColor = Color.Lime;
            base.WindowState = FormWindowState.Minimized;
        }
        private void ElTest_Move(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.Hide();
                notifyIcon.ShowBalloonTip(1000, "System su an calisiyor!", "Programi acmak icin ustune tiklayin!", ToolTipIcon.Info);
            }
        }
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            base.Show();
        }
        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            base.Show();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.Show();
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Green;
            base.WindowState = FormWindowState.Minimized;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            AboutBox1 frm = new AboutBox1();
            frm.ShowDialog();
        }
    }

}
