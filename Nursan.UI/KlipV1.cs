using Nursan.Business.Manager;
using Nursan.Business.Services;
using Nursan.Domain.AmbarModels;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Logging.Messages;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.SortedList;
using Nursan.Validations.ValidationCode;
using Nursan.XMLTools;
using SQLitePCL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using XMLIslemi = Nursan.Core.Printing.XMLIslemi;

namespace Nursan.UI
{
    public partial class KlipV1 : Form
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
        private ScreenMonitor _screenMonitor;
        private GroupBox groupBoxScreenMonitor;
        private TextBox textBoxTextToWatch;
        private Button buttonAddTextToWatch;
        private Button buttonStartMonitoring;
        private Button buttonStopMonitoring;
        private Label labelStatus;
        private ComboBox comboBoxAction;
        private System.Windows.Forms.Timer expandTimer;
        private int targetWidth = 800;
        private int targetHeight = 200;
        private int expandStep = 20; // колко пиксела да се увеличава на стъпка
        private List<Button> dynamicTicketButtons = new List<Button>();
        string _vardiya;
        private bool isExpanded = false; // Добавяме променлива за проследяване на състоянието
        private string lastScreenshotPath = null;
        private readonly SystemTicket _systemTicket;
        public KlipV1(UnitOfWork repo)
        {
            InitializeComponent();

            // Настройки за формата
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.ShowInTaskbar = true;
            this.BackColor = Color.WhiteSmoke;
            this.TransparencyKey = Color.WhiteSmoke;
            _systemTicket = new SystemTicket();
            // Разпъни формата по цялата ширина на екрана и височина 300px
            this.Left = 0;
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = 300;

            // Настройваме бутона Ариза
            btnAriza.FlatStyle = FlatStyle.Flat;
            btnAriza.FlatAppearance.BorderSize = 0;
            btnAriza.BackColor = Color.WhiteSmoke;
            btnAriza.ForeColor = Color.Red;
            btnAriza.Text = "Ariza";

            // Настройваме лейбъла
            lblCountProductions.ForeColor = Color.Red;
            lblCountProductions.BackColor = Color.WhiteSmoke;
            lblCountProductions.Font = new Font(lblCountProductions.Font.FontFamily, 16, FontStyle.Bold);
            lblCountProductions.Text = "0";

            GitDirektoryBac();
            this.watcher1 = new FileSystemWatcher(pathc[0], format);
            this.watcher1.Created += new FileSystemEventHandler(Watcher1_Created);
            this.watcher2 = new FileSystemWatcher(pathc[1], format);
            this.watcher2.Created += new FileSystemEventHandler(this.Watcher2_Created);
            this.watcher3 = new FileSystemWatcher(pathc[2], format);
            this.watcher3.Created += new FileSystemEventHandler(this.Watcher3_Created);
            this.Kliptest_3 = new FileSystemWatcher(pathc[3], format);
            this.Kliptest_3.Created += new FileSystemEventHandler(this.KlipTest_3_Created);
            lblVersion.Text = $"KlipTest {Environment.Version}";
            _elTest = new EltestValidasyonlari(repo);
            _repo = repo;
            GitSytemdenSil();

            // Инициализация на ScreenMonitor
            InitializeScreenMonitor();

            expandTimer = new System.Windows.Forms.Timer();
            expandTimer.Interval = 10;
            expandTimer.Tick += ExpandTimer_Tick;

            btnAriza.Click += btnAriza_Click;
            GetEltestountActivDeactiv();
        }

      private void GetEltestountActivDeactiv()
        {
           if(XMLSeverIp.ElTestCount())
            {
           
                lblCountProductions.Enabled= true;
                lblCountProductions.Visible = true;
            }
            else
            {
                lblCountProductions.Enabled = false;
                lblCountProductions.Visible = false;
            }
        }

        private void btnAriza_Click(object sender, EventArgs e)
        {
            if (!isExpanded)
            {
                // Първо зареждаме бутоните
                LoadTicketButtons();

                // Показваме всички контроли
                foreach (Control control in this.Controls)
                {
                    control.Visible = true;
                }

                // Запазваме прозрачността
                this.BackColor = Color.WhiteSmoke;
                this.TransparencyKey = Color.WhiteSmoke;

                // Увеличаваме размера на формата
                this.Width = Screen.PrimaryScreen.WorkingArea.Width;
                this.Height = 300;

                isExpanded = true;
            }
            else
            {
                // Първо премахваме динамичните бутони
                foreach (var btn in dynamicTicketButtons)
                {
                    this.Controls.Remove(btn);
                    btn.Dispose();
                }
                dynamicTicketButtons.Clear();

                // Скриваме всички контроли освен бутона Ариза и лейбъла
                foreach (Control control in this.Controls)
                {
                    if (control != btnAriza && control != lblCountProductions)
                    {
                        control.Visible = false;
                    }
                }

                // Върни формата в малък и прозрачен режим
                this.TransparencyKey = Color.WhiteSmoke;
                this.BackColor = Color.WhiteSmoke;
                
                // Размерът на формата - както беше в началото
                int formWidth = lblCountProductions.Right + 5;
                int formHeight = Math.Max(btnAriza.Height, lblCountProductions.Height) + 10;
                //this.Size = new Size(formWidth, formHeight);

                isExpanded = false;
            }
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
            // Премахваме минимизирането
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
               // scren.Owner = this;
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
                    //scren.Owner = this;
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
            notifyIcon.Text = "Sistem calisyot";
            notifyIcon.Visible = true;
        }

        private void ElTest_Move(object sender, EventArgs e)
        {
            // Предотвратяваме минимизирането
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.WindowState = FormWindowState.Normal;
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
            // Премахваме минимизирането
            this.BackColor = Color.Green;
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

        // Метод за инициализация на ScreenMonitor и UI елементите за него
        private void InitializeScreenMonitor()
        {
            //try
            //{
            //    // Създаваме контролите за ScreenMonitor
            //    groupBoxScreenMonitor = new GroupBox();
            //    groupBoxScreenMonitor.Text = "Автоматично наблюдение на екрана";
            //    groupBoxScreenMonitor.Location = new System.Drawing.Point(12, 300);
            //    groupBoxScreenMonitor.Size = new System.Drawing.Size(350, 200);
            //    groupBoxScreenMonitor.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

            //    // Текстово поле за въвеждане на текст за наблюдение
            //    textBoxTextToWatch = new TextBox();
            //    textBoxTextToWatch.Location = new System.Drawing.Point(10, 30);
            //    textBoxTextToWatch.Size = new System.Drawing.Size(200, 25);
            //    textBoxTextToWatch.PlaceholderText = "Текст за наблюдение...";

            //    // Падащо меню с действия
            //    comboBoxAction = new ComboBox();
            //    comboBoxAction.Location = new System.Drawing.Point(10, 60);
            //    comboBoxAction.Size = new System.Drawing.Size(200, 25);
            //    comboBoxAction.DropDownStyle = ComboBoxStyle.DropDownList;
            //    comboBoxAction.Items.AddRange(new object[] { 
            //        "Сигнализирай", 
            //        "Отвори ElTest", 
            //        "Затвори ElTest", 
            //        "Изчисти файлове" 
            //    });
            //    comboBoxAction.SelectedIndex = 0;

            //    // Бутон за добавяне на текст за наблюдение
            //    buttonAddTextToWatch = new Button();
            //    buttonAddTextToWatch.Text = "Добави за наблюдение";
            //    buttonAddTextToWatch.Location = new System.Drawing.Point(220, 30);
            //    buttonAddTextToWatch.Size = new System.Drawing.Size(120, 25);
            //    buttonAddTextToWatch.Click += ButtonAddTextToWatch_Click;

            //    // Бутони за стартиране/спиране на наблюдението
            //    buttonStartMonitoring = new Button();
            //    buttonStartMonitoring.Text = "Стартирай";
            //    buttonStartMonitoring.Location = new System.Drawing.Point(10, 100);
            //    buttonStartMonitoring.Size = new System.Drawing.Size(90, 30);
            //    buttonStartMonitoring.Click += ButtonStartMonitoring_Click;

            //    buttonStopMonitoring = new Button();
            //    buttonStopMonitoring.Text = "Спри";
            //    buttonStopMonitoring.Location = new System.Drawing.Point(110, 100);
            //    buttonStopMonitoring.Size = new System.Drawing.Size(90, 30);
            //    buttonStopMonitoring.Enabled = false;
            //    buttonStopMonitoring.Click += ButtonStopMonitoring_Click;

            //    // Статус лейбъл
            //    labelStatus = new Label();
            //    labelStatus.Text = "Статус: Неактивно";
            //    labelStatus.Location = new System.Drawing.Point(10, 150);
            //    labelStatus.AutoSize = true;

            //    // Добавяме контролите към GroupBox
            //    groupBoxScreenMonitor.Controls.Add(textBoxTextToWatch);
            //    groupBoxScreenMonitor.Controls.Add(comboBoxAction);
            //    groupBoxScreenMonitor.Controls.Add(buttonAddTextToWatch);
            //    groupBoxScreenMonitor.Controls.Add(buttonStartMonitoring);
            //    groupBoxScreenMonitor.Controls.Add(buttonStopMonitoring);
            //    groupBoxScreenMonitor.Controls.Add(labelStatus);

            //    // Добавяме GroupBox към формата
            //    this.Controls.Add(groupBoxScreenMonitor);

            //    // Инициализираме ScreenMonitor
            //    _screenMonitor = new ScreenMonitor();
            //    _screenMonitor.TextDetected += ScreenMonitor_TextDetected;
            //}
            //catch (Exception ex)
            //{
            //    mes.messanger($"Грешка при инициализация на ScreenMonitor: {ex.Message}");
            //}
        }

        // Обработка на събитието TextDetected
        private void ScreenMonitor_TextDetected(object sender, TextDetectedEventArgs e)
        {
            try
            {
                // Изпълняваме на UI нишката
                this.Invoke((MethodInvoker)delegate
                {
                    mes.messanger($"Намерен текст: {e.DetectedText}");
                });
            }
            catch (Exception ex)
            {
                mes.messanger($"Грешка при обработка на открит текст: {ex.Message}");
            }
        }

        // Добавяне на текст за наблюдение
        private void ButtonAddTextToWatch_Click(object sender, EventArgs e)
        {
            string textToWatch = textBoxTextToWatch.Text.Trim();
            if (string.IsNullOrEmpty(textToWatch))
            {
                mes.messanger("Моля, въведете текст за наблюдение.");
                return;
            }

            try
            {
                // Действие, което да се изпълни при откриване на текста
                Action action = null;
                switch (comboBoxAction.SelectedIndex)
                {
                    case 0: // Сигнализирай
                        action = () => this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show($"Открит е текст: {textToWatch}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        });
                        break;
                    case 1: // Отвори ElTest
                        action = () => this.Invoke((MethodInvoker)delegate
                        {
                            if (this.WindowState == FormWindowState.Minimized)
                            {
                                this.WindowState = FormWindowState.Normal;
                            }
                            this.BringToFront();
                        });
                        break;
                    case 2: // Затвори ElTest
                        action = () => this.Invoke((MethodInvoker)delegate
                        {
                            this.WindowState = FormWindowState.Minimized;
                        });
                        break;
                    case 3: // Изчисти файлове
                        action = () => this.Invoke((MethodInvoker)delegate
                        {
                            GitSytemdenSil();
                        });
                        break;
                }

                _screenMonitor.AddTextToWatch(textToWatch, action);
                mes.messanger($"Добавен текст за наблюдение: {textToWatch}");
                textBoxTextToWatch.Clear();
            }
            catch (Exception ex)
            {
                mes.messanger($"Грешка при добавяне на текст за наблюдение: {ex.Message}");
            }
        }

        // Стартиране на наблюдението
        private void ButtonStartMonitoring_Click(object sender, EventArgs e)
        {
            try
            {
                _screenMonitor.StartMonitoring();
                buttonStartMonitoring.Enabled = false;
                buttonStopMonitoring.Enabled = true;
                labelStatus.Text = "Статус: Активно";
                mes.messanger("Наблюдението на екрана е стартирано.");
            }
            catch (Exception ex)
            {
                mes.messanger($"Грешка при стартиране на наблюдението: {ex.Message}");
            }
        }

        // Спиране на наблюдението
        private void ButtonStopMonitoring_Click(object sender, EventArgs e)
        {
            try
            {
                _screenMonitor.StopMonitoring();
                buttonStartMonitoring.Enabled = true;
                buttonStopMonitoring.Enabled = false;
                labelStatus.Text = "Статус: Неактивно";
                mes.messanger("Наблюдението на екрана е спряно.");
            }
            catch (Exception ex)
            {
                mes.messanger($"Грешка при спиране на наблюдението: {ex.Message}");
            }
        }

        // Освобождаване на ресурсите при затваряне
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Освобождаваме ресурсите на ScreenMonitor
            _screenMonitor?.Dispose();
        }

        private void ExpandTimer_Tick(object sender, EventArgs e)
        {
            int newWidth = this.Width + expandStep;
            int newHeight = this.Height + expandStep;

            if (newWidth >= targetWidth) newWidth = targetWidth;
            if (newHeight >= targetHeight) newHeight = targetHeight;

            //this.Size = new Size(newWidth, newHeight);

            if (this.Width >= targetWidth && this.Height >= targetHeight)
            {
                expandTimer.Stop();
                this.TransparencyKey = Color.Empty;
                this.BackColor = SystemColors.Control;
                foreach (Control control in this.Controls)
                {
                    control.Visible = true;
                }
                LoadTicketButtons();
            }
        }
        public void AddTicket(string tiketName, string description, int role)
        {
            decimal? pcId = _elTest.GetPcId();
            using (var context = new AmbarContext())
            {
                // Ограничаваме дължината на tiketName до 50 символа за да избегнем SQL truncation error
                string truncatedTiketName = tiketName?.Length > 50 ? tiketName.Substring(0, 50) : tiketName;
                
                var islemler = new Islemler
                {
                    Ariza = truncatedTiketName,
                    // Islem = description,
                    Tarih = DateTime.Now,
                    Role = role,
                    PcId = pcId,
                    Active = true

                    // ... други полета по желание
                };
                context.Islemlers.Add(islemler);
                context.SaveChanges();
            }
        }

        private void LoadTicketButtons()
        {
            try
            {
                // Премахни стари бутони, ако има
                foreach (var btn in dynamicTicketButtons)
                {
                    this.Controls.Remove(btn);
                    btn.Dispose();
                }
                dynamicTicketButtons.Clear();

                int btnWidth = 200;
                int btnHeight = 40;
                int marginX = 10;
                int marginY = 10;
                int startY = btnAriza.Bottom + 20;

                var tickets = _repo.GetRepository<SyTicketName>().GetAll(null);
                if (tickets == null || !tickets.Data.Any())
                {
                    MessageBox.Show("Няма налични билети за зареждане.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int totalButtons = tickets.Data.Count();
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int maxColumns = Math.Max(1, screenWidth / (btnWidth + marginX));
                int buttonsPerRow = Math.Min(maxColumns, Math.Max(1, (int)Math.Ceiling(Math.Sqrt(totalButtons))));

                int count = 0;
                foreach (var ticket in tickets.Data)
                {
                    int row = count / buttonsPerRow;
                    int col = count % buttonsPerRow;

                    Button btn = new Button();
                    btn.Text = ticket.TiketName;
                    btn.Width = btnWidth;
                    btn.Height = btnHeight;
                    btn.Left = 30 + col * (btnWidth + marginX);
                    btn.Top = startY + row * (btnHeight + marginY);
                    
                    // Модерен дизайн на бутона
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.BackColor = Color.FromArgb(45, 45, 48);
                    btn.ForeColor = Color.Red;
                    btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                    btn.Cursor = Cursors.Hand;
                    
                    // Добавяме hover ефект
                    btn.MouseEnter += (s, e) => {
                        Button b = s as Button;
                        b.BackColor = Color.FromArgb(0, 122, 204);
                        b.ForeColor = Color.White;
                    };
                    btn.MouseLeave += (s, e) => {
                        Button b = s as Button;
                        b.BackColor = Color.FromArgb(45, 45, 48);
                        b.ForeColor = Color.Red;
                    };

                    btn.Tag = ticket;
                    btn.Click += TicketButton_Click;

                    this.Controls.Add(btn);
                    dynamicTicketButtons.Add(btn);

                    count++;
                }
              
            }
            catch (Exception ex)
            {
                mes.messanger($"Грешка при зареждане на бутоните: {ex.Message}");
            }
        }

        private void TicketButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var ticket = btn.Tag as SyTicketName;
            if (ticket != null)
            {
                if (XMLSeverIp.WebApiTrue())
                {
                    Console.WriteLine("=== WebAPI е активно, стартираме изпращане ===");
                    SendTicketWithScreenshot();
                    Console.WriteLine($"lastScreenshotPath след SendTicketWithScreenshot: {lastScreenshotPath}");
                    
                    // Използваме Role параметъра от тикета
                    int roleValue = ticket.Role ?? 5; // Ако Role е null, използваме 5 като default
                    ShowQrCodeAfterTicketCreation(ticket.TiketName, ticket.Description, lastScreenshotPath, roleValue);
                }
                else
                {
                    Console.WriteLine("=== WebAPI не е активно, използваме локално запазване ===");
                    // Добавяме проверка за nullable Role
                    int roleValue = ticket.Role ?? 5; // Ако Role е null, използваме 5 като default
                    AddTicket(ticket.TiketName, ticket.Description, roleValue);
                }
                // Първо премахваме динамичните бутони
                foreach (var b in dynamicTicketButtons)
                {
                    this.Controls.Remove(b);
                    b.Dispose();
                }
                dynamicTicketButtons.Clear();

                // Скриваме всички контроли освен бутона Ариза и лейбъла
                foreach (Control control in this.Controls)
                {
                    if (control != btnAriza && control != lblCountProductions)
                    {
                        control.Visible = false;
                    }
                }

                // Върни формата в малък и прозрачен режим
                this.TransparencyKey = Color.WhiteSmoke;
                this.BackColor = Color.WhiteSmoke;
                
                // Размерът на формата - както беше в началото
                int formWidth = lblCountProductions.Right + 5;
                int formHeight = Math.Max(btnAriza.Height, lblCountProductions.Height) + 10;
               // this.Size = new Size(formWidth, formHeight);
                
                isExpanded = false;
            }
        }
        private void SendTicketWithScreenshot()
        {
            try
            {
                // 1. Правим скрийншот
                var bounds = Screen.PrimaryScreen.Bounds;
                using (var bmp = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
                    }
                    
                    // Създаваме пълен път към файла
                    string fileName = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                    
                    bmp.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
                    lastScreenshotPath = fullPath;
                    
                    Console.WriteLine($"Скрийншот запазен в: {lastScreenshotPath}");
                    Console.WriteLine($"Файлът съществува: {File.Exists(lastScreenshotPath)}");
                }
                // 2. Пращаме тикета (логиката е същата като в ButtonCreateTicket_Click)
               //CreateTicket();
                // MessageBox.Show("Тикетът и скрийншотът са изпратени автоматично!", "IT тикет", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при SendTicketWithScreenshot: {ex.Message}");
                // labelStatus може да не е инициализиран
                if (labelStatus != null)
                {
                    labelStatus.Text = $"Грешка при автоматично изпращане: {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Асинхронно създава тикет и показва QR код за проследяване
        /// </summary>
        /// <param name="tiketName">Име на тикета</param>
        /// <param name="description">Описание на тикета</param>
        /// <param name="lastScreenshotPath">Път към скрийншота</param>
        /// <param name="roleValue">Роля на потребителя</param>
        private async void ShowQrCodeAfterTicketCreation(string tiketName, string description, string lastScreenshotPath, int roleValue)
        {
            try
            {
                Console.WriteLine("=== ShowQrCodeAfterTicketCreation стартира ===");
                Console.WriteLine($"tiketName: {tiketName}");
                Console.WriteLine($"description: {description}");
                Console.WriteLine($"lastScreenshotPath: {lastScreenshotPath}");
                Console.WriteLine($"roleValue: {roleValue}");
                
                // Първо пращаме тикета
                Console.WriteLine("Стартиране на CreateTicket...");
                Console.WriteLine($"_systemTicket е null: {_systemTicket == null}");
                var (success, serverTicketId) = await _systemTicket.CreateTicket(tiketName, description, lastScreenshotPath, roleValue);
                Console.WriteLine($"CreateTicket резултат: {success}");
                Console.WriteLine($"Server Ticket ID: {serverTicketId}");
                
                if (success)
                {
                    Console.WriteLine("Тикетът е изпратен успешно! Показваме QR кода...");
                    
                    // Използваме истинския ticket ID от сървъра
                    Console.WriteLine($"Използваме server ticket ID: {serverTicketId}");
                    
                    // Показваме QR кода САМО ако тикетът е изпратен успешно
                    string serverIp = XMLSeverIp.XmlWebApiIP();
                    Console.WriteLine($"Server IP: {serverIp}");
                    
                    QrTicketForm qrForm = new QrTicketForm(serverTicketId, serverIp);
                    qrForm.Show();
                    Console.WriteLine("QR форма показана");
                }
                else
                {
                    Console.WriteLine("Грешка при изпращане на тикет към сървъра - QR кода няма да се показва");
                    MessageBox.Show("Грешка при изпращане на тикета!", "Грешка", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка в ShowQrCodeAfterTicketCreation: {ex.Message}");
                MessageBox.Show($"Грешка при създаване на тикет: {ex.Message}", "Грешка", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
    }
}
