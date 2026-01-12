using Nursan.Business.Services;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Domain.VideoModels;
using Nursan.Logging.Messages;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Personal.Valadation;
using Nursan.UI.Library;
using Nursan.UI.OzelClasslar;
using Nursan.UI.Services;
using Nursan.Validations.ProcessServise.ProcesManager;
using Nursan.Validations.ValidationCode;
using System.IO.Ports;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.IO;


namespace Nursan.UI
{
    public partial class Gromet : Form
    {
        private static UnitOfWork _repo;
        private static OpMashin _makine;
        private static UrVardiya _vardiya;
        private static List<UrIstasyon> _istasyonList;
        private static List<SyBarcodeInput> _syBarcodeInputList;
        //private static List<UrModulerYapi> _modulerYapiList;
        //private static List<SyBarcodeOut> _syBarcodeOutList;
        //private static List<SyPrinter> _syPrinterList;
        //private static List<OrFamily> _familyList;
        //SyBarcodeInput BarcodeInput = new SyBarcodeInput();
        //string pfbSerial;
        //SerialPort _serialPort;
        //BarcodeValidation barcode;
        //int brtSayi = 0;
        ////List<SyBarcodeInput> Barcode = new List<SyBarcodeInput>();
        //ProcessServices process;
        private List<UrPersonalTakib> urPersonalTakibs;
        CountDegerValidations _countDegerValidations;
        int pi;
        static int brcodeVCount;
        Proveri proveri = new Proveri();
        PersonalValidasyonu personal;
        TorkService tork;
        UrIstasyon _istasyon;
        System.Windows.Forms.Timer timer;
        VideoApiService _videoApiService;
        // WebBrowser? _videoBrowser;
        
        // Кеширане на последния баркод и видео за да избегнем ненужни API заявки
        private string? _lastBarcode;
        private Video? _lastVideo;
        private string? _lastVideoUrl;
        public Gromet(UnitOfWork repo, OpMashin makine, UrVardiya vardiya, List<UrIstasyon> istasyonList, List<UrModulerYapi> modulerYapiList, List<SyBarcodeInput> syBarcodeInputList, List<SyBarcodeOut> syBarcodeOutList, List<SyPrinter> syPrinterList, List<OrFamily> familyList)
        {

            //_syBarcodeOutList = syBarcodeOutList;
            //_syPrinterList = syPrinterList;
            //_familyList = familyList;
            //process = new ProcessServices();
            //brtSayi = brcodeVCount;
            //_modulerYapiList = modulerYapiList;
            _repo = repo;
            _makine = makine;
            _vardiya = vardiya;
            _istasyonList = istasyonList;
            _syBarcodeInputList = syBarcodeInputList;

            brcodeVCount = _syBarcodeInputList.Count;
            _countDegerValidations = new CountDegerValidations(_repo, _makine, _vardiya, _istasyonList);
            Form.CheckForIllegalCrossThreadCalls = false;
            personal = new PersonalValidasyonu(new UnitOfWorPersonal(new PersonalContext()), _repo);
            tork = new TorkService(repo, vardiya);
            _istasyon = istasyonList.FirstOrDefault(x => x.MashinId == _makine.Id);
            _videoApiService = new VideoApiService();

            // Инициализираме WebBrowser за видео плеър
            InitializeVideoPlayer();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 3 * 1000;
            timer.Tick += new System.EventHandler(GetTikket);
            timer.Start();
            InitializeComponent();
            
            // Добавяме event handler за Windows Media Player за да засичаме кога видеото свършва
            if (axWindowsMediaPlayer1 != null)
            {
                axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
            }
        }

        private void GetTikket(object? sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            GetCounts();
            GetPersonal();
            StaringAP frm = new StaringAP();
            Thread.Sleep(500);
            frm.Dispose();
            frm.Close();
        }

        private async void txtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            DateTime date = OtherTools.GetValuesDatetime();
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarcode.Text.StartsWith("*"))
                {
                    try
                    {
                        string sicilPersonal = txtBarcode.Text.Substring(1);
                        var personalResult = personal.GetPersonal(sicilPersonal).Data;
                        var personalTakipResult = personal.GetPersonalAndSicilTakibTek(sicilPersonal).Data;
                        Messaglama.MessagYaz(sicilPersonal);
                        if (personalTakipResult == null)
                        {
                            Messaglama.MessagYaz(personalResult.FIRST_NAME);
                            //Messaglama.MessagYaz(personalTakipResult.FullName);
                            personal.ADDPersonalTakib(new UrPersonalTakib
                            {
                                Sicil = sicilPersonal,
                                FullName = $"{personalResult.FIRST_NAME} {personalResult.LAST_NAME}",
                                UrIstasyonId = _istasyon.Id,
                                DayOfYear = $"{_istasyon.Id}*{date.Year}{date.Month}{date.Day}",
                                CreateDate = date,
                                UpdateDate = date
                            });
                        }
                        else if (personalTakipResult != null && personalTakipResult.UrIstasyonId != _istasyon.Id)
                        {
                            Messaglama.MessagYaz(personalTakipResult.FullName + "H");
                            personal.UpdatePersonalTakib(new UrPersonalTakib
                            {
                                Id = personalTakipResult.Id,
                                Sicil = sicilPersonal,
                                FullName = $"{personalResult.FIRST_NAME} {personalResult.LAST_NAME}",
                                UrIstasyonId = _istasyon.Id,
                                DayOfYear = $"{_istasyon.Id}*{date.Year}{date.Month}{date.Day}",
                                UpdateDate = date
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                if (txtBarcode.Text.Length != 10)
                {
                    if (_vardiya.Name != txtBarcode.Text)
                    {
                        // ВАЖНО: Проверяваме дали pi е в границите преди достъп
                        if (pi < 0 || pi >= _syBarcodeInputList.Count)
                        {
                            proveri.MessageAyarla($"Грешка: Индекс извън границите (pi={pi}, Count={_syBarcodeInputList.Count})", Color.Red, lblMessage);
                            txtBarcode.Clear();
                            pi = 0;
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
                            if (veri.Success)
                            {
                                // ВАЖНО: Проверяваме дали pi е в границите преди достъп
                                // pi вече е увеличено, така че използваме pi - 1 за последния елемент
                                if (pi > 0 && pi <= _syBarcodeInputList.Count)
                                {
                                    var lastBarcode = _syBarcodeInputList[pi - 1];
                                    if (lastBarcode != null && !string.IsNullOrEmpty(lastBarcode.BarcodeIcerik))
                                    {
                                        await LoadVideoByBarcode(lastBarcode.BarcodeIcerik);
                                    }
                                }
                            }
                            listBox1.Items.Clear();
                            for (int i = 0; i < pi && i < _syBarcodeInputList.Count; i++)
                            {
                                _syBarcodeInputList[i].BarcodeIcerik = null;
                            }
                            pi = 0;
                            proveri.MessageAyarla($"{veri.Message} {txtBarcode.Text} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage);

                            // Ако баркодът е успешно обработен, опитваме се да заредим видео
                        }
                        //proveri.MessageAyarla($"{veri.Message} {txtBarcode.Text} ", veri.Success == true ? Color.LightBlue : Color.Red, lblMessage);
                        txtBarcode.Clear();
                        GetCounts(); GetPersonal();
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


        }

        private void GetPersonal()
        {
            listBox2.Items.Clear();
            urPersonalTakibs = personal.GetPersonalTakib(_istasyon).Data;
            var veriler = urPersonalTakibs.Count() == 0 ? null : urPersonalTakibs;
            Messaglama.MessagYaz(_istasyon.Id.ToString());
            if (veriler != null)
            {
                //string[] parca = veriler.First().DayOfYear.Split('*');
                foreach (var item in urPersonalTakibs)
                {
                    //Messaglama.MessagYaz($"{item.Sicil}-{item.FullName}-{GitSytemDeAyiklaVesay(item.Sicil)}");
                    listBox2.Items.Add($"{item.Sicil}-{item.FullName}-{GitSytemDeAyiklaVesay(item.Sicil)}");
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
        private int GitSytemDeAyiklaVesay(string? sicil)
        {
            var result = SayiIzlemeSIcilBagizliService.SayiHesapla(sicil, _vardiya.Name);
            return ((int)_istasyonList.First().Realadet + result);
        }

        /// <summary>
        /// Инициализира видео плеър (WebBrowser контрол)
        /// </summary>
        private void InitializeVideoPlayer()
        {
            //try
            //{
            //    _videoBrowser = new WebBrowser
            //    {
            //        Dock = DockStyle.Fill,
            //        IsWebBrowserContextMenuEnabled = false,
            //        WebBrowserShortcutsEnabled = false,
            //        ScriptErrorsSuppressed = true,
            //        AllowWebBrowserDrop = false
            //    };

            //    // Забраняваме сваляне на файлове - обработваме Navigating event
            //    _videoBrowser.Navigating += VideoBrowser_Navigating;

            //    // Обработваме DocumentCompleted за да предотвратим сваляне
            //    _videoBrowser.DocumentCompleted += VideoBrowser_DocumentCompleted;

            //    // Добавяме WebBrowser в panel2
            //    if (panel2 != null)
            //    {
            //        panel2.Controls.Add(_videoBrowser);
            //        _videoBrowser.Dock = DockStyle.Fill;
            //        _videoBrowser.Visible = false; // Скриваме го докато няма видео
            //        _videoBrowser.BringToFront();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Messaglama.MessagException($"Грешка при инициализация на видео плеър: {ex.Message}");
            //}
        }

        /// <summary>
        /// Обработва Navigating event за да предотвратим сваляне на файлове
        /// </summary>
        private void VideoBrowser_Navigating(object? sender, WebBrowserNavigatingEventArgs e)
        {
            // Ако се опитва да навигира към видео файл директно (не през HTML5 video tag),
            // предотвратяваме го и използваме HTML5 video tag вместо това
            if (e.Url != null &&
                (e.Url.AbsolutePath.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ||
                 e.Url.AbsolutePath.EndsWith(".webm", StringComparison.OrdinalIgnoreCase) ||
                 e.Url.AbsolutePath.EndsWith(".avi", StringComparison.OrdinalIgnoreCase) ||
                 e.Url.AbsolutePath.Contains("/file", StringComparison.OrdinalIgnoreCase)))
            {
                // Не предотвратяваме, защото искаме да стриймваме през HTML5 video tag
                // Но ако се опитва директно да свали файла, ще го предотвратим
            }
        }

        /// <summary>
        /// Обработва DocumentCompleted event
        /// </summary>
        private void VideoBrowser_DocumentCompleted(object? sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //try
            //{
            //    // Осигуряваме че видеото се стриймва, не сваля
            //    if (_videoBrowser?.Document != null)
            //    {
            //        // Добавяме JavaScript за предотвратяване на сваляне
            //        var script = @"
            //            if (document.getElementById('videoPlayer')) {
            //                var video = document.getElementById('videoPlayer');
            //                video.addEventListener('error', function(e) {
            //                    console.error('Video error:', video.error);
            //                });
            //            }
            //        ";

            //        var head = _videoBrowser.Document.GetElementsByTagName("head")[0];
            //        var scriptEl = _videoBrowser.Document.CreateElement("script");
            //        scriptEl.SetAttribute("type", "text/javascript");
            //        scriptEl.SetAttribute("text", script);
            //        head.AppendChild(scriptEl);
            //    }
            //}
            //catch
            //{
            //    // Игнорираме грешки при добавяне на JavaScript
            //}
        }

        private void Panel2_Paint(object? sender, PaintEventArgs e)
        {
            // Рисуване на червена граница около panel2 (видео областта)
            if (sender is Panel panel)
            {
                using (Pen redPen = new Pen(Color.Red, 2))
                {
                    // Рисуване на горна, дясна и долна граница (като на снимката)
                    e.Graphics.DrawLine(redPen, 0, 0, panel.Width, 0); // Горна
                    e.Graphics.DrawLine(redPen, panel.Width - 1, 0, panel.Width - 1, panel.Height); // Дясна
                    e.Graphics.DrawLine(redPen, 0, panel.Height - 1, panel.Width, panel.Height - 1); // Долна
                }
            }
        }

        /// <summary>
        /// Event handler за Windows Media Player - засича кога видеото свършва
        /// </summary>
        private void AxWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            try
            {
                // WMPPlayState enum: 0=Undefined, 1=Stopped, 2=Paused, 3=Playing, 4=ScanForward, 5=ScanReverse, 
                // 6=Buffering, 7=MediaEnded, 8=Transitioning, 9=Ready, 10=Reconnecting, 11=Last
                
                // Когато видеото свърши (MediaEnded = 8), го плейваме отново
                if (e.newState == 8) // MediaEnded
                {
                    if (axWindowsMediaPlayer1 != null && !string.IsNullOrEmpty(_lastVideoUrl))
                    {
                        // Плейваме видеото отново (loop)
                        axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        System.Diagnostics.Debug.WriteLine("LoadVideo: Видеото свърши, плейваме отново (loop)");
                    }
                }
            }
            catch (Exception ex)
            {
                // Игнорираме грешки при обработка на PlayStateChange
                System.Diagnostics.Debug.WriteLine($"AxWindowsMediaPlayer1_PlayStateChange грешка: {ex.Message}");
            }
        }

        /// <summary>
        /// Зарежда видео по баркод референса
        /// </summary>
        private async Task LoadVideoByBarcode(string barcodeText)
        {
            try
            {
                // Вземаме първия баркод от списъка (input баркод)
                if (_syBarcodeInputList == null || _syBarcodeInputList.Count == 0)
                    return;

                var firstBarcode = _syBarcodeInputList.FirstOrDefault();
                if (firstBarcode == null || string.IsNullOrEmpty(firstBarcode.BarcodeIcerik))
                    return;

                // ВАЖНО: Проверяваме дали баркодът е същият като предишния
                // Ако е същият, не правим нова API заявка, а плейваме същото видео отново
                string currentBarcode = firstBarcode.BarcodeIcerik;
                if (!string.IsNullOrEmpty(_lastBarcode) && _lastBarcode.Equals(currentBarcode, StringComparison.OrdinalIgnoreCase))
                {
                    // Баркодът е същият - плейваме същото видео отново без нова API заявка
                    if (_lastVideo != null && !string.IsNullOrEmpty(_lastVideoUrl))
                    {
                        System.Diagnostics.Debug.WriteLine($"LoadVideoByBarcode: Същият баркод ({currentBarcode}), плейваме същото видео отново");
                        await LoadVideo(_lastVideo, skipApiCall: true);
                        return;
                    }
                }

                // Извличаме заглавието за търсене от BarcodeIcerik
                // Формат: prefix-family-suffix_... или prefix-family-suffix
                string searchTitle = firstBarcode.BarcodeIcerik;

                // Ако баркодът е в формат "prefix-family-suffix_...", извличаме само харнес модела
                string[] mainParts = firstBarcode.BarcodeIcerik.Split('_');
                if (mainParts.Length > 0)
                {
                    // Извличаме харнес модела (първата част преди '_')
                    string harnessModel = mainParts[0];

                    // Разделяме на prefix-family-suffix
                    string[] harnessParts = harnessModel.Split('-');
                    if (harnessParts.Length >= 3)
                    {
                        // Премахваме цифри и специални символи от suffix (като в GitBarcodeTorkBak)
                        string suffix = System.Text.RegularExpressions.Regex.Replace(harnessParts[2], "[^a-z,A-Z,@,^,/,]", "");

                        // Използваме prefix, family и suffix за търсене (напр. "8D 401Q16-428" или "8D-401Q16-ABC")
                        searchTitle = $"{harnessParts[0]}-{harnessParts[1]}-{suffix}";
                    }
                    else if (harnessParts.Length >= 2)
                    {
                        // Ако няма suffix, използваме само prefix и family
                        searchTitle = $"{harnessParts[0]} {harnessParts[1]}";
                    }
                    else
                    {
                        // Ако няма достатъчно части, използваме целия харнес модел
                        searchTitle = harnessModel;
                    }
                }

                // Търсим видео в API-то по заглавие (prefix-family-suffix)
                var videos = await _videoApiService.SearchVideosByTitleAsync(searchTitle);

                if (videos != null && videos.Count > 0)
                {
                    // Вземаме първото активно видео (вече са сортирани по дата в API-то)
                    var video = videos.FirstOrDefault(v => v.IsActive && !string.IsNullOrEmpty(v.VideoUrl));

                    if (video != null)
                    {
                        // Запазваме текущия баркод и видео за кеширане
                        _lastBarcode = currentBarcode;
                        _lastVideo = video;
                        
                        // Зареждаме видеото
                        await LoadVideo(video);
                    }

                }

            }
            catch (Exception ex)
            {
                // Тиха обработка на грешки - не прекъсваме основния процес
                Messaglama.MessagException($"Грешка при зареждане на видео: {ex.Message}");
            }
        }

        /// <summary>
        /// Зарежда видео файл в видео плеъра
        /// </summary>
        /// <param name="video">Видеото за зареждане</param>
        /// <param name="skipApiCall">Ако е true, използва кеширания URL вместо да го генерира отново</param>
        private async Task LoadVideo(Video video, bool skipApiCall = false)
        {
            try
            {
                // Използваме Windows Media Player за стрийминг (най-добрият вариант за WinForms)
                if (axWindowsMediaPlayer1 != null)
                {
                    // Debug: Логваме че използваме Windows Media Player
                    System.Diagnostics.Debug.WriteLine($"LoadVideo: Използваме Windows Media Player за видео ID: {video.Id}");

                    // Ако видеото е различно от предишното, спираме текущото и освобождаваме паметта
                    if (!string.IsNullOrEmpty(_lastVideoUrl) && _lastVideoUrl != _videoApiService.GetVideoFileUrl(video.Id))
                    {
                        try
                        {
                            axWindowsMediaPlayer1.Ctlcontrols.stop();
                            axWindowsMediaPlayer1.URL = string.Empty; // Освобождаваме паметта
                            System.Diagnostics.Debug.WriteLine("LoadVideo: Спирано предишно видео и освободена памет");
                        }
                        catch
                        {
                            // Игнорираме грешки при спиране
                        }
                    }

                    // Показваме Windows Media Player
                    axWindowsMediaPlayer1.Visible = true;
                    axWindowsMediaPlayer1.BringToFront();

                    // Настройваме Windows Media Player
                    axWindowsMediaPlayer1.uiMode = "none"; // Скриваме UI, показваме само видео
                    axWindowsMediaPlayer1.stretchToFit = true; // Разтягаме видеото да запълни пространството
                    axWindowsMediaPlayer1.enableContextMenu = false; // Забраняваме контекстното меню

                    // Използваме видео API endpoint за стрийминг
                    string videoFileUrl1;
                    if (skipApiCall && !string.IsNullOrEmpty(_lastVideoUrl))
                    {
                        // Използваме кеширания URL
                        videoFileUrl1 = _lastVideoUrl;
                        System.Diagnostics.Debug.WriteLine($"LoadVideo: Използваме кеширан URL: {videoFileUrl1}");
                    }
                    else
                    {
                        // Генерираме нов URL
                        videoFileUrl1 = _videoApiService.GetVideoFileUrl(video.Id);
                        _lastVideoUrl = videoFileUrl1; // Кешираме URL-а
                    }

                    // ВАЖНО: Оставяме URL-а както е (HTTPS или HTTP)
                    // За HTTPS да работи без диалог, SSL сертификатът трябва да е инсталиран в Windows Trusted Root Certificate Authorities
                    // Инструкции: Отворете HTTPS URL в браузър → Преглед на сертификата → Копиране във файл → Инсталиране в Trusted Root Certification Authorities

                    // Debug: Логваме URL-а
                    System.Diagnostics.Debug.WriteLine($"LoadVideo: Video URL: {videoFileUrl1}");

                    // Задаваме URL-а на видеото (Windows Media Player автоматично стриймва от URL)
                    axWindowsMediaPlayer1.URL = videoFileUrl1;

                    // Започваме възпроизвеждане
                    axWindowsMediaPlayer1.Ctlcontrols.play();

                    // Debug: Логваме че сме стартирали възпроизвеждането
                    System.Diagnostics.Debug.WriteLine($"LoadVideo: Стартирано възпроизвеждане");

                    proveri.MessageAyarla($"Възпроизвеждане: {video.Title}", Color.LightGreen, lblMessage);
                    return;
                }

                // Fallback: Ако Windows Media Player не е наличен, използваме WebBrowser
                // Използваме видео API endpoint за стрийминг на файла
                // Endpoint: /api/Videos/{id}/file - връща видео файла за стрийминг
                // Този endpoint поддържа Range requests (HTTP 206 Partial Content) за стрийминг
                string videoFileUrl = _videoApiService.GetVideoFileUrl(video.Id);

                try
                {
                    if (panel2 != null)
                    {
                        panel2.Visible = true;
                        panel2.BringToFront();
                    }

                    // Очакваме малко за да се зареди HTML-а
                    await Task.Delay(1000);


                }
                catch (Exception ex)
                {
                    proveri.MessageAyarla($"Грешка при зареждане на видео: {ex.Message}", Color.Red, lblMessage);

                }
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = videoFileUrl,
                        UseShellExecute = true
                    });

                    proveri.MessageAyarla($"Видео отворено: {video.Title}", Color.LightGreen, lblMessage);
                }
                catch (Exception ex)
                {
                    proveri.MessageAyarla($"Грешка при отваряне на видео: {ex.Message}", Color.Red, lblMessage);

                }
            }
            catch (Exception ex)
            {
                proveri.MessageAyarla($"Грешка при зареждане на видео: {ex.Message}", Color.Red, lblMessage);
                //await LoadPDF(); // Fallback към PDF
            }
        }

        private async void lblToplam_Click(object sender, EventArgs e)
        {
            var videos = await _videoApiService.SearchVideosByTitleAsync("8D 401Q16-428");

            if (videos != null && videos.Count > 0)
            {
                // Вземаме първото активно видео (вече са сортирани по дата в API-то)
                var video = videos.FirstOrDefault(v => v.IsActive && !string.IsNullOrEmpty(v.VideoUrl));

                if (video != null)
                {
                    // Зареждаме видеото
                    await LoadVideo(video);
                }

            }
        }
    }
}