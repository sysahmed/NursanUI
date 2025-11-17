using Nursan.Business.Logging;
using Nursan.Business.Services;
using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using Nursan.UI.Library;
using Nursan.UI.OzelClasslar;
using Nursan.Validations.SortedList;
using Nursan.Validations.ValidationCode;
using System.Drawing.Imaging;
using System.IO.Ports;

namespace Nursan.UI
{
    public partial class GozKontrol : Form
    {
        private static List<SyBarcodeOut> _syBarcodeOutList;
        private static List<SyPrinter> _syPrinterList;
        private static List<OrFamily> _familyList;
        private static List<UrModulerYapi> _modulerYapiList;
        SyBarcodeInput BarcodeInput = new SyBarcodeInput();
        SerialPort _serialPort;
        BarcodeValidation barcode;
        string pfbSerial;
        int brtSayi = 0;
        private static UnitOfWork _repo;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        CountDegerValidations _countDegerValidations;
        int pi; string barkodGk;
        static int brcodeVCount;
        Proveri proveri = new Proveri();
        //List<SyBarcodeInput> Barcode = new List<SyBarcodeInput>();
        TorkService tork;
        private string lastScreenshotPath = "";
        
        // Ticket система полета (като в ElTest)
        private List<Button> dynamicTicketButtons = new List<Button>();
        private bool isTicketExpanded = false;
        private readonly SystemTicket _systemTicket;
        private Button btnAriza;
        private readonly StructuredLogger ticketLogger;
        
        public GozKontrol(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
        {
            // Добавяне на global exception handler за формата
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

             brtSayi = brcodeVCount;
             _modulerYapiList = modulerYapiList;
             _syBarcodeOutList = syBarcodeOutList;
             _syPrinterList = syPrinterList;
             _familyList = familyList;
            _repo = repo;
            _makine = makine;
            _vardiya = vardiya;
            _istasyonList = istasyonList;
            _syBarcodeInputList = syBarcodeInputList;

            brcodeVCount = _syBarcodeInputList.Count;
            _countDegerValidations = new CountDegerValidations(_repo, _makine, _vardiya, _istasyonList);
            Form.CheckForIllegalCrossThreadCalls = false;
            tork = new TorkService(repo, vardiya);
            _systemTicket = new SystemTicket();
            ticketLogger = new StructuredLogger(nameof(GozKontrol));
            InitializeComponent();
            
            // Създаване на ARIZA бутон
            CreateArizaButton();
            
            // Добавяне на тестов KeyDown handler за crash тест (Ctrl+Shift+F12)
            this.KeyDown += GozKontrol_KeyDown;
            this.KeyPreview = true; // Важно за да улавя клавишни комбинации
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                GetCounts();
                StaringAP frm = new StaringAP();
                Thread.Sleep(500);
                frm.Dispose();
                frm.Close();
            }
            catch (Exception ex)
            {
                HandleException(ex, "Грешка при зареждане на формата GozKontrol");
            }
        }
        
        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (_vardiya.Name != txtBarcode.Text)
                    {
                        if (txtBarcode.Text.StartsWith("#"))
                            barkodGk = txtBarcode.Text.Substring(1);
                        else
                            barkodGk = txtBarcode.Text;

                        if (tork.IsAlertGkLocked(barkodGk))
                        {
                            // Вземи харнес модела за формата
                            string harnessName;
                            try
                            {
                                harnessName = StringSpanConverter.ExtractText(barkodGk.AsSpan()).ToString();
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                proveri.MessageAyarla($"Невалиден баркод: '{barkodGk}'", Color.Red, lblMessage);
                                txtBarcode.Clear();
                                return;
                            }
                        var harnessModel = _repo.GetRepository<OrHarnessModel>().Get(x => x.HarnessModelName == harnessName).Data;
                        using (var dlg = new AlertGkLockedOpen(_repo, harnessModel))
                        {

                            if (dlg.ShowDialog() != DialogResult.OK)
                            {
                                lblMessage.Text= "GK Locked не е отключена!";
                                lblMessage.ForeColor = Color.Red;
                                //proveri.MessageAyarla($"GK Locked не е отключена!", Color.Red, lblMessage);
                                //txtBarcode.Clear();
                                return;
                            }
                            else
                            {
                                lblMessage.Text = "GK Locked е отключена!";
                                lblMessage.ForeColor = Color.Lime;
                                //proveri.MessageAyarla($"GK Locked е отключена!", Color.Lime, lblMessage);
                                //listBox1.Items.Clear();
                                //txtBarcode.Clear();
                                return;
                            }
                        }
                    }

                    listBox1.Items.Add(txtBarcode.Text);
                    _syBarcodeInputList[pi].BarcodeIcerik = txtBarcode.Text;
                    if (!txtBarcode.Text.StartsWith(_syBarcodeInputList[pi].OzelChar == null ? "" : _syBarcodeInputList[pi].OzelChar))
                    {
                        proveri.MessageAyarla($"Yanlis Brcode Okudunuz!", Color.Red, lblMessage);
                        txtBarcode.Clear(); listBox1.Items.Clear(); pi = 0;
                        return;
                    }
                    listBox1.Items.Add(txtBarcode.Text);
                    _syBarcodeInputList[pi].BarcodeIcerik = txtBarcode.Text;
                    if (!txtBarcode.Text.StartsWith(_syBarcodeInputList[pi].OzelChar == null ? "" : _syBarcodeInputList[pi].OzelChar))
                    {
                        proveri.MessageAyarla($"Yanlis Brcode Okudunuz!", Color.Red, lblMessage);
                        txtBarcode.Clear(); listBox1.Items.Clear(); pi = 0;
                        return;
                    }
                    pi++;
                    if (_syBarcodeInputList.Count == pi)
                    {
                        var veri = tork.GetTorkDonanimBarcode(_syBarcodeInputList);
                        listBox1.Items.Clear();
                        for (int i = 0; i < pi; i++)
                        {
                            _syBarcodeInputList[i].BarcodeIcerik = null;
                            pi = 0;
                        }
                        proveri.MessageAyarla($"{veri.Message} {txtBarcode.Text} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage);
                    }
                    //proveri.MessageAyarla($"{veri.Message} {txtBarcode.Text} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage);
                    txtBarcode.Clear();
                    GetCounts();
                }
                else
                {
                    SicilOkumaAP sicil = new SicilOkumaAP(_repo, txtBarcode.Text);
                    sicil.ShowDialog(); this.Hide();
                    //Thread.Sleep(10000);
                    this.Dispose();
                    this.Close();

                }
            }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Грешка при обработка на баркод в GozKontrol");
            }
        }
        
        int ortalamaCount;
        int vardiyaCount;
        int toplamCount;
        private void GetCounts()
        {

            Label[] lable;
            lable = new Label[9];
            lable[0] = label1;
            lable[1] = lblVardiya; lable[2] = lblToplama; lable[3] = lblOrtalama; lable[4] = label3; lable[5] = label5; lable[6] = label7; lable[7] = label8; lable[8] = label9;

            if (Domain.SystemClass.XMLSeverIp.SayiGoster())
            {
                foreach (Label l in lable)
                {
                    if (l != null)
                        SayiGoster.LabellerVisibleYap(l);
                }
                _countDegerValidations.Hesapla(out ortalamaCount, out vardiyaCount, out toplamCount);
                lblOrtalama.Text = ortalamaCount.ToString();
                lblToplama.Text = toplamCount.ToString();
                lblVardiya.Text = vardiyaCount.ToString();
            }
            else
            {
                foreach (Label l in lable)
                {
                    if (l != null)
                        SayiGoster.LabellerNonVisibleYap(l);
                }
            }

        }

        #region Автоматична система за тикети при crash

        /// <summary>
        /// Тестов KeyDown handler за симулация на crash (Ctrl+Shift+F12)
        /// </summary>
        private void GozKontrol_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl+Shift+F12 = Тестов crash
            if (e.Control && e.Shift && e.KeyCode == Keys.F12)
            {
                Console.WriteLine("🧪 ТЕСТОВ CRASH АКТИВИРАН! Симулиране на грешка...");
                
                // Симулираме различни видове грешки за тест
                try
                {
                    // Вариант 1: NullReferenceException
                    string testString = null;
                    int length = testString.Length; // Това ще предизвика NullReferenceException
                }
                catch (Exception ex)
                {
                    // Пускаме грешката за да я улови нашия handler
                    throw new Exception("ТЕСТОВА ГРЕШКА: Симулиран crash от GozKontrol формата за тестване на автоматичната система за тикети", ex);
                }
            }
        }

        /// <summary>
        /// Global exception handler за Thread exceptions
        /// </summary>
        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception, "Thread Exception в GozKontrol");
        }

        /// <summary>
        /// Global exception handler за Unhandled exceptions
        /// </summary>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "Unhandled Exception в GozKontrol");
            }
        }

        /// <summary>
        /// Централен метод за обработка на грешки
        /// </summary>
        private void HandleException(Exception ex, string context)
        {
            try
            {
                // Прави screenshot на формата
                string screenshotPath = TakeScreenshot();

                // Създава детайлно съобщение за грешката
                string errorDetails = $@"
ГРЕШКА В ФОРМА: GozKontrol
КОНТЕКСТ: {context}
ДАТА/ЧАС: {DateTime.Now:dd.MM.yyyy HH:mm:ss}
МАШИНА: {Environment.MachineName}
ПОТРЕБИТЕЛ: {Environment.UserName}
ВАРДИЯ: {_vardiya?.Name ?? "Неизвестна"}

СЪОБЩЕНИЕ ЗА ГРЕШКА:
{ex.Message}

STACK TRACE:
{ex.StackTrace}

ВЪТРЕШНА ГРЕШКА:
{ex.InnerException?.Message ?? "Няма"}
{ex.InnerException?.StackTrace ?? ""}
";

                Dictionary<string, string> autoContext = new Dictionary<string, string>
                {
                    { "Context", SensitiveDataMasker.MaskValue(context) },
                    { "ScreenshotName", SensitiveDataMasker.MaskPath(screenshotPath) }
                };
                ticketLogger.LogError("AutoTicketTriggered", autoContext);

                Task.Run(async () =>
                {
                    await SendAutoTicketToIT(
                        $"AUTO CRASH: GozKontrol - {context}",
                        errorDetails,
                        screenshotPath,
                        1);
                });
            }
            catch (Exception ticketEx)
            {
                Dictionary<string, string> exceptionContext = new Dictionary<string, string>
                {
                    { "Message", ticketEx.Message }
                };
                ticketLogger.LogError("AutoTicketFailure", exceptionContext);
            }
        }

        /// <summary>
        /// Прави screenshot на целия екран
        /// </summary>
        private string TakeScreenshot()
        {
            try
            {
                // Създава LOGS папка ако не съществува
                string logsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LOGS");
                if (!Directory.Exists(logsFolder))
                {
                    Directory.CreateDirectory(logsFolder);
                }

                // Генерира уникално име за screenshot файла
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string filename = $"CRASH_GozKontrol_{timestamp}.jpg";
                string filepath = Path.Combine(logsFolder, filename);

                // Прави screenshot на целия екран
                Rectangle bounds = Screen.PrimaryScreen.Bounds;
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }

                    // Записва като JPEG
                    bitmap.Save(filepath, ImageFormat.Jpeg);
                }

                lastScreenshotPath = filepath;
                Dictionary<string, string> screenshotContext = new Dictionary<string, string>
                {
                    { "ScreenshotName", SensitiveDataMasker.MaskPath(filepath) }
                };
                ticketLogger.LogInfo("ScreenshotCreated", screenshotContext);
                
                return filepath;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorContext = new Dictionary<string, string>
                {
                    { "Message", ex.Message }
                };
                ticketLogger.LogError("ScreenshotFailure", errorContext);
                return string.Empty;
            }
        }

        /// <summary>
        /// Изпраща автоматичен тикет към IT системата
        /// </summary>
        private async Task<bool> SendAutoTicketToIT(string tiketName, string description, string screenshotPath, int role)
        {
            try
            {
                string bolge = _makine?.MasineName ?? "GozKontrol";
                Dictionary<string, string> startContext = new Dictionary<string, string>
                {
                    { "TicketName", SensitiveDataMasker.MaskValue(tiketName) },
                    { "Bolge", SensitiveDataMasker.MaskValue(bolge) },
                    { "ScreenshotName", SensitiveDataMasker.MaskPath(screenshotPath) },
                    { "Role", role.ToString() }
                };
                ticketLogger.LogInfo("AutoTicketSendStart", startContext);

                SystemTicket ticketService = new SystemTicket();
                (bool success, string ticketId) = await ticketService.CreateTicket(
                    tiketName,
                    bolge,
                    screenshotPath,
                    role
                );

                Dictionary<string, string> resultContext = new Dictionary<string, string>
                {
                    { "Success", success.ToString() },
                    { "TicketId", ticketId ?? string.Empty }
                };
                ticketLogger.LogInfo("AutoTicketSendResult", resultContext);

                return success;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> exceptionContext = new Dictionary<string, string>
                {
                    { "Message", ex.Message }
                };
                ticketLogger.LogError("AutoTicketSendException", exceptionContext);
                return false;
            }
        }

        #endregion

        #region Ticket система с бутони (като в ElTest)

        /// <summary>
        /// Създава ARIZA бутон на мястото на lblToplam (Donanim)
        /// </summary>
        private void CreateArizaButton()
        {
            // Скриваме lblToplam (надписа "Donanim")
            lblToplam.Visible = false;
            
            // Създаваме ARIZA бутон
            btnAriza = new Button();
            btnAriza.Text = "ARIZA";
            btnAriza.Dock = DockStyle.Fill; // Заема цялото пространство
            btnAriza.BackColor = Color.FromArgb(220, 53, 69); // Червен цвят
            btnAriza.ForeColor = Color.White;
            btnAriza.FlatStyle = FlatStyle.Flat;
            btnAriza.FlatAppearance.BorderSize = 0;
            btnAriza.Font = new Font("Segoe UI", 16F, FontStyle.Bold); // По-голям шрифт
            btnAriza.Cursor = Cursors.Hand;
            btnAriza.Click += BtnAriza_Click;
            
            // Добавяме бутона в tableLayoutPanel4 на мястото на lblToplam (row 0, col 0)
            tableLayoutPanel4.Controls.Add(btnAriza, 0, 0);
            btnAriza.BringToFront();
        }

        /// <summary>
        /// Обработва натискането на ARIZA бутона
        /// </summary>
        private void BtnAriza_Click(object sender, EventArgs e)
        {
            if (!isTicketExpanded)
            {
                // Разширяваме и зареждаме тикет бутоните
                LoadTicketButtons();
                isTicketExpanded = true;
            }
            else
            {
                // Свиваме и скриваме тикет бутоните
                CollapseTicketButtons();
                isTicketExpanded = false;
            }
        }

        /// <summary>
        /// Зарежда динамично тикет бутоните от базата данни
        /// </summary>
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

                var ticketsResult = _repo.GetRepository<SyTicketName>().GetAll(null);
                if (ticketsResult == null || ticketsResult.Data == null)
                {
                    Console.WriteLine("❌ GozKontrol: ticketsResult е null! Грешка при зареждане на тикети от базата данни.");
                    return;
                }

                var visibleTicketIds = Nursan.XMLTools.XMLSeverIp.VisibleTicketTypeIds();
                var tickets = ticketsResult.Data.ToList();

                if (visibleTicketIds.Any())
                {
                    tickets = tickets.Where(t => visibleTicketIds.Contains(t.Id)).ToList();
                    Console.WriteLine($"GozKontrol: Филтрирани тикети според Baglanti.xml ({tickets.Count} от {ticketsResult.Data.Count()}).");
                }

                if (!tickets.Any())
                {
                    Console.WriteLine("⚠️ GozKontrol: Няма налични тикети след прилагане на филтъра VisibleTicketTypeIds.");
                    return;
                }

                int totalButtons = tickets.Count;
                int screenWidth = this.Width;
                int maxColumns = Math.Max(1, screenWidth / (btnWidth + marginX));
                int buttonsPerRow = Math.Min(maxColumns, Math.Max(1, (int)Math.Ceiling(Math.Sqrt(totalButtons))));

                Console.WriteLine($"Общо тикети: {totalButtons}");
                
                int count = 0;
                foreach (var ticket in tickets)
                {
                    int row = count / buttonsPerRow;
                    int col = count % buttonsPerRow;

                    Button btn = new Button();
                    btn.Text = ticket.TiketName;
                    btn.Width = btnWidth;
                    btn.Height = btnHeight;
                    btn.Left = 10 + col * (btnWidth + marginX);
                    btn.Top = startY + row * (btnHeight + marginY);

                    // Модерен дизайн на бутона
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.BackColor = Color.FromArgb(45, 45, 48);
                    btn.ForeColor = Color.White;
                    btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                    btn.Cursor = Cursors.Hand;

                    // Добавяме hover ефект
                    btn.MouseEnter += (s, e) =>
                    {
                        Button b = s as Button;
                        b.BackColor = Color.FromArgb(0, 122, 204);
                        b.ForeColor = Color.White;
                    };
                    btn.MouseLeave += (s, e) =>
                    {
                        Button b = s as Button;
                        b.BackColor = Color.FromArgb(45, 45, 48);
                        b.ForeColor = Color.White;
                    };
              
                    btn.Tag = ticket;
                    btn.Click += TicketButton_Click;

                    this.Controls.Add(btn);
                    btn.BringToFront();
                    dynamicTicketButtons.Add(btn);

                    count++;
                }

                Console.WriteLine($"Заредени {count} тикет бутона");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при зареждане на бутоните: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Грешка при LoadTicketButtons: {ex.Message}");
            }
        }

        /// <summary>
        /// Свива и премахва тикет бутоните
        /// </summary>
        private void CollapseTicketButtons()
        {
            foreach (var btn in dynamicTicketButtons)
            {
                this.Controls.Remove(btn);
                btn.Dispose();
            }
            dynamicTicketButtons.Clear();
            Console.WriteLine("Тикет бутоните са скрити");
        }

        /// <summary>
        /// Обработва натискането на тикет бутон
        /// </summary>
        private void TicketButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var ticket = btn.Tag as SyTicketName;
            if (ticket != null)
            {
                Dictionary<string, string> selectionContext = new Dictionary<string, string>
                {
                    { "TicketName", SensitiveDataMasker.MaskValue(ticket.TiketName) },
                    { "Role", (ticket.Role ?? 5).ToString() }
                };
                ticketLogger.LogInfo("TicketButtonSelected", selectionContext);

                if (Nursan.XMLTools.XMLSeverIp.WebApiTrue())
                {
                    ManualSendTicketWithScreenshot();
                    int roleValue = ticket.Role ?? 5; // Ако Role е null, използваме 5 като default
                    ShowQrCodeAfterTicketCreation(ticket.TiketName, ticket.Description, lastScreenshotPath, roleValue);
                }
                else
                {
                    ticketLogger.LogWarning(
                        "WebApiDisabled",
                        new Dictionary<string, string>
                        {
                            { "TicketName", SensitiveDataMasker.MaskValue(ticket.TiketName) }
                        });
                }

                // Скриваме тикет бутоните след избор
                CollapseTicketButtons();
                isTicketExpanded = false;
            }
        }

        /// <summary>
        /// Прави screenshot за ръчно изпратен тикет
        /// </summary>
        private void ManualSendTicketWithScreenshot()
        {
            try
            {
                // Правим screenshot
                var bounds = Screen.PrimaryScreen.Bounds;
                using (var bmp = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
                    }
                    
                    // Създаваме LOGS папка ако не съществува
                    string logsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LOGS");
                    if (!Directory.Exists(logsFolder))
                    {
                        Directory.CreateDirectory(logsFolder);
                    }
                    
                    string fileName = $"ticket_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    string fullPath = Path.Combine(logsFolder, fileName);
                    
                    bmp.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
                    lastScreenshotPath = fullPath;

                    Dictionary<string, string> screenshotContext = new Dictionary<string, string>
                    {
                        { "ScreenshotName", SensitiveDataMasker.MaskPath(lastScreenshotPath) }
                    };
                    ticketLogger.LogInfo("ManualScreenshotCaptured", screenshotContext);
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorContext = new Dictionary<string, string>
                {
                    { "Message", ex.Message }
                };
                ticketLogger.LogError("ManualScreenshotFailure", errorContext);
            }
        }

        /// <summary>
        /// Асинхронно създава тикет и показва QR код за проследяване
        /// </summary>
        private async void ShowQrCodeAfterTicketCreation(string tiketName, string description, string screenshotPath, int roleValue)
        {
            try
            {
                Dictionary<string, string> startContext = new Dictionary<string, string>
                {
                    { "TicketName", SensitiveDataMasker.MaskValue(tiketName) },
                    { "Role", roleValue.ToString() },
                    { "ScreenshotName", SensitiveDataMasker.MaskPath(screenshotPath) }
                };
                ticketLogger.LogInfo("ManualTicketStart", startContext);

                string bolge = _makine?.MasineName ?? "GozKontrol";
                
                // Първо пращаме тикета
                var (success, serverTicketId) = await _systemTicket.CreateTicket(tiketName, bolge, screenshotPath, roleValue);
                Dictionary<string, string> createContext = new Dictionary<string, string>
                {
                    { "Success", success.ToString() },
                    { "TicketId", serverTicketId ?? string.Empty }
                };
                ticketLogger.LogInfo("ManualTicketResult", createContext);
                
                if (success)
                {
                    string serverIp = Nursan.XMLTools.XMLSeverIp.XmlWebApiIP();
                    Dictionary<string, string> qrContext = new Dictionary<string, string>
                    {
                        { "ServerIp", SensitiveDataMasker.MaskIp(serverIp) },
                        { "TicketId", serverTicketId ?? string.Empty }
                    };
                    ticketLogger.LogInfo("QrDisplayTriggered", qrContext);

                    QrTicketForm qrForm = new QrTicketForm(serverTicketId, serverIp);
                    qrForm.Show();
                }
                else
                {
                    ticketLogger.LogError(
                        "ManualTicketFailed",
                        new Dictionary<string, string>
                        {
                            { "TicketName", SensitiveDataMasker.MaskValue(tiketName) }
                        });
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> exceptionContext = new Dictionary<string, string>
                {
                    { "Message", ex.Message }
                };
                ticketLogger.LogError("ManualTicketException", exceptionContext);
            }
        }

        #endregion

    }
}

