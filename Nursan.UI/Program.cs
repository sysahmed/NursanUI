using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Nursan.Business.Logging;
using Nursan.Business.Manager;
using Nursan.Business.Services;
using Nursan.Domain.AmbarModels;
using Nursan.Domain.Entity;
using Nursan.Domain.NursanBarcode;
using Nursan.Domain.Personal;
using Nursan.Domain.TORKS;
using Nursan.Licenzing;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Repository;
using Nursan.Persistanse.UnitOfWork;
using Nursan.UI.Kasalama;
using Nursan.UI.Security;
using Nursan.Validations.Interface;
using Nursan.Validations.KasaManager;
using Nursan.Validations.Opsionlar;
using Nursan.Validations.ValidationCode;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Security;
using System.Threading.Tasks;

namespace Nursan.UI
{
    public static class Program
    {
        public static IConfiguration _configuration;

        [STAThread]
        public static void Main(string[] args)
        {
            // Глобален SSL bypass - изключваме всички SSL проверки
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.Expect100Continue = false;
            
            // ReadOnlySpan<char> input="KK2T - 14401 - ABCD_000000004".AsSpan();
            // var veriler= StringSpanConverter.GetCharsMyDecoder(input,'-');
            // var veriler1 = StringSpanConverter.SplitWithoutAllocation(input, '-');
            var app = new Apllications();
            app.Starter();
        }
        public class Apllications
        {
            public void Starter()
            {
                string processName = Process.GetCurrentProcess().ProcessName;
                if ((int)Process.GetProcessesByName(processName).Length > 1)
                {
                    Process[] processes = Process.GetProcesses();
                    for (int i = 0; i < (int)processes.Length; i++)
                    {
                        Process process = processes[i];
                        if (process.ProcessName.StartsWith(processName))
                        {
                            process.Kill();
                        }
                    }
                }
              
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key.SetValue("Nursan.UI", "\"" + Application.ExecutablePath + "\"");
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                ApplicationConfiguration.Initialize();
                Application.SetCompatibleTextRenderingDefault(false);

                // Добавяне на глобални exception handlers за автоматично генериране на тикети при crash
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                // Опитваме се да инициализираме лиценза чрез API
                bool licenseInitialized = false;
                string licenseError = string.Empty;
                
                //try
                //{
                //    licenseInitialized = LicenseContext.TryInitialize(out licenseError);
                //}
                //catch (Exception ex)
                //{
                //    // Ако има изключение при опит за достъп до API, записваме грешката
                //    licenseError = $"Грешка при достъп до лицензния API: {ex.Message}";
                //    licenseInitialized = false;
                //}
                
                //// Ако не успее (не може да достъпи API или лицензът не е валиден), показваме формата за активация
                //if (!licenseInitialized)
                //{
                //    try
                //    {
                //        using (var licenseForm = new Lisanslama())
                //        {
                //            // Показваме формата модално - приложението ще чака докато се затвори
                //            licenseForm.ShowDialog();
                //        }
                        
                //        // След затваряне на формата, опитваме отново да инициализираме лиценза
                //        try
                //        {
                //            licenseInitialized = LicenseContext.TryInitialize(out licenseError);
                //        }
                //        catch (Exception ex)
                //        {
                //            // Ако отново има проблем с API, продължаваме със старата логика
                //            licenseError = $"Грешка при повторен опит за достъп до API: {ex.Message}";
                //            licenseInitialized = false;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        // Ако има проблем при създаването или показването на формата, показваме съобщение
                //        MessageBox.Show($"Грешка при отваряне на формата за лиценз: {ex.Message}\n\nОригинална грешка: {licenseError}", 
                //            "Лиценз", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
                //}

                var builder = CreateHostBuilder();
                builder.Build();
                UretimOtomasyonContext _context = new();
                UnitOfWork unitOfWork = new(_context);
                //Application.Run(new Sigorta("397",unitOfWork));
                // Application.Run(new ElTestvApi());
                //Application.Run(new BoltCameraConfigForm());
                //if (new LisanGet().LisanBak(processName))
                //{
                    try
                    {

                        var result = (from i in _context.UrIstasyons
                                      join p in _context.OpMashins on i.MashinId equals p.Id
                                      join f in _context.OrFamilies on i.FamilyId equals f.Id
                                      join m in _context.UrModulerYapis on i.ModulerYapiId equals m.Id
                                      where p.MasineName == Environment.MachineName && i.Activ == true
                                      select new
                                      {
                                          IstasyonID = i.Id,
                                          Istasyon = i.Name,
                                          MakineID = p.Id,
                                          Makine = p.MasineName,
                                          VardiyaID = i.VardiyaId,
                                          ModulerYapiID = m.Id,
                                          ModulerYapiEtap = m.Etap,
                                          FamilyID = f.Id,
                                          FamilyName = f.FamilyName

                                      }).FirstOrDefault();
                        if (result != null)
                        {

                            switch (result.ModulerYapiID)
                            {
                                case 3:
                                    Messaglama.MessagYaz($"Etap:{result.ModulerYapiEtap},Makine{result.Makine},Istasyon{result.Istasyon},Family        {result.FamilyName}");
                                    Application.Run(new KlipV1(unitOfWork));
                                    break;
                                case 4:
                                    Messaglama.MessagYaz($"Etap:{result.ModulerYapiEtap},Makine{result.Makine},Istasyon{result.Istasyon},Family        {result.FamilyName}");
                                    Application.Run(new ElTest(unitOfWork));
                                    break;
                                case >= 18 :
                                    Messaglama.MessagYaz($"Etap:{result.ModulerYapiEtap},Makine{result.Makine},Istasyon{result.Istasyon},Family        {result.FamilyName}");
                                    Application.Run(new Staring(unitOfWork));
                                    break;

                                case >= 5:
                                    Messaglama.MessagYaz($"Etap:{result.ModulerYapiEtap},Makine{result.Makine},Istasyon{result.Istasyon},Family        {result.FamilyName}");
                                    Application.Run(new StaringAP(unitOfWork));
                                    break;
                               
                                default:
                                    Messaglama.MessagYaz($"Etap:{result.ModulerYapiEtap},Makine{result.Makine},Istasyon{result.Istasyon},Family {result.FamilyName}");
                                    Application.Run(new Staring(unitOfWork));
                                    break;
                            }
                        }
                        else
                        {
                            Application.Run(new Staring(unitOfWork));
                        }
                    }
                    catch (ErrorExceptionHandller ex)
                    {
                        Messaglama.MessagYaz($"Etap:{ex.Message}");
                    }

                //}
            }
            /// <summary>
            /// Глобален exception handler за Thread exceptions
            /// </summary>
            private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
            {
                HandleGlobalException(e.Exception, "Thread Exception");
            }

            /// <summary>
            /// Глобален exception handler за Unhandled exceptions
            /// </summary>
            private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
            {
                if (e.ExceptionObject is Exception ex)
                {
                    HandleGlobalException(ex, "Unhandled Exception");
                }
            }

            /// <summary>
            /// Централен метод за обработка на глобални грешки
            /// </summary>
            private static void HandleGlobalException(Exception ex, string context)
            {
                try
                {
                    string formName = Application.OpenForms.Count > 0 
                        ? Application.OpenForms[0].GetType().Name 
                        : "Unknown";
                    
                    // Прави screenshot на целия екран
                    string screenshotPath = TakeGlobalScreenshot(formName);

                    // Създава детайлно съобщение за грешката
                    string errorDetails = $@"
ГРЕШКА В ПРИЛОЖЕНИЕТО
ФОРМА: {formName}
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

                    // Изпраща автоматичен тикет към IT системата
                    Task.Run(async () =>
                    {
                        try
                        {
                            SystemTicket ticketService = new SystemTicket();
                            string bolge = Environment.MachineName;
                            await ticketService.CreateTicket(
                                $"AUTO CRASH: {formName} - {context}",
                                bolge,
                                screenshotPath,
                                1 // Role = 1 (IT)
                            );
                        }
                        catch (Exception ticketEx)
                        {
                            Messaglama.MessagException($"Грешка при автоматично изпращане на тикет: {ticketEx.Message}");
                        }
                    });
                }
                catch (Exception handlerEx)
                {
                    Messaglama.MessagException($"Грешка в exception handler: {handlerEx.Message}");
                }
            }

            /// <summary>
            /// Прави screenshot на целия екран
            /// </summary>
            private static string TakeGlobalScreenshot(string formName)
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
                    string filename = $"CRASH_{formName}_{timestamp}.jpg";
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

                    return filepath;
                }
                catch (Exception ex)
                {
                    Messaglama.MessagException($"Грешка при създаване на screenshot: {ex.Message}");
                    return string.Empty;
                }
            }

            private static IHostBuilder CreateHostBuilder()
            {
                AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
                {
                    Messaglama.MessagException(eventArgs.Exception.ToString());
                    return;
                };
                return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddDbContext<UretimOtomasyonContext>();
                    services.AddDbContext<AmbarContext>();
                    services.AddDbContext<NursandatabaseContext>();
                    services.AddDbContext<PersonalContext>();
                    services.AddScoped<IUnitOfWork, UnitOfWork>();
                    services.AddSingleton(typeof(IRepository<ErRework>), typeof(RevorkService));
                    services.AddSingleton(typeof(IRepository<>), typeof(RepositoryORM<>));
                    services.AddSingleton(typeof(IRepositoryAmabar<>), typeof(RepositoryAmbar<>));
                    services.AddSingleton(typeof(IPersonalRepository<>), typeof(RepositoryAmbar<>));
                    services.AddSingleton(typeof(INewRepository<>), typeof(NewRepository<>));
                    services.AddSingleton(typeof(IKasaServices<>), typeof(KasaManager<>));
                    services.AddSingleton<ITorkManager, TorkKonfigService>();
                    services.AddSingleton<IHarnesConfigServices, HarnessConfigManager>();
                });
            }
            public static System.Data.IDbConnection GetConnection()
            {
                return new SQLiteConnection($"Data Source={AppDomain.CurrentDomain.BaseDirectory}OptionDB.db; Version=3;New=true; Password=myPassword;");

            }

            //public void ConfigureServices(IServiceCollection services)
            //{
            //    services.AddScoped<UnitOfWork>(); // Scoped ����� ���� ��������� �� ������ (request)
            //    services.AddSingleton<DonanimService>(); // Singleton ����� ���� ��������� �� ����� ������ ����� �� ������������
            //    services.AddSingleton<ToplamV769Services>();
            //    services.AddScoped<PersonalValidasyonu>();
            //    services.AddScoped<OzelReferansControlEt>();
            //}
        }

    }
}


