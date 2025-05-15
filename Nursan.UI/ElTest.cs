using Nursan.Business.Manager;
using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.SortedList;
using Nursan.Validations.ValidationCode;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using XMLIslemi = Nursan.Core.Printing.XMLIslemi;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Linq.Expressions;
using Nursan.Caching;
using Microsoft.Extensions.DependencyInjection;
using Nursan.Caching.DTOs;

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
        static string[] pathc = { "C:\\Kliptest\\", "C:\\Kliptest_2\\", "C:\\Klt\\", "C:\\Kliptest_3\\", "C:\\DEMO\\", "C:\\_Kliptest\\", "C:\\Labels\\" };
        private EltestValidasyonlari _elTest;
        private static string format = $"*.txt";
        private ScreenSaverForm scren;
        private SicilOkuma sicil;
        private UnitOfWork _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly LocalCacheService _localCache;
        private readonly Nursan.Business.Services.ISynchronizationService _syncService;
        private bool _isConnected = true;

        public ElTest(UnitOfWork repo)
        {
            InitializeComponent();
            this._repo = repo;
            _unitOfWork = repo;
            _elTest = new EltestValidasyonlari(repo);
            _localCache = new LocalCacheService();
            //TorkService = new TorkService(repo, new UrVardiya { Name = "A" }, _localCache);
            GitDirektoryBac();
            GitSytemdenSil();
            //scren = new ScreenSaverForm(0, "Info");
            //sicil = new SicilOkuma("A");
            //scren.TetikSicil += Scre_TetikSicil;

            // Получаване на ISynchronizationService от DI контейнера
            var services = new ServiceCollection();
            services.AddSingleton<IUnitOfWork>(_unitOfWork);
            services.AddSingleton<Nursan.Caching.ILocalCache>(_localCache);
            services.AddSingleton<Messaglama>(new Messaglama());
            services.AddScoped<Nursan.Business.Services.ISynchronizationService, Nursan.Business.Services.SynchronizationService>();
            var serviceProvider = services.BuildServiceProvider();
            _syncService = serviceProvider.GetRequiredService<Nursan.Business.Services.ISynchronizationService>();

            this.watcher1 = new FileSystemWatcher(pathc[0], format);
            this.watcher1.Created += new FileSystemEventHandler(Watcher1_Created);
            this.watcher2 = new FileSystemWatcher(pathc[1], format);
            this.watcher2.Created += new FileSystemEventHandler(this.Watcher2_Created);
            this.watcher3 = new FileSystemWatcher(pathc[2], format);
            this.watcher3.Created += new FileSystemEventHandler(this.Watcher3_Created);
            this.Kliptest_3 = new FileSystemWatcher(pathc[3], format);
            this.Kliptest_3.Created += new FileSystemEventHandler(this.KlipTest_3_Created);
            lblVersion.Text = $"ElTest {Environment.Version}";
            LoadModels();
        }

        private void LoadModels()
        {
            try
            {
                // 1. Първо взимаме машината за текущия компютър
                var makineResult = _unitOfWork.GetRepository<OpMashin>().Get(x => x.MasineName == Environment.MachineName);
                if (!makineResult.Success || makineResult.Data == null)
                {
                    Messaglama.MessagYaz("Грешка: Не е намерена машина за този компютър");
                    return;
                }
                // Конвертираме до DTO преди кеширане
                var makineDto = makineResult.Data.ToDto();
                _localCache.CacheModel("CurrentMachine", makineDto);

                // 2. Взимаме станцията за тази машина
                var istasyonResult = _unitOfWork.GetRepository<UrIstasyon>()
                    .Get(x => x.MashinId == makineResult.Data.Id && x.Activ == true);
                if (!istasyonResult.Success || istasyonResult.Data == null)
                {
                    Messaglama.MessagYaz("Грешка: Не са намерени станции за тази машина");
                    return;
                }
                // Конвертираме до DTO преди кеширане
                var istasyonDto = istasyonResult.Data.ToDto();
                _localCache.CacheModel("CurrentIstasyon", istasyonDto);

                // 3. Взимаме активната смяна - първата активна смяна
                var vardiyaResult = _unitOfWork.GetRepository<UrVardiya>().Get(x =>x.Id==istasyonResult.Data.VardiyaId && x.Activ == true);
                if (vardiyaResult.Success && vardiyaResult.Data != null)
                {
                    // Конвертираме до DTO преди кеширане
                    var vardiyaDto = vardiyaResult.Data.ToDto();
                    _localCache.CacheModel("ActiveVardiya", vardiyaDto);
                }

             
                // 4. Взимаме фамилията за тази станция
                if (istasyonResult.Data.FamilyId.HasValue)
                {
                    var familyResult = _unitOfWork.GetRepository<OrFamily>()
                        .Get(x => x.Id == istasyonResult.Data.FamilyId);
                    if (familyResult.Success && familyResult.Data != null)
                    {
                        // Конвертираме до DTO преди кеширане
                        var familyDto = familyResult.Data.ToDto();
                        _localCache.CacheModel("CurrentFamily", familyDto);
                    }
                }

                // 5. Взимаме модулните структури (само активните)
                var modulerYapiResult = _unitOfWork.GetRepository<UrModulerYapi>()
                    .GetAll(x => x.Activ == true);
                if (modulerYapiResult.Success && modulerYapiResult.Data != null)
                {
                    // Конвертираме до DTO преди кеширане
                    var modulerYapiDto = modulerYapiResult.Data.ToDto();
                    _localCache.CacheModel("ModulerYapi", modulerYapiDto);
                }

                // 6. Взимаме генерираните ID-та за тази станция (ограничен брой)
                if (istasyonResult.Data.Id > 0)
                {
                    var generateIdResult = _unitOfWork.GetRepository<IzGenerateId>()
                        .GetAll(x => x.UrIstasyonId == istasyonResult.Data.Id && x.Activ == true);
                    if (generateIdResult.Success && generateIdResult.Data != null)
                    {
                        // Вземаме само последните 50 записа и конвертираме до DTO
                        var limitedData = generateIdResult.Data.OrderByDescending(x => x.Id).Take(200).ToList();
                        var generateIdDto = limitedData.ToDto();
                        _localCache.CacheModel("GenerateId", generateIdDto);
                    }
                }

                // 7. Взимаме броячите за тази станция (ограничен брой)
                if (istasyonResult.Data.Id > 0)
                {
                    var donanimCountResult = _unitOfWork.GetRepository<IzDonanimCount>()
                        .GetAll(x => x.UrIstasyonId == istasyonResult.Data.Id && x.Activ == true);
                    if (donanimCountResult.Success && donanimCountResult.Data != null)
                    {
                        // Вземаме само последните 50 записа и конвертираме до DTO
                        var limitedData = donanimCountResult.Data.OrderByDescending(x => x.Id).Take(200).ToList();
                        var donanimCountDto = limitedData.ToDto();
                        _localCache.CacheModel("DonanimCount", donanimCountDto);
                    }
                }

                _isConnected = true;
                Messaglama.MessagYaz("Всички модели са успешно кеширани за текущата машина");
            }
            catch (Exception ex)
            {
                _isConnected = false;
                Messaglama.MessagYaz($"Грешка при зареждане от SQL сървъра: {ex.Message}");
                
                // Опит за зареждане на кешираните данни - използваме DTO типовете
                var cachedMachine = _localCache.GetCachedModel<OpMashinDto>("CurrentMachine");
                var cachedVardiya = _localCache.GetCachedModel<UrVardiyaDto>("ActiveVardiya");
                var cachedIstasyon = _localCache.GetCachedModel<UrIstasyonDto>("CurrentIstasyon");
                var cachedFamily = _localCache.GetCachedModel<OrFamilyDto>("CurrentFamily");
                var cachedModulerYapi = _localCache.GetCachedModel<IEnumerable<UrModulerYapiDto>>("ModulerYapi");
                var cachedGenerateId = _localCache.GetCachedModel<IEnumerable<IzGenerateIdDto>>("GenerateId");
                var cachedDonanimCount = _localCache.GetCachedModel<IEnumerable<IzDonanimCountDto>>("DonanimCount");

                if (cachedMachine != null || cachedVardiya != null || cachedIstasyon != null || 
                    cachedFamily != null || cachedModulerYapi != null || cachedGenerateId != null || 
                    cachedDonanimCount != null)
                {
                    Messaglama.MessagYaz("Използват се кеширани данни за моделите");
                }
                else
                {
                    Messaglama.MessagYaz("Няма налични кеширани данни за моделите");
                }
            }
        }

        public void SaveChanges(IzGenerateId model)
        {
            try
            {
                if (_isConnected)
                {
                    var result = _unitOfWork.GetRepository<IzGenerateId>().Update(model);
                    if (result.Success)
                    {
                        // Конвертираме до DTO преди кеширане
                        var modelDto = model.ToDto();
                        _localCache.CacheModel($"GenerateId_{model.Id}", modelDto);
                        Messaglama.MessagYaz("Промените са запазени успешно в базата данни");
                    }
                    else
                    {
                        Messaglama.MessagYaz($"Грешка при запазване в базата: {result.Message}");
                        _localCache.AddPendingOperation("UpdateModel", model);
                        Messaglama.MessagYaz("Промените са запазени локално и ще бъдат синхронизирани при възстановяване на връзката");
                    }
                }
                else
                {
                    _localCache.AddPendingOperation("UpdateModel", model);
                    Messaglama.MessagYaz("Промените са запазени локално и ще бъдат синхронизирани при възстановяване на връзката");
                }
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz($"Грешка при запазване: {ex.Message}");
                _localCache.AddPendingOperation("UpdateModel", model);
                Messaglama.MessagYaz("Промените са запазени локално и ще бъдат синхронизирани при възстановяване на връзката");
            }
        }

        // Таймер за периодична проверка на връзката
        private System.Windows.Forms.Timer _connectionTimer;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            _connectionTimer = new System.Windows.Forms.Timer();
            _connectionTimer.Interval = 60000; // Проверка на всяка минута
            _connectionTimer.Tick += async (sender, e) => 
            {
                await _syncService.CheckConnectionAndSync();
                _isConnected = _syncService.IsConnected;
                
                // Актуализираме статус индикатора, ако има такъв
                UpdateConnectionStatus(_isConnected);
            };
            _connectionTimer.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _connectionTimer?.Stop();
            _connectionTimer?.Dispose();
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

        private void LogOperation(string operation, string details)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                string logFile = Path.Combine(logPath, $"operations_{DateTime.Now:yyyyMMdd}.txt");
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{operation}|{details}\n";
                File.AppendAllText(logFile, logEntry);
            }
            catch (Exception ex)
            {
                mes.messanger($"Грешка при логване: {ex.Message}");
            }
        }

        private void PrintLabel(string fileName)
        {
            try
            {
                // Създаваме директория за етикети ако не съществува
                if (!Directory.Exists(pathc[6]))
                    Directory.CreateDirectory(pathc[6]);

                var barcodeData = new SyBarcodeOut { BarcodeIcerik = Path.GetFileNameWithoutExtension(fileName).ToUpper() };
                var parsedData = GitParcalama(barcodeData);

                // Създаваме PRN файл за етикета
                string labelFile = Path.Combine(pathc[6], $"label_{DateTime.Now:yyyyMMddHHmmss}.prn");
                string labelContent = $@"^XA
^FO50,50^A0N,50,50^FD{parsedData.prefix}-{parsedData.family}-{parsedData.suffix}^FS
^FO50,120^A0N,40,40^FDDonanim ID: {parsedData.IdDonanim}^FS
^FO50,180^A0N,40,40^FDName: {parsedData.Name}^FS
^BY3,3,100^FO50,290^BC^FD{parsedData.prefix}-{parsedData.family}-{parsedData.suffix}{parsedData.IdDonanim}^FS
^XZ";
                
                File.WriteAllText(labelFile, labelContent);
                
                // Логваме информацията за етикета
                LogOperation("PRINT_LABEL", $"File:{labelFile}|Data:{parsedData.prefix}-{parsedData.family}-{parsedData.suffix}|ID:{parsedData.IdDonanim}");
                
                // Изпращаме към принтера
                System.Diagnostics.Process.Start("cmd.exe", $"/c copy \"{labelFile}\" \"\\\\.\\PRN\"");
            }
            catch (Exception ex)
            {
                mes.messanger($"Грешка при печат на етикет: {ex.Message}");
                LogOperation("PRINT_ERROR", ex.Message);
            }
        }

        private async Task ProcessPendingFiles()
        {
            try
            {
                // Използваме SynchronizationService за проверка на връзката
                await _syncService.CheckConnectionAndSync();
                
                if (!_syncService.IsConnected)
                {
                    Messaglama.MessagYaz("Няма връзка с базата данни. Файловете ще бъдат обработени когато връзката се възстанови.");
                    return; // Все още няма връзка
                }

                // Взимаме всички pending файлове
                string pendingPath = pathc[5]; // C:\\_Kliptest\\
                var pendingFiles = Directory.GetFiles(pendingPath, "pending_*.txt");

                if (pendingFiles.Length > 0)
                {
                    Messaglama.MessagYaz($"Започва обработка на {pendingFiles.Length} pending файлове");
                }

                foreach (var pendingFile in pendingFiles)
                {
                    try
                    {
                        string fileName = Path.GetFileName(pendingFile);
                        // Премахваме "pending_" префикса
                        string originalFileName = fileName.Substring(8);

                        // Опитваме се да запишем във базата данни
                         var result = _elTest.GitSystemeYukle(originalFileName.ToUpper());
                        LogOperation("PENDING_DEBUG", $"Return type: {result?.GetType()}, Properties: {string.Join(", ", result?.GetType().GetProperties().Select(p => $"{p.Name}={p.GetValue(result)}"))}");

                        // Ако записът е успешен, изтриваме pending файла
                        if (result.Success)
                        {
                            File.Delete(pendingFile);
                            LogOperation("PENDING_PROCESSED", $"Successfully processed: {originalFileName}");
                            Messaglama.MessagYaz($"Успешно обработен файл: {originalFileName}");
                        }
                        else
                        {
                            LogOperation("PENDING_FAILED", $"Failed to process: {originalFileName}, Error: {result.Message}");
                            Messaglama.MessagYaz($"Неуспешна обработка на файл: {originalFileName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogOperation("PENDING_ERROR", $"Error processing {pendingFile}: {ex.Message}");
                        Messaglama.MessagYaz($"Грешка при обработка на файл: {pendingFile}, {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                mes.messanger($"Грешка при обработка на pending файлове: {ex.Message}");
                LogOperation("PENDING_PROCESS_ERROR", ex.Message);
            }
        }

        private async void Watcher1_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                // Проверка на връзката със SQL сървъра чрез SynchronizationService
                await _syncService.CheckConnectionAndSync();
                
                if (!_syncService.IsConnected)
                {
                    mes.messanger("Няма връзка със SQL сървъра! Моля проверете връзката.");
                    LogOperation("SQL_ERROR", "No connection");
                    if (!string.IsNullOrEmpty(e.FullPath))
                    {
                        string pendingFile = Path.Combine(pathc[5], "pending_" + Path.GetFileName(e.FullPath));
                        File.Move(e.FullPath, pendingFile);
                        LogOperation("FILE_PENDING", pendingFile);
                        Messaglama.MessagYaz($"Файлът е запазен като pending: {pendingFile}");
                    }
                    return;
                }

                // Ако връзката е успешна, първо обработваме pending файловете
                LogOperation("SQL_SUCCESS", "Connection established");
                await ProcessPendingFiles();
                
                // След това обработваме текущия файл
                var veri = ((FileSystemWatcher)sender);
                Thread.Sleep(XMLIslemi.XmlSaniye());
                Watcher1(veri.Path, veri.Filter);
            }
            catch (ErrorExceptionHandller ex)
            {
                mes.messanger(ex.Message);
                LogOperation("GENERAL_ERROR", ex.Message);
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
            try
            {
                // Проверяваме за pending файлове при стартиране
                ProcessPendingFiles();
            }
            catch (Exception ex)
            {
                LogOperation("STARTUP_ERROR", ex.Message);
            }

            this.watcher1.EnableRaisingEvents = true;
            this.watcher2.EnableRaisingEvents = true;
            this.watcher3.EnableRaisingEvents = true;
            this.Kliptest_3.EnableRaisingEvents = true;
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
                var idBak = TorkService.GitSytemeSayiElTestBack(new SyBarcodeInput() { BarcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}" });
                scren = new ScreenSaverForm(0, strArrays.Length > 2 ? strArrays[2] : "Info");
                sicil = new SicilOkuma(strArrays.Length > 2 ? strArrays[2] : "Info");
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
                    var idBak = TorkService.GitSytemeSayiBac(new SyBarcodeInput() { BarcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}" });
                    scren = new ScreenSaverForm(0, strArrays.Length > 2 ? strArrays[2] : "Info");
                    sicil = new SicilOkuma(strArrays.Length > 2 ? strArrays[2] : "Info");
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

        // Метод за актуализиране на UI според статуса на връзката
        private void UpdateConnectionStatus(bool isConnected)
        {
            // Тук можем да допълним статуса в заглавието на формата
            this.Invoke((MethodInvoker)delegate
            {
                if (isConnected)
                {
                    this.Text = $"ElTest - Свързан";
                    this.BackColor = Color.FromArgb(200, 255, 200); // Light green
                }
                else
                {
                    this.Text = $"ElTest - Няма връзка";
                    this.BackColor = Color.FromArgb(255, 200, 200); // Light red
                }
                
                // Обновяваме лога
                string statusMessage = isConnected ? 
                    "Връзка с базата данни: Установена" : 
                    "Връзка с базата данни: Няма връзка, използва се локален кеш";
                Messaglama.MessagYaz(statusMessage);
            });
        }
    }
}