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
        WebBrowser? _videoBrowser;
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
        }

        private void GetTikket(object? sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            await GetPDF();
            GetCounts();
            GetPersonal();
            StaringAP frm = new StaringAP();
            Thread.Sleep(500);
            frm.Dispose();
            frm.Close();
        }

        private async Task GetPDF()
        {
            // Initialize Windows Media Player
            // Ще се разкоментира след генериране на interop DLL-ите (AxWMPLib.dll и WMPLib.dll)
            /*
            if (axWindowsMediaPlayer1 != null)
            {
                try
                {
                    axWindowsMediaPlayer1.uiMode = "none";
                    axWindowsMediaPlayer1.stretchToFit = true;
                    axWindowsMediaPlayer1.enableContextMenu = false;
                    
                    // Зареждане на видео файл
                    string mediaPath = $"{Environment.CurrentDirectory}\\media.mp4";
                    if (System.IO.File.Exists(mediaPath))
                    {
                        axWindowsMediaPlayer1.URL = mediaPath;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                    }
                    else
                    {
                        // Ако няма видео, показваме PDF
                        await LoadPDF();
                    }
                }
                catch (Exception ex)
                {
                    // При грешка с видео, опитваме се да заредим PDF
                    await LoadPDF();
                }
            }
            else
            {
                await LoadPDF();
            }
            */

            // За сега показваме само PDF
            await LoadPDF();
        }

        private async Task LoadPDF()
        {
            // Скриваме видео плеър ако е видим
            if (_videoBrowser != null)
            {
                _videoBrowser.Visible = false;
                _videoBrowser.Hide();
                _videoBrowser.SendToBack();
            }

            // Зареждане на PDF viewer ако няма видео
            if (axAcropdf1 != null)
            {
                try
                {
                    string veri = $"{Environment.CurrentDirectory}\\pdfdoc.pdf";
                    if (System.IO.File.Exists(veri))
                    {
                        axAcropdf1.src = veri;
                        axAcropdf1.Dock = DockStyle.Fill;
                        axAcropdf1.setShowToolbar(false);
                        axAcropdf1.Show();
                        axAcropdf1.BringToFront();
                    }
                }
                catch (Exception)
                {
                    if (axAcropdf1 != null)
                    {
                        axAcropdf1.Dispose();
                    }
                }
            }
        }

        private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
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

                            // Ако баркодът е успешно обработен, опитваме се да заредим видео
                            if (veri.Success)
                            {
                                LoadVideoByBarcode(txtBarcode.Text);
                            }
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
            try
            {
                _videoBrowser = new WebBrowser
                {
                    Dock = DockStyle.Fill,
                    IsWebBrowserContextMenuEnabled = false,
                    WebBrowserShortcutsEnabled = false,
                    ScriptErrorsSuppressed = true,
                    AllowWebBrowserDrop = false
                };

                // Забраняваме сваляне на файлове - обработваме Navigating event
                _videoBrowser.Navigating += VideoBrowser_Navigating;

                // Обработваме DocumentCompleted за да предотвратим сваляне
                _videoBrowser.DocumentCompleted += VideoBrowser_DocumentCompleted;

                // Добавяме WebBrowser в panel2
                if (panel2 != null)
                {
                    panel2.Controls.Add(_videoBrowser);
                    _videoBrowser.Dock = DockStyle.Fill;
                    _videoBrowser.Visible = false; // Скриваме го докато няма видео
                    _videoBrowser.BringToFront();
                }
            }
            catch (Exception ex)
            {
                Messaglama.MessagException($"Грешка при инициализация на видео плеър: {ex.Message}");
            }
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
            try
            {
                // Осигуряваме че видеото се стриймва, не сваля
                if (_videoBrowser?.Document != null)
                {
                    // Добавяме JavaScript за предотвратяване на сваляне
                    var script = @"
                        if (document.getElementById('videoPlayer')) {
                            var video = document.getElementById('videoPlayer');
                            video.addEventListener('error', function(e) {
                                console.error('Video error:', video.error);
                            });
                        }
                    ";

                    var head = _videoBrowser.Document.GetElementsByTagName("head")[0];
                    var scriptEl = _videoBrowser.Document.CreateElement("script");
                    scriptEl.SetAttribute("type", "text/javascript");
                    scriptEl.SetAttribute("text", script);
                    head.AppendChild(scriptEl);
                }
            }
            catch
            {
                // Игнорираме грешки при добавяне на JavaScript
            }
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
        /// Зарежда видео по баркод референса
        /// </summary>
        private async void LoadVideoByBarcode(string barcodeText)
        {
            try
            {
                // Вземаме първия баркод от списъка (input баркод)
                if (_syBarcodeInputList == null || _syBarcodeInputList.Count == 0)
                    return;

                var firstBarcode = _syBarcodeInputList.FirstOrDefault();
                if (firstBarcode == null || string.IsNullOrEmpty(firstBarcode.BarcodeIcerik))
                    return;

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
                        searchTitle = $"{harnessParts[0]} {harnessParts[1]}-{suffix}";
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
                        // Зареждаме видеото
                        await LoadVideo(video);
                    }
                    else
                    {
                        // Ако няма активно видео, fallback към PDF
                        await LoadPDF();
                    }
                }
                else
                {
                    // Ако няма намерени видеа, fallback към PDF
                    await LoadPDF();
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
        private async Task LoadVideo(Video video)
        {
            try
            {
                // Използваме Windows Media Player за стрийминг (най-добрият вариант за WinForms)
                if (axWindowsMediaPlayer1 != null)
                {
                    // Скриваме PDF viewer и WebBrowser ако са видими
                    if (axAcropdf1 != null)
                    {
                        axAcropdf1.Hide();
                        axAcropdf1.SendToBack();
                    }
                    
                    if (_videoBrowser != null)
                    {
                        _videoBrowser.Hide();
                        _videoBrowser.SendToBack();
                    }
                    
                    // Показваме Windows Media Player
                    axWindowsMediaPlayer1.Visible = true;
                    axWindowsMediaPlayer1.BringToFront();
                    
                    // Настройваме Windows Media Player
                    axWindowsMediaPlayer1.uiMode = "none"; // Скриваме UI, показваме само видео
                    axWindowsMediaPlayer1.stretchToFit = true; // Разтягаме видеото да запълни пространството
                    axWindowsMediaPlayer1.enableContextMenu = false; // Забраняваме контекстното меню
                    
                    // Използваме видео API endpoint за стрийминг
                    string videoFileUrl1 = _videoApiService.GetVideoFileUrl(video.Id);
                    
                    // Задаваме URL-а на видеото (Windows Media Player автоматично стриймва от URL)
                    axWindowsMediaPlayer1.URL = videoFileUrl1;
                    
                    // Започваме възпроизвеждане
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    
                    proveri.MessageAyarla($"Възпроизвеждане: {video.Title}", Color.LightGreen, lblMessage);
                    return;
                }

                // Fallback: Ако Windows Media Player не е наличен, използваме WebBrowser
                // Използваме видео API endpoint за стрийминг на файла
                // Endpoint: /api/Videos/{id}/file - връща видео файла за стрийминг
                // Този endpoint поддържа Range requests (HTTP 206 Partial Content) за стрийминг
                string videoFileUrl = _videoApiService.GetVideoFileUrl(video.Id);

                // Показваме видеото във вградения WebBrowser плеър (fallback)
                if (_videoBrowser != null && panel2 != null)
                {
                    try
                    {
                        // Скриваме PDF viewer ако е видим
                        if (axAcropdf1 != null)
                        {
                            axAcropdf1.Hide();
                            axAcropdf1.SendToBack();
                        }
                        
                        // Уверяваме се че WebBrowser е видим и на преден план
                        _videoBrowser.Visible = true;
                        _videoBrowser.BringToFront();
                        _videoBrowser.Dock = DockStyle.Fill;
                        
                        // WebBrowser в WinForms използва IE engine
                        // За да предотвратим сваляне на файл, използваме HTML5 video tag
                        // с правилни настройки за стрийминг (preload='metadata' или 'none')
                        // и използваме object-fit за правилно показване

                        // Проблем: WebBrowser (IE engine) може да сваля файла вместо да го стриймва
                        // Решение: Използваме HTML5 video tag с правилни настройки за стрийминг
                        // и осигуряваме че endpoint-ът поддържа Range requests (HTTP 206)
                        
                        // Важно: 
                        // 1. Използваме preload='metadata' за да започне стрийминг веднага
                        // 2. Endpoint-ът /api/Videos/{id}/file трябва да поддържа Range requests
                        // 3. Endpoint-ът трябва да връща правилни headers (Content-Type, Accept-Ranges)
                        
                        // Debug: Показваме URL-а в съобщението
                        proveri.MessageAyarla($"Зареждане на видео: {video.Title}...", Color.Yellow, lblMessage);
                        
                        string htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        html, body {{
            width: 100%;
            height: 100%;
            background-color: #2d2d2d;
            overflow: hidden;
        }}
        video {{
            width: 100%;
            height: 100%;
            object-fit: contain;
            background-color: #000;
        }}
    </style>
</head>
<body>
    <video id='videoPlayer' controls autoplay preload='metadata' playsinline>
        <source src='{videoFileUrl}' type='video/mp4'>
        Вашият браузър не поддържа HTML5 video.
    </video>
    <script>
        (function() {{
            var video = document.getElementById('videoPlayer');
            
            // Предотвратяваме сваляне на файл при кликване с десен бутон
            document.addEventListener('contextmenu', function(e) {{
                e.preventDefault();
                return false;
            }}, false);
            
            // Осигуряваме стрийминг режим - не сваляне
            if (video) {{
                // Принудително задаваме preload='metadata' за стрийминг
                // Това казва на браузъра да зареди само метаданните първо
                video.preload = 'metadata';
                
                // Обработваме грешки
                video.addEventListener('error', function(e) {{
                    console.error('Video loading error:', video.error ? video.error.code : 'unknown');
                }}, false);
                
                // Когато има достатъчно данни, започваме възпроизвеждане
                video.addEventListener('loadedmetadata', function() {{
                    console.log('Video metadata loaded, starting playback');
                    // Започваме възпроизвеждане веднага след зареждане на метаданните
                    video.play().catch(function(err) {{
                        console.error('Play error:', err);
                    }});
                }}, false);
                
                // Предотвратяваме сваляне - използваме стрийминг
                video.addEventListener('loadstart', function() {{
                    console.log('Video loading started (streaming mode)');
                }}, false);
                
                // Когато има достатъчно данни за възпроизвеждане
                video.addEventListener('canplay', function() {{
                    console.log('Video can start playing (streaming)');
                }}, false);
            }}
        }})();
    </script>
</body>
</html>";
                        
                        // Зареждаме HTML в WebBrowser
                        // WebBrowser ще използва HTML5 video tag който автоматично
                        // ще изпраща Range requests (HTTP 206) за стрийминг
                        _videoBrowser.DocumentText = htmlContent;
                        
                        // Уверяваме се че WebBrowser е видим и на преден план
                        _videoBrowser.Visible = true;
                        _videoBrowser.Show();
                        _videoBrowser.BringToFront();
                        _videoBrowser.Refresh();
                        
                        // Уверяваме се че panel2 е видим
                        if (panel2 != null)
                        {
                            panel2.Visible = true;
                            panel2.BringToFront();
                        }
                        
                        // Очакваме малко за да се зареди HTML-а
                        await Task.Delay(1000);
                        
                        // Проверяваме дали видеото се зарежда
                        try
                        {
                            if (_videoBrowser.Document != null && _videoBrowser.Document.Body != null)
                            {
                                var videoElement = _videoBrowser.Document.GetElementById("videoPlayer");
                                if (videoElement != null)
                                {
                                    proveri.MessageAyarla($"Видео заредено: {video.Title}", Color.LightGreen, lblMessage);
                                }
                                else
                                {
                                    // Ако няма video елемент, опитваме се с fallback
                                    proveri.MessageAyarla($"Видео елемент не е намерен, опитвам се с fallback...", Color.Orange, lblMessage);
                                    await Task.Delay(1000);
                                    proveri.MessageAyarla($"Възпроизвеждане: {video.Title}", Color.LightGreen, lblMessage);
                                }
                            }
                            else
                            {
                                proveri.MessageAyarla($"Възпроизвеждане: {video.Title}", Color.LightGreen, lblMessage);
                            }
                        }
                        catch (Exception docEx)
                        {
                            // Ако има проблем с Document, все пак показваме съобщение
                            proveri.MessageAyarla($"Възпроизвеждане: {video.Title}", Color.LightGreen, lblMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        proveri.MessageAyarla($"Грешка при зареждане на видео: {ex.Message}", Color.Red, lblMessage);
                        await LoadPDF(); // Fallback към PDF
                    }
                }
                else
                {
                    // Ако няма WebBrowser, опитваме се с default плеър
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
                        await LoadPDF(); // Fallback към PDF
                    }
                }
            }
            catch (Exception ex)
            {
                Messaglama.MessagException($"Грешка при зареждане на видео файл: {ex.Message}");
                await LoadPDF(); // Fallback към PDF при грешка
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
                else
                {
                    // Ако няма активно видео, fallback към PDF
                    await LoadPDF();
                }
            }
        }
    }
}