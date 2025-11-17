using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Nursan.Business.Manager;
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
using System.Net;
using System.Net.Security;

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
                //Application.Run(new Nursan.UI.Kasalama.Kasalama());
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
                                case >= 17 :
                                    Messaglama.MessagYaz($"Etap:{result.ModulerYapiEtap},Makine{result.Makine},Istasyon{result.Istasyon},Family        {result.FamilyName}");
                                    Application.Run(new GrometTSC(unitOfWork));
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


