using Nursan.Business.Logging;
using Nursan.Business.Services;
using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.UI.Library;
using Nursan.UI.OzelClasslar;
using Nursan.Validations.ValidationCode;
using Nursan.XMLTools;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Nursan.UI
{
    public partial class Paket : Form
    {
        private readonly UnitOfWork _repo;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<UrModulerYapi> _modulerYapiList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        private static List<SyBarcodeOut> _syBarcodeOutList;
        private static List<SyPrinter> _syPrinterList;
        private static List<OrFamily> _familyList;
        BarcodeValidation barcode;
        CountDegerValidations _countDegerValidations;
        OzelReferansControlEt ozel;
        int pi;
        TorkService tork;
        List<SyBarcodeInput> secondSyBarcodeInputList = new List<SyBarcodeInput>();
        static int brcodeVCount;
        Proveri proveri = new Proveri();
        int brtSayi = 0;
        
        // Ticket система полета
        private List<Button> dynamicTicketButtons = new List<Button>();
        private bool isTicketExpanded = false;
        private readonly SystemTicket _systemTicket;
        private Button btnAriza;
        private string lastScreenshotPath = "";
        private readonly StructuredLogger ticketLogger;
        public Paket(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
        {
            _repo = repo;
            _makine = makine;
            _vardiya = vardiya;
            _modulerYapiList = modulerYapiList;
            _istasyonList = istasyonList;
            _syBarcodeInputList = syBarcodeInputList;
            _syBarcodeOutList = syBarcodeOutList;
            _syPrinterList = syPrinterList;
            _familyList = familyList;
            brcodeVCount = _syBarcodeInputList.Count;
            brtSayi = brcodeVCount;
            _countDegerValidations = new CountDegerValidations(_repo, _makine, _vardiya, _istasyonList);
            _systemTicket = new SystemTicket();
            ticketLogger = new StructuredLogger(nameof(Paket));

            // Добавяне на exception handlers за автоматично генериране на тикети при crash
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            InitializeComponent();
            tork = new TorkService(repo, vardiya);
            ozel = new OzelReferansControlEt(repo);
            
            // Създаване на ARIZA бутон
            CreateArizaButton();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            GetCounts();
            Staring frm = new Staring();
            Thread.Sleep(500);
            frm.Dispose();
            frm.Close();
        }

        private void txtBarcode_KeyUpAsync(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (_vardiya.Name != txtBarcode.Text)
                {
                    pictureBox1.Image = null;
                    if (txtBarcode.Text.StartsWith("#"))
                    {
                        string[] parcala = txtBarcode.Text.Substring(1).Split('-');
                        string suffix = Regex.Replace(parcala[2], "[^a-z,A-Z,@,^,/,]", "");
                        string veri = $"{parcala[0]}-{parcala[1]}-{suffix}";
                        var ozelRezult = ozel.ControletSystemi(veri, int.Parse(Regex.Replace(parcala[2], "[^0-9]", "")).ToString());
                        if (ozelRezult.Success && ozelRezult.Data != null)
                        {
                            using (MemoryStream ms = new MemoryStream(ozelRezult.Data.image))
                            {
                                pictureBox1.Image = Image.FromStream(ms);
                            }
                            txtBarcode.Clear();
                            proveri.MessageAyarla($"{ozelRezult.Message} {txtBarcode.Text} ", ozelRezult.Success != true ? Color.LightBlue : Color.Red, lblMessage);
                            listBox1.Items.Clear();
                            return;
                        }
                        else
                        {
                            lblMessage.Text = "";
                            listBox1.Items.Add(txtBarcode.Text);
                            _syBarcodeInputList[pi].BarcodeIcerik = txtBarcode.Text;
                            proveri.MessageAyarla($"Sigorta Etiketini Okuttunuz! {txtBarcode.Text}", Color.Lime, lblMessage);
                            if (!txtBarcode.Text.StartsWith(_syBarcodeInputList[pi].OzelChar == null ? "" : _syBarcodeInputList[pi].OzelChar))
                            {
                                proveri.MessageAyarla($"Yanlis Brcode Okudunuz!", Color.Red, lblMessage);
                                txtBarcode.Clear(); listBox1.Items.Clear(); pi = 0;
                                return;
                            }
                            pi++;
                            txtBarcode.Clear();
                            //GetCounts();
                        }
                    }
                    else
                    {
                        listBox1.Items.Add(txtBarcode.Text);
                        _syBarcodeInputList[pi].BarcodeIcerik = txtBarcode.Text;
                        if (_syBarcodeInputList != null && _syBarcodeInputList.Count > 0 && 
                            _syBarcodeInputList.First().BarcodeIcerik != null &&
                            _syBarcodeInputList.Last().BarcodeIcerik != null &&
                            _syBarcodeInputList.First().BarcodeIcerik.Contains(_syBarcodeInputList.Last().BarcodeIcerik))
                        {
                            if (!txtBarcode.Text.StartsWith(_syBarcodeInputList[pi].OzelChar == null ? "" : _syBarcodeInputList[pi].OzelChar))
                            {
                                proveri.MessageAyarla($"Yanlis Barcode Okudunuz!", Color.Red, lblMessage);
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
                            proveri.MessageAyarla($"Okutugunuz Barcod-lar Yalnis!!!", Color.Red, lblMessage);
                            for (int i = 0; i < pi; i++)
                            {
                                proveri.MessageAyarla($"Yanlis Brcode Okudunuz!", Color.Red, lblMessage);
                                txtBarcode.Clear(); listBox1.Items.Clear(); pi = 0;
                                return;
                            }
                        }
                    }
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
        
        #region Ticket система с бутони (като в GozKontrol)

        /// <summary>
        /// Създава ARIZA бутон на мястото на lblToplam (Donanim)
        /// </summary>
        private void CreateArizaButton()
        {
            // Скриваме lblToplam (надписа "Paket Sistemi")
            lblToplam.Visible = false;
            
            // Създаваме ARIZA бутон
            btnAriza = new Button();
            btnAriza.Text = "ARIZA";
            btnAriza.Dock = DockStyle.Fill; // Заема цялото пространство
            btnAriza.BackColor = Color.FromArgb(220, 53, 69); // Червен цвят
            btnAriza.ForeColor = Color.White;
            btnAriza.FlatStyle = FlatStyle.Flat;
            btnAriza.FlatAppearance.BorderSize = 0;
            btnAriza.Font = new Font("Segoe UI", 12F, FontStyle.Bold); 
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
                foreach (Button btn in dynamicTicketButtons)
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
                    Console.WriteLine("❌ Paket: ticketsResult е null! Грешка при зареждане на тикети от базата данни.");
                    return;
                }

                var visibleTicketIds = XMLSeverIp.VisibleTicketTypeIds();
                List<SyTicketName> tickets = ticketsResult.Data.ToList();

                if (visibleTicketIds.Any())
                {
                    tickets = tickets.Where(t => visibleTicketIds.Contains(t.Id)).ToList();
                    Console.WriteLine($"Paket: Филтрирани тикети ({tickets.Count} от {ticketsResult.Data.Count()}) според VisibleTicketTypeIds.");
                }

                if (!tickets.Any())
                {
                    Console.WriteLine("⚠️ Paket: Няма налични тикети в базата данни.");
                    return;
                }

                int totalButtons = tickets.Count;
                int screenWidth = this.Width;
                int maxColumns = Math.Max(1, screenWidth / (btnWidth + marginX));
                int buttonsPerRow = Math.Min(maxColumns, Math.Max(1, (int)Math.Ceiling(Math.Sqrt(totalButtons))));

                Console.WriteLine($"Общо тикети: {totalButtons}");
                
                int count = 0;
                foreach (SyTicketName ticket in tickets)
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
                Console.WriteLine($"Грешка при LoadTicketButtons: {ex.Message}");
            }
        }

        /// <summary>
        /// Свива и премахва тикет бутоните
        /// </summary>
        private void CollapseTicketButtons()
        {
            foreach (Button btn in dynamicTicketButtons)
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
            Button btn = sender as Button;
            SyTicketName ticket = btn.Tag as SyTicketName;
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
                    int roleValue = ticket.Role ?? 5;
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
                Rectangle bounds = Screen.PrimaryScreen.Bounds;
                using (Bitmap bmp = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
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
                    
                    bmp.Save(fullPath, ImageFormat.Png);
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

                string bolge = _makine?.MasineName ?? "Paket";
                
                // Първо пращаме тикета
                (bool success, string serverTicketId) = await _systemTicket.CreateTicket(tiketName, bolge, screenshotPath, roleValue);
                Dictionary<string, string> resultContext = new Dictionary<string, string>
                {
                    { "Success", success.ToString() },
                    { "TicketId", serverTicketId ?? string.Empty }
                };
                ticketLogger.LogInfo("ManualTicketResult", resultContext);
                
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

        #region Автоматична система за тикети при crash

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception, "Thread Exception в Paket");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "Unhandled Exception в Paket");
            }
        }

        private void HandleException(Exception ex, string context)
        {
            try
            {
                string screenshotPath = TakeScreenshot();
                string errorDetails = $@"
ГРЕШКА В ФОРМА: Paket
КОНТЕКСТ: {context}
ДАТА/ЧАС: {DateTime.Now:dd.MM.yyyy HH:mm:ss}
МАШИНА: {Environment.MachineName}
ПОТРЕБИТЕЛ: {Environment.UserName}

СЪОБЩЕНИЕ ЗА ГРЕШКА:
{ex.Message}

STACK TRACE:
{ex.StackTrace}

ВЪТРЕШНА ГРЕШКА:
{ex.InnerException?.Message ?? "Няма"}
{ex.InnerException?.StackTrace ?? ""}
";

                ticketLogger.LogError("AutoTicketTriggered", new Dictionary<string, string>
                {
                    { "Context", SensitiveDataMasker.MaskValue(context) },
                    { "ScreenshotName", SensitiveDataMasker.MaskPath(screenshotPath) }
                });

                Task.Run(async () =>
                {
                    await SendAutoTicketToIT($"AUTO CRASH: Paket - {context}", errorDetails, screenshotPath, 1);
                });
            }
            catch (Exception ticketEx)
            {
                ticketLogger.LogError("AutoTicketFailure", new Dictionary<string, string> { { "Message", ticketEx.Message } });
            }
        }

        private string TakeScreenshot()
        {
            try
            {
                string logsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LOGS");
                if (!Directory.Exists(logsFolder))
                    Directory.CreateDirectory(logsFolder);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string filename = $"CRASH_Paket_{timestamp}.jpg";
                string filepath = Path.Combine(logsFolder, filename);

                Rectangle bounds = Screen.PrimaryScreen.Bounds;
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }
                    bitmap.Save(filepath, ImageFormat.Jpeg);
                }

                ticketLogger.LogInfo("ScreenshotCreated", new Dictionary<string, string> { { "ScreenshotName", SensitiveDataMasker.MaskPath(filepath) } });
                return filepath;
            }
            catch (Exception ex)
            {
                ticketLogger.LogError("ScreenshotFailure", new Dictionary<string, string> { { "Message", ex.Message } });
                return string.Empty;
            }
        }

        private async Task<bool> SendAutoTicketToIT(string tiketName, string description, string screenshotPath, int role)
        {
            try
            {
                string bolge = Environment.MachineName;
                ticketLogger.LogInfo("AutoTicketSendStart", new Dictionary<string, string>
                {
                    { "TicketName", SensitiveDataMasker.MaskValue(tiketName) },
                    { "Bolge", SensitiveDataMasker.MaskValue(bolge) },
                    { "ScreenshotName", SensitiveDataMasker.MaskPath(screenshotPath) },
                    { "Role", role.ToString() }
                });

                (bool success, string ticketId) = await _systemTicket.CreateTicket(tiketName, bolge, screenshotPath, role);
                ticketLogger.LogInfo("AutoTicketSendResult", new Dictionary<string, string>
                {
                    { "Success", success.ToString() },
                    { "TicketId", ticketId ?? string.Empty }
                });
                return success;
            }
            catch (Exception ex)
            {
                ticketLogger.LogError("AutoTicketSendException", new Dictionary<string, string> { { "Message", ex.Message } });
                return false;
            }
        }

        #endregion
    }
}
