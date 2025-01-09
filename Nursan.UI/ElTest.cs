using Nursan.Business.Manager;
using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.SortedList;
using Nursan.Validations.ValidationCode;
using System.Text.RegularExpressions;
using XMLIslemi = Nursan.Core.Printing.XMLIslemi;

namespace Nursan.UI
{
    public partial class ElTest : Form
    {
        private string deger;
        private FileSystemWatcher watcher1;
        private FileSystemWatcher watcher2;
        private FileSystemWatcher watcher3;
        private FileSystemWatcher Kliptest_3;
        private Messaglama mes = new Messaglama();
        // private XMLIslemi xmlis = new XMLIslemi();
        private TorkService TorkService;
        static string[] pathc = { "C:\\Kliptest\\", "C:\\Kliptest_2\\", "C:\\Klt\\", "C:\\Kliptest_3\\", "C:\\DEMO\\", "C:\\_Kliptest\\", };
        private EltestValidasyonlari _elTest;
        private static string format = $"*.txt";
        private ScreenSaverForm scren;
        private SicilOkuma sicil;
        private UnitOfWork _repo;

        public ElTest(UnitOfWork repo)
        {
            InitializeComponent();
            GitDirektoryBac();
            this.watcher1 = new FileSystemWatcher(pathc[0], format);
            this.watcher1.Created += new FileSystemEventHandler(Watcher1_Created);
            this.watcher2 = new FileSystemWatcher(pathc[1], format);
            this.watcher2.Created += new FileSystemEventHandler(this.Watcher2_Created);
            this.watcher3 = new FileSystemWatcher(pathc[2], format);
            this.watcher3.Created += new FileSystemEventHandler(this.Watcher3_Created);
            this.Kliptest_3 = new FileSystemWatcher(pathc[3], format);
            this.Kliptest_3.Created += new FileSystemEventHandler(this.KlipTest_3_Created);
            lblVersion.Text = $"ElTest {Environment.Version}";
            _elTest = new EltestValidasyonlari(repo);
            _repo = repo;
            GitSytemdenSil();
        }

        private void Scre_TetikSicil(object? sender, EventArgs e)
        {
            sicil.ShowDialog();
        }

        private void GitSytemdenSil()
        {
            FileInfo[] files = (new DirectoryInfo("C:\\kliptest_2")).GetFiles("*.txt");
            FileInfo[] fileInfo = files;
            for (int i = 0; i < (int)files.Length; i++)
            {
                File.Delete(string.Concat("C:\\kliptest_2\\", fileInfo[i].Name));
            }
            FileInfo[] files1 = (new DirectoryInfo("C:\\Klt")).GetFiles("*.txt");
            FileInfo[] fileInfo1 = files1;
            for (int i = 0; i < (int)files1.Length; i++)
            {
                File.Delete(string.Concat("C:\\Klt\\", fileInfo1[i].Name));
            }
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

        private void Watcher1_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());
                 
                Watcher1(veri.Path, veri.Filter);
            }
            catch (ErrorExceptionHandller ex)
            {
                mes.messanger(ex.Message);
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
            catch (ErrorExceptionHandller ex)
            {
                mes.messanger(ex.Message);
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
            catch (ErrorExceptionHandller ex)
            {
                mes.messanger(ex.Message);
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
        private void KlipTest_3_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());
                KlipTest_3(veri.Path, veri.Filter);
            }
            catch (ErrorExceptionHandller ex)
            {
                mes.messanger(ex.Message);
            }
        }
      
        private static async Task DeleteFileAsync(string filePath)
        {
            await Task.Run(() => File.Delete(filePath));
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
                    File.Copy(string.Concat(Path, fileInfo.Name), string.Concat(pathc[5].ToString(), $"Start-{fileInfo.Name}"), true);
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
            for (int i = 0; i < (int)fileInfoArray.Count(); i++)
            {
                FileInfo fileInfo = fileInfoArray[i];
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                this.deger = fileInfo.Name.ToUpper();
                try
                {
                    File.Copy(string.Concat(Path, fileInfo.Name), string.Concat(pathc[5].ToString(), fileInfo.Name), true);
                    Thread.Sleep(XMLIslemi.XmlSaniye());
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

        private void KlipTest_3(string Path, string Format)
        {
            FileInfo[] files = (new DirectoryInfo(Path).GetFiles(Format));
            FileInfo[] fileInfoArray = files;
            for (int i = 0; i < (int)fileInfoArray.Count(); i++)
            {
                FileInfo fileInfo = fileInfoArray[i];
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                this.deger = fileInfo.Name.ToUpper();
                try
                {
                    File.Copy(string.Concat(Path, fileInfo.Name), string.Concat(pathc[5].ToString(), fileInfo.Name), true);
                    Thread.Sleep(XMLIslemi.XmlSaniye());
                    GitSystemeDesktopAc($"{Path}{fileInfo.Name.ToUpper()}");
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
            this.Kliptest_3.EnableRaisingEvents = true;
            //Watcher1(pathc[0], format);
            Watcher1(pathc[0], format);
            Watcher2(pathc[1], format);
            Watcher3(pathc[2], format);
            KlipTest_3(pathc[3], format);
        }

        public void WatcherStop()
        {
            this.watcher1.EnableRaisingEvents = false;
            this.watcher2.EnableRaisingEvents = false;
            this.watcher3.EnableRaisingEvents = false;
            this.Kliptest_3.EnableRaisingEvents = false;
        }

        public SyBarcodeOut GitParcalama(SyBarcodeOut barcodece)
        {
            var res = StringSpanConverter.SplitWithoutAllocationReturnArray(barcodece.BarcodeIcerik.AsSpan(), '_');
            var idres = StringSpanConverter.GetCharsIsDigit(res[1]);
            string[] poarca = StringSpanConverter.SplitWithoutAllocationReturnArray(res[0].AsSpan(), '-');
            barcodece.prefix = poarca[0];
            barcodece.family = poarca[1];
            barcodece.suffix = Regex.Replace(poarca[2], "[^a-z,A-Z,@,^,/,]", "");
            barcodece.IdDonanim = res[1];
            barcodece.Name = res[2];
            return barcodece;
        }

        private void GitSystemeDesktopAc(string name)
        {
            FileInfo[] files = (new DirectoryInfo("C:\\kliptest_3")).GetFiles("*.txt");
            for (int i = 0; i < (int)files.Length; i++)
            {
                FileInfo fileInfo = files[i];
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                this.deger = fileInfo.Name.ToUpper();
                string[] strArrays = this.deger.Split(new char[] { '\u005F' });
                var gelenDegerler = GitParcalama(new SyBarcodeOut { BarcodeIcerik = deger });
                TorkService = new TorkService(_repo, new UrVardiya() { Name = gelenDegerler.Name });
                var idBak = TorkService.GitSytemeSayiElTestBack(new SyBarcodeInput() { BarcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}" });
                scren = new ScreenSaverForm(0, strArrays[2].ToString());
                sicil = new SicilOkuma(strArrays[2].ToString());
                scren.TetikSicil += Scre_TetikSicil;
                try
                {
                    switch (idBak.Message)
                    {
                        case "Donanimi":
                            FormCalistir(scren, strArrays, " Donanimi Bi oceki Istasyona Yonlendirin!", "\\Pictures\\error.png", color: Color.Red);
                            break;

                        case "Donanimi Bi oceki Istasyona Yonlendirin!":
                            FormCalistir(scren, strArrays, idBak.Message, "\\Pictures\\error.png", Color.Red);
                            break;

                        case "Donanimi ID Systemde Yok":
                            FormCalistir(scren, strArrays, idBak.Message, "\\Pictures\\error.png", Color.Red); break;
                        case "OK":
                            FormCalistir(scren, strArrays, " Donanımı Kanal Takma Istasyonundaa Islem Yapabilirsiniz!", "\\Pictures\\success.png", Color.Lime); break;
                        default:
                            FormCalistir(scren, strArrays, " TestMasasindan Gelen Veriler Tanimlanamadi! MASABAKIM ve KALITE-YE donun!", "\\Pictures\\error.png", Color.Red);
                            scren.lblMessage.ForeColor = Color.Red;
                            scren.lblMessage.Text = string.Concat(new string[] { strArrays[1], " ", strArrays[0], Environment.NewLine, " TestMasasindan Gelen Veriler Tanimlanamadi! MASABAKIM ve KALITE-YE donun!" });
                            scren.pictureBox1.Image = Image.FromFile(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\Pictures\\error.png"));
                            // screenSaverForm.ShowDialog();
                            break;
                    }
                }
                catch (ErrorExceptionHandller ex)
                {
                    File.Delete(string.Concat("C:\\kliptest_3\\", fileInfo.Name));
                    scren.Dispose();
                    sicil.Dispose();
                }
                File.Delete(string.Concat("C:\\kliptest_3\\", fileInfo.Name));
                scren.Dispose();
                sicil.Dispose();
            }


        }
        private void FormCalistir(ScreenSaverForm screenSaverForm, string[] strArrays, string message, string path, Color color)
        {
            screenSaverForm.lblMessage.ForeColor = color;
            screenSaverForm.lblMessage.Text = string.Concat(new string[] { strArrays[1], " ", strArrays[0], Environment.NewLine, message });
            screenSaverForm.pictureBox1.Image = Image.FromFile(string.Concat(AppDomain.CurrentDomain.BaseDirectory, path));
            screenSaverForm.ShowDialog();
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
                catch (ErrorExceptionHandller ex)
                {
                    File.Delete(string.Concat("C:\\kliptest_2\\", fileInfo.Name));
                    scren.Dispose();
                    sicil.Dispose();
                }
                File.Delete(string.Concat("C:\\kliptest_2\\", fileInfo.Name));
                scren.Dispose();
                sicil.Dispose();
            }
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