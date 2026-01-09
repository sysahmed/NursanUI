using Nursan.Business.Logging;
using Nursan.Business.Manager;
using Nursan.Business.Services;
using Nursan.Domain.AmbarModels;
using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.UI.Services;
using Nursan.UI.DTOs;
using Nursan.Validations.SortedList;
using Nursan.XMLTools;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms; // За Clipboard
using XMLIslemi = Nursan.Core.Printing.XMLIslemi;

namespace Nursan.UI
{
    public partial class ElTestvApi: Form
    {
        private string deger;
        private FileSystemWatcher watcher1;
        private FileSystemWatcher watcher2;
        private FileSystemWatcher watcher3;
        private FileSystemWatcher Kliptest_3;
        private Messaglama mes = new Messaglama();
        // private XMLIslemi xmlis = new XMLIslemi();
        static string[] pathc = { "C:\\Kliptest\\", "C:\\Kliptest_2\\", "C:\\Klt\\", "C:\\Kliptest_3\\", "C:\\DEMO\\", "C:\\_Kliptest\\", };
        private ElTestApiService _elTestApi;
        private StationBootstrapDto? _stationBootstrap;
        private static string format = $"*.txt";
        private ScreenSaverForm scren;
        private SicilOkuma sicil;
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
        private readonly StructuredLogger ticketLogger;
        private readonly StructuredLogger apiKeyLogger;
        public ElTestvApi()
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

            // Разпъни формата по цялата ширина на екрана и височина 300px
            this.Left = 0;
            this.Top = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = 300;
            _systemTicket = new SystemTicket();
            ticketLogger = new StructuredLogger(nameof(ElTestvApi));
            apiKeyLogger = new StructuredLogger("ElTestvApi.ApiKey");

            // Добавяне на exception handlers за автоматично генериране на тикети при crash
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

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
            lblVersion.Text = $"ElTest-vApi {Environment.Version}";
            _elTestApi = new ElTestApiService();
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
            if (XMLSeverIp.ElTestCount())
            {

                lblCountProductions.Enabled = true;
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

        private async void Watcher1_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());

                await Watcher1(veri.Path, veri.Filter);
            }
            catch (Exception ex)
            {
                mes.messanger(ex.Message);
            }
        }

        private async void Watcher2_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());
                await Watcher2(veri.Path, veri.Filter);
            }
            catch (Exception ex)
            {
                mes.messanger(ex.Message);
            }
        }

        private async void Watcher3_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());
                await Watcher3(veri.Path, veri.Filter);
            }
            catch (Exception ex)
            {
                mes.messanger(ex.Message);
            }
        }
        private async Task Watcher1(string Path, string Format)
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
                    // Проверяваме дали има валиден API Key преди операцията
                    if (!EnsureApiKeyIsValidForOperation())
                    {
                        File.Delete(string.Concat(Path, fileInfo.Name));
                        return;
                    }
                    
                    string[] getParca = fileInfo.Name.ToUpper().Split('_');
                    string vardiyaName = getParca.Length > 2 ? getParca[2] : "";
                    var result = await _elTestApi.GitSystemeYukle(getParca, vardiyaName);
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
                catch (Exception ex)
                {
                    mes.messanger(ex.Message);
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
            }
        }
        private async void KlipTest_3_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());
                await KlipTest_3(veri.Path, veri.Filter);
            }
            catch (Exception ex)
            {
                mes.messanger(ex.Message);
            }
        }

        private static async Task DeleteFileAsync(string filePath)
        {
            await Task.Run(() => File.Delete(filePath));
        }
        private async Task Watcher2(string Path, string Format)
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
                    await GitSystemeDesktopKapa(fileInfo.Name.ToUpper());
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
                catch (Exception ex)
                {
                    mes.messanger(ex.Message);
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
            }
        }

        private async Task Watcher3(string Path, string Format)
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
                    // TODO: За сега използваме същия метод като Watcher1, тъй като GithataYukle не е имплементиран в API
                    // В бъдеще може да се създаде специален endpoint за обработка на грешки
                    // Проверяваме дали има валиден API Key преди операцията
                    if (!EnsureApiKeyIsValidForOperation())
                    {
                        File.Delete(string.Concat(Path, fileInfo.Name));
                        return;
                    }
                    
                    string[] getParca = fileInfo.Name.ToUpper().Split('_');
                    string vardiyaName = getParca.Length > 2 ? getParca[2] : "";
                    var result = await _elTestApi.GitSystemeYukle(getParca, vardiyaName);
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
                catch (Exception ex)
                {
                    mes.messanger(ex.Message);
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
            }
        }

        private async Task KlipTest_3(string Path, string Format)
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
                    await GitSystemeDesktopAc($"{Path}{fileInfo.Name.ToUpper()}");
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
                catch (Exception ex)
                {
                    mes.messanger(ex.Message);
                    File.Delete(string.Concat(Path, fileInfo.Name));
                }
            }
        }

        private void ElTest_Load(object sender, EventArgs e)
        {
            // Проверяваме API Key статуса и блокираме ако не е валиден
            _ = CheckApiKeyStatusAndBlockIfInvalidAsync();
        }

        /// <summary>
        /// Флаг дали API Key е валиден и може да работи с API-то
        /// </summary>
        private bool _isApiKeyValid = false;

        /// <summary>
        /// Проверява дали API Key е валиден преди извършване на операция
        /// Ако не е валиден, показва съобщение и блокира операцията
        /// </summary>
        private bool EnsureApiKeyIsValidForOperation()
        {
            if (!_isApiKeyValid)
            {
                // Операцията е блокирана - API Key не е валиден
                MessageBox.Show(
                    "API Key не е валиден или не е аутентикиран.\n\nПриложението не може да работи без валиден API Key.\n\nМоля, проверете API Key в конфигурацията или рестартирайте приложението.",
                    "API Key грешка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Проверява API Key от XML конфигурацията, валидира срещу API-то и блокира приложението ако не е валиден
        /// </summary>
        private async Task CheckApiKeyStatusAndBlockIfInvalidAsync()
        {
            try
            {
                // Четем API Key и DeviceId от XML
                string? apiKey = ApiKeyManager.GetApiKey();
                string? deviceId = ApiKeyManager.GetDeviceId();
                
                if (string.IsNullOrEmpty(apiKey))
                {
                    // API Key не е намерен - блокираме приложението
                    apiKeyLogger.LogError("ApiKeyNotFound", new Dictionary<string, string>
                    {
                        { "Message", "API Key не е намерен в XML конфигурацията" },
                        { "MachineName", Environment.MachineName }
                    });
                    
                    _isApiKeyValid = false;
                    BlockApplication("API Key не е намерен в конфигурацията.\n\nМоля, генерирайте API Key от MVC интерфейса на API-то и го запишете в Baglanti.xml файла.");
                    return;
                }

                // Ако ключът е в стар (plaintext) формат, мигрираме го към DPAPI (LocalMachine)
                // Това не променя ключа към API-то, само начина на съхранение в Baglanti.xml
                ApiKeyManager.TryMigratePlaintextApiKeyToDpapi(deviceId);

                // Валидираме срещу API-то
                bool isValid = await ValidateApiKeyWithServerAsync(apiKey, deviceId);
                
                if (!isValid)
                {
                    // API Key не е валиден - блокираме приложението
                    apiKeyLogger.LogError("ApiKeyValidationFailed", new Dictionary<string, string>
                    {
                        { "Message", "API Key не е валиден или не е активен" },
                        { "DeviceId", deviceId ?? "NotProvided" },
                        { "MachineName", Environment.MachineName }
                    });
                    
                    _isApiKeyValid = false;
                    BlockApplication("API Key не е валиден или не е активен.\n\nМоля, проверете API Key в MVC интерфейса на API-то или генерирайте нов.");
                    return;
                }

                // API Key е валиден - активираме приложението
                apiKeyLogger.LogInfo("ApiKeyValidated", new Dictionary<string, string>
                {
                    { "Message", "API Key е валидиран успешно" },
                    { "DeviceId", deviceId ?? "NotProvided" },
                    { "MachineName", Environment.MachineName }
                });
                
                _isApiKeyValid = true;

                // 1) Зареждаме конфигурацията за станцията при старт (както локалната версия я "знае" от Program.cs/DB)
                _stationBootstrap = await _elTestApi.GetStationBootstrapAsync(Environment.MachineName);
                if (_stationBootstrap == null || _stationBootstrap.Station == null)
                {
                    _isApiKeyValid = false;
                    BlockApplication(
                        "Не успях да заредя конфигурацията на станцията от API.\n\n" +
                        "Проверете дали API работи и дали е настроена активна станция за тази машина (MachineName).\n\n" +
                        $"MachineName: {Environment.MachineName}");
                    return;
                }
                
                // 2) Стартираме функционалностите едва след като bootstrap конфигурацията е налична
                WatcherStart();
                TaskBaraAl();
            }
            catch (Exception ex)
            {
                // Грешка при проверка - блокираме приложението за сигурност
                apiKeyLogger.LogError("ApiKeyCheckException", new Dictionary<string, string>
                {
                    { "Message", ex.Message },
                    { "StackTrace", ex.StackTrace ?? "N/A" },
                    { "MachineName", Environment.MachineName }
                });
                
                _isApiKeyValid = false;
                BlockApplication($"Грешка при валидация на API Key:\n{ex.Message}\n\nМоля, проверете връзката с API сървъра.");
            }
        }

        /// <summary>
        /// Блокира приложението - скрива контролите и показва съобщение
        /// </summary>
        private void BlockApplication(string message)
        {
            try
            {
                // Спираме FileSystemWatcher ако работи
                WatcherStop();
                
                // Скриваме всички контроли (освен може би съобщението)
                this.Enabled = false;
                
                // Показваме съобщение
                MessageBox.Show(
                    $"Приложението е блокирано!\n\n{message}\n\nПриложението ще се затвори.",
                    "ElTestvApi - API Key грешка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Затваряме приложението
                this.Close();
            }
            catch (Exception)
            {
                // Грешка при блокиране на приложението - игнорираме
            }
        }

        /// <summary>
        /// Валидира API Key срещу API сървъра
        /// Връща true ако API Key е валиден и може да работи с API-то
        /// </summary>
        private async Task<bool> ValidateApiKeyWithServerAsync(string apiKey, string? deviceId)
        {
            try
            {
                // Вземаме Master API Address от XML - това е основният адрес за всички API заявки
                // Тикетите имат свой отделен адрес в ticketTracking node
                string masterApiAddress = XMLSeverIp.XmlMasterApiAddress();
                
                // Определяме протокола: за localhost използваме http, за останалото https
                string protocol = (masterApiAddress.StartsWith("localhost", StringComparison.OrdinalIgnoreCase) || 
                                  masterApiAddress.StartsWith("127.0.0.1")) ? "http" : "https";
                string apiUrl = $"{protocol}://{masterApiAddress}/api/auth/validate-api-key";
                
                if (!string.IsNullOrEmpty(deviceId))
                {
                    apiUrl += $"?deviceId={Uri.EscapeDataString(deviceId)}";
                }

                var handler = new System.Net.Http.HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                
                using (var client = new System.Net.Http.HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
                    if (!string.IsNullOrEmpty(deviceId))
                    {
                        client.DefaultRequestHeaders.Add("X-Device-Id", deviceId);
                    }
                    
                    var response = await client.PostAsync(apiUrl, null);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        // Парсваме отговора
                        try
                        {
                            var result = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(responseContent);
                            var canWork = result.TryGetProperty("CanWorkWithApi", out var canWorkProp) && canWorkProp.GetBoolean();
                            
                            if (canWork)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            // Ако не можем да парснем отговора, приемаме че е успешен ако статусът е OK
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                apiKeyLogger.LogError("ApiKeyValidationException", new Dictionary<string, string>
                {
                    { "Message", ex.Message },
                    { "StackTrace", ex.StackTrace ?? "N/A" },
                    { "DeviceId", deviceId ?? "NotProvided" },
                    { "MachineName", Environment.MachineName }
                });
                return false;
            }
        }

        /// <summary>
        /// Генерира API Key локално и го записва в XML (тихо без съобщения)
        /// </summary>
        private void GenerateApiKeyLocally()
        {
            try
            {
                // Използваме ApiKeyManager за генериране
                string? newApiKey = ApiKeyManager.GenerateAndSaveApiKey();
                
                if (!string.IsNullOrEmpty(newApiKey))
                {
                    // Презареждаме API Service с новия ключ
                    _elTestApi?.Dispose();
                    _elTestApi = new ElTestApiService();
                }
                // API Key не може да се генерира - правата за достъп до Baglanti.xml не са достатъчни
            }
            catch (Exception)
            {
                // Грешка при генериране на API Key - игнорираме
            }
        }

        /// <summary>
        /// Генерира API Key от API endpoint (ако API е достъпен) - тихо без съобщения
        /// </summary>
        private async void GenerateApiKeyFromApi()
        {
            try
            {
                string serverIp = XMLSeverIp.XmlWebApiIP();
                string apiUrl = $"https://{serverIp}/api/auth/generate-api-key";
                
                var handler = new System.Net.Http.HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                
                using (var client = new System.Net.Http.HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    var response = await client.PostAsync(apiUrl, null);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        
                        // Парсваме JSON и записваме в XML
                        try
                        {
                            using (var doc = System.Text.Json.JsonDocument.Parse(jsonResponse))
                            {
                                if (doc.RootElement.TryGetProperty("ApiKey", out var apiKeyElement))
                                {
                                    string? apiKey = apiKeyElement.GetString();
                                    if (!string.IsNullOrEmpty(apiKey))
                                    {
                                        // Записваме в локалния XML
                                        if (ApiKeyManager.SaveApiKey(apiKey))
                                        {
                                            // API Key е генериран от сървъра и записан
                                            
                                            // Презареждаме API Service
                                            _elTestApi?.Dispose();
                                            _elTestApi = new ElTestApiService();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // Fallback към локално генериране
                            GenerateApiKeyLocally();
                        }
                    }
                    else
                    {
                        // Fallback към локално генериране
                        GenerateApiKeyLocally();
                    }
                }
            }
            catch (Exception)
            {
                // Fallback към локално генериране
                GenerateApiKeyLocally();
            }
        }

        /// <summary>
        /// Маскира API Key за показване (показва само първите и последните символи)
        /// </summary>
        private string MaskApiKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey) || apiKey.Length <= 8)
            {
                return "***";
            }
            
            // Показваме първите 4 и последните 4 символа
            string start = apiKey.Substring(0, 4);
            string end = apiKey.Substring(apiKey.Length - 4);
            return $"{start}...{end}";
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

        private async Task GitSystemeDesktopAc(string name)
        {
            FileInfo[] files = (new DirectoryInfo("C:\\kliptest_3")).GetFiles("*.txt");
            for (int i = 0; i < (int)files.Length; i++)
            {
                FileInfo fileInfo = files[i];
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                this.deger = fileInfo.Name.ToUpper();
                string[] strArrays = this.deger.Split(new char[] { '\u005F' });
                var gelenDegerler = GitParcalama(new SyBarcodeOut { BarcodeIcerik = deger });
                string barcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}";
                // Проверяваме дали има валиден API Key преди операцията
                if (!EnsureApiKeyIsValidForOperation())
                {
                    return;
                }
                
                var idBak = await _elTestApi.CheckSystemElTest(barcodeIcerik, gelenDegerler.Name);
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
                catch (Exception ex)
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
        private async Task GitSystemeDesktopKapa(string name)
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
                    string barcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}";
                    // Проверяваме дали има валиден API Key преди операцията
                    if (!EnsureApiKeyIsValidForOperation())
                    {
                        return;
                    }
                    
                    var idBak = await _elTestApi.CheckSystem(barcodeIcerik, gelenDegerler.Name);
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
                catch (Exception ex)
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

            // Освобождаваме ресурсите
            _screenMonitor?.Dispose();
            _elTestApi?.Dispose();
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
        public async Task AddTicket(string tiketName, string description, int role)
        {
            // Използваме SystemTicket.CreateTicket който вече работи с API
            // Ако WebApi е изключено, все пак използваме SystemTicket за консистентност
            try
            {
                // Проверяваме дали има валиден API Key преди операцията
                if (!EnsureApiKeyIsValidForOperation())
                {
                    return;
                }
                
                decimal? pcId = await _elTestApi.GetPcId();
                string bolge = Environment.MachineName;
                
                // SystemTicket.CreateTicket вече използва API
                await _systemTicket.CreateTicket(tiketName, bolge, null, role);
            }
            catch (Exception ex)
            {
                mes.messanger($"Грешка при добавяне на тикет: {ex.Message}");
            }
        }

        private async void LoadTicketButtons()
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

                // Зареждаме тикетите от API
                string roleName = string.Empty; // Може да се конфигурира в baglanti.xml ако е необходимо
                List<TickedRolleNote> tickets = await _systemTicket.GetTicketsByRoleName();

                if (tickets == null || !tickets.Any())
                {
                    MessageBox.Show("Няма налични билети за зареждане.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Прилагаме филтъра от baglanti.xml
                var visibleTicketIds = XMLSeverIp.VisibleTicketTypeIds();
                if (visibleTicketIds.Any())
                {
                    tickets = tickets.Where(t => visibleTicketIds.Contains(t.Id)).ToList();
                    // Тикетите са филтрирани според VisibleTicketTypeIds
                }

                if (!tickets.Any())
                {
                    MessageBox.Show("Няма разрешени тикети за показване.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int totalButtons = tickets.Count;
                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int maxColumns = Math.Max(1, screenWidth / (btnWidth + marginX));
                int buttonsPerRow = Math.Min(maxColumns, Math.Max(1, (int)Math.Ceiling(Math.Sqrt(totalButtons))));

                int count = 0;
                foreach (var ticket in tickets)
                {
                    int row = count / buttonsPerRow;
                    int col = count % buttonsPerRow;

                    Button btn = new Button();
                    btn.Text = ticket.Description; // Използваме Description като текст на бутона
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

        private async void TicketButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var ticket = btn.Tag as TickedRolleNote;
            if (ticket != null)
            {
                Dictionary<string, string> selectionContext = new Dictionary<string, string>
                {
                    { "TicketDescription", SensitiveDataMasker.MaskValue(ticket.Description) },
                    { "TicketId", ticket.Id.ToString() },
                    { "RoleId", (ticket.RoleId ?? 5).ToString() }
                };
                ticketLogger.LogInfo("TicketButtonSelected", selectionContext);

                // Валидация преди изпращане на тикет
                //var validationResult = ValidateBeforeTicketSubmission(ticket);
                //if (!validationResult.IsValid)
                //{
                //    Console.WriteLine($"❌ ElTest: Валидацията не мина - {validationResult.Message}");
                //    Console.WriteLine($"⚠️ Тикетът '{ticket.TiketName}' НЕ беше изпратен!");
                    
                //    // Скриваме бутоните и възстановяваме формата
                //    foreach (var b in dynamicTicketButtons)
                //    {
                //        this.Controls.Remove(b);
                //        b.Dispose();
                //    }
                //    dynamicTicketButtons.Clear();
                    
                //    foreach (Control control in this.Controls)
                //    {
                //        if (control != btnAriza && control != lblCountProductions)
                //        {
                //            control.Visible = false;
                //        }
                //    }
                    
                //    this.TransparencyKey = Color.WhiteSmoke;
                //    this.BackColor = Color.WhiteSmoke;
                //    isExpanded = false;
                //    return;
                //}
                
               // Console.WriteLine($"✅ ElTest: Валидацията мина успешно - {validationResult.Message}");
                
                if (XMLSeverIp.WebApiTrue())
                {
                    SendTicketWithScreenshot();
                    
                    // Използваме RoleId параметъра от тикета
                    int roleValue = ticket.RoleId ?? 5; // Ако RoleId е null, използваме 5 като default
                    ShowQrCodeAfterTicketCreation(ticket.Description, ticket.Description, lastScreenshotPath, roleValue);
                }
                else
                {
                    ticketLogger.LogWarning(
                        "WebApiDisabled",
                        new Dictionary<string, string>
                        {
                            { "TicketDescription", SensitiveDataMasker.MaskValue(ticket.Description) }
                        });
                    // Добавяме проверка за nullable RoleId
                    int roleValue = ticket.RoleId ?? 5; // Ако RoleId е null, използваме 5 като default
                    await AddTicket(ticket.Description, ticket.Description, roleValue);
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
        
        /// <summary>
        /// Валидира дали може да се изпрати тикет
        /// </summary>
        private async Task<(bool IsValid, string Message)> ValidateBeforeTicketSubmission(TickedRolleNote ticket)
        {
            try
            {
                // 1. Проверка дали има активен файл/баркод
                if (string.IsNullOrEmpty(deger))
                {
                    return (false, "Няма активен баркод/файл в момента");
                }
                
                // 2. Парсваме текущия баркод
                var gelenDegerler = GitParcalama(new SyBarcodeOut { BarcodeIcerik = deger });
                string currentBarcode = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}";
                string idDonanim = gelenDegerler.IdDonanim;
                
                // 3. Проверка дали IDDonanim съществува в системата
                var idBak = await _elTestApi.CheckSystemElTest(currentBarcode, gelenDegerler.Name);
                
                if (idBak.Message == "Donanimi ID Systemde Yok")
                {
                    return (false, $"IDDonanim '{idDonanim}' не съществува в системата");
                }
                
                // 4. Ако е IT тикет (RoleId = 1), проверяваме дали е минал през предходните станции
                int roleValue = ticket.RoleId ?? 5;
                if (roleValue == 1)
                {
                    if (idBak.Message == "Donanimi Bi oceki Istasyona Yonlendirin!")
                    {
                        return (false, $"Продуктът с IDDonanim '{idDonanim}' НЕ Е минал през предходните станции. Моля, изпратете го първо към предходната станция!");
                    }
                    
                    if (idBak.Message == "Donanimi")
                    {
                        return (false, $"Продуктът с IDDonanim '{idDonanim}' не е готов за тази станция");
                    }
                }
                
                // 5. Всички проверки минаха успешно
                return (true, $"IDDonanim '{idDonanim}' е валиден и готов за обработка");
            }
            catch (Exception ex)
            {
                return (false, $"Грешка при валидация: {ex.Message}");
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
                    
                    Dictionary<string, string> screenshotContext = new Dictionary<string, string>
                    {
                        { "ScreenshotName", SensitiveDataMasker.MaskPath(lastScreenshotPath) }
                    };
                    ticketLogger.LogInfo("ManualScreenshotCaptured", screenshotContext);
                }
                // 2. Пращаме тикета (логиката е същата като в ButtonCreateTicket_Click)
                //CreateTicket();
                // MessageBox.Show("Тикетът и скрийншотът са изпратени автоматично!", "IT тикет", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorContext = new Dictionary<string, string>
                {
                    { "Message", ex.Message }
                };
                ticketLogger.LogError("ManualScreenshotFailure", errorContext);
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
                Dictionary<string, string> startContext = new Dictionary<string, string>
                {
                    { "TicketName", SensitiveDataMasker.MaskValue(tiketName) },
                    { "Role", roleValue.ToString() },
                    { "ScreenshotName", SensitiveDataMasker.MaskPath(lastScreenshotPath) }
                };
                ticketLogger.LogInfo("ManualTicketStart", startContext);

                // Първо пращаме тикета
                var (success, serverTicketId) = await _systemTicket.CreateTicket(tiketName, description, lastScreenshotPath, roleValue);
                Dictionary<string, string> resultContext = new Dictionary<string, string>
                {
                    { "Success", success.ToString() },
                    { "TicketId", serverTicketId ?? string.Empty }
                };
                ticketLogger.LogInfo("ManualTicketResult", resultContext);
                
                if (success)
                {
                    string serverIp = XMLSeverIp.XmlWebApiIP();
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
                    MessageBox.Show("Грешка при изпращане на тикета!", "Грешка", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Грешка при създаване на тикет: {ex.Message}", "Грешка", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Автоматична система за тикети при crash

        /// <summary>
        /// Global exception handler за Thread exceptions
        /// </summary>
        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception, "Thread Exception в ElTest");
        }

        /// <summary>
        /// Global exception handler за Unhandled exceptions
        /// </summary>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                HandleException(ex, "Unhandled Exception в ElTest");
            }
        }

        /// <summary>
        /// Централен метод за обработка на грешки
        /// </summary>
        private void HandleException(Exception ex, string context)
        {
            try
            {
                // Прави screenshot на целия екран
                string screenshotPath = TakeScreenshot();

                // Създава детайлно съобщение за грешката
                string errorDetails = $@"
                    ГРЕШКА В ФОРМА: ElTest
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

                Dictionary<string, string> autoContext = new Dictionary<string, string>
                {
                    { "Context", SensitiveDataMasker.MaskValue(context) },
                    { "ScreenshotName", SensitiveDataMasker.MaskPath(screenshotPath) }
                };
                ticketLogger.LogError("AutoTicketTriggered", autoContext);

                Task.Run(async () =>
                {
                    await SendAutoTicketToIT(
                        $"AUTO CRASH: ElTest - {context}",
                        errorDetails,
                        screenshotPath,
                        1); // Role = 1 (IT)
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
                string filename = $"CRASH_ElTest_{timestamp}.jpg";
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
                string bolge = Environment.MachineName;
                Dictionary<string, string> startContext = new Dictionary<string, string>
                {
                    { "TicketName", SensitiveDataMasker.MaskValue(tiketName) },
                    { "Bolge", SensitiveDataMasker.MaskValue(bolge) },
                    { "ScreenshotName", SensitiveDataMasker.MaskPath(screenshotPath) },
                    { "Role", role.ToString() }
                };
                ticketLogger.LogInfo("AutoTicketSendStart", startContext);

                (bool success, string ticketId) = await _systemTicket.CreateTicket(
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

    }
}
