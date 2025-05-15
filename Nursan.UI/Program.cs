using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Nursan.Caching;
using Nursan.Core.Services;
using Nursan.Domain.AmbarModels;
using Nursan.Domain.Entity;
using Nursan.Domain.Personal;
using Nursan.Domain.TORKS;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Repository;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.Interface;
using Nursan.Validations.KasaManager;
using Nursan.Validations.Opsionlar;
using Nursan.Validations.ValidationCode;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace Nursan.UI
{
    public static class Program
    {
        public static IConfiguration _configuration;
        public static LocalCacheService _localCache;

        [STAThread]
        public static void Main(string[] args)
        {
            // ReadOnlySpan<char> input="KK2T - 14401 - ABCD_000000004".AsSpan();
            // var veriler= StringSpanConverter.GetCharsMyDecoder(input,'-');
            // var veriler1 = StringSpanConverter.SplitWithoutAllocation(input, '-');
            var app = new Apllications();
            app.Starter();
        }
        public class Apllications
        {
            private readonly LocalCacheService _localCache;

            public Apllications()
            {
                _localCache = new LocalCacheService();
            }

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
                 //Application.Run(new Ariza.Form1());
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key.SetValue("Nursan.UI", "\"" + Application.ExecutablePath + "\"");
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                ApplicationConfiguration.Initialize();
                Application.SetCompatibleTextRenderingDefault(false);
                var builder = CreateHostBuilder();
                builder.Build();
                UretimOtomasyonContext _context = new();
                UnitOfWork unitOfWork = new(_context);
                // Application.Run(new Nursan.UI.Kasalama.Kasalama());

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

                        // Кеширане на резултата
                        if (result != null)
                        {
                            _localCache.CacheModel("CurrentWorkstation", result);
                        }

                        // Проверка за кеширани данни ако няма връзка с SQL сървъра
                        if (result == null)
                        {
                            result = _localCache.GetCachedModel<dynamic>("CurrentWorkstation");
                            Messaglama.MessagYaz("Използват се кеширани данни поради липса на връзка с SQL сървъра");
                        }

                        if (result != null)
                        {
                            switch (result.ModulerYapiID)
                            {
                                case 3:
                                    Messaglama.MessagYaz($"Етап:{result.ModulerYapiEtap},Машина:{result.Makine},Станция:{result.Istasyon},Фамилия:{result.FamilyName}");
                                    Application.Run(new KlipTest(unitOfWork));
                                    break;
                                case 4:
                                    Messaglama.MessagYaz($"Етап:{result.ModulerYapiEtap},Машина:{result.Makine},Станция:{result.Istasyon},Фамилия:{result.FamilyName}");
                                    Application.Run(new ElTest(unitOfWork));
                                    break;
                                case >= 11:
                                    Messaglama.MessagYaz($"Етап:{result.ModulerYapiEtap},Машина:{result.Makine},Станция:{result.Istasyon},Фамилия:{result.FamilyName}");
                                    Application.Run(new Staring(unitOfWork));
                                    break;
                                case >= 5:
                                    Messaglama.MessagYaz($"Етап:{result.ModulerYapiEtap},Машина:{result.Makine},Станция:{result.Istasyon},Фамилия:{result.FamilyName}");
                                    Application.Run(new StaringAP(unitOfWork));
                                    break;
                                default:
                                    Messaglama.MessagYaz($"Етап:{result.ModulerYapiEtap},Машина:{result.Makine},Станция:{result.Istasyon},Фамилия:{result.FamilyName}");
                                    Application.Run(new Staring(unitOfWork));
                                    break;
                            }
                        }
                        else
                        {
                            Messaglama.MessagYaz("Няма налични данни за текущата работна станция");
                            Application.Run(new Staring(unitOfWork));
                        }
                    }
                    catch (Exception ex)
                    {
                        Messaglama.MessagYaz($"Грешка: {ex.Message}");
                        // Опит за използване на кеширани данни при грешка
                        var cachedResult = _localCache.GetCachedModel<dynamic>("CurrentWorkstation");
                        if (cachedResult != null)
                        {
                            Messaglama.MessagYaz("Използват се кеширани данни поради грешка в SQL сървъра");
                            Application.Run(new ElTest(unitOfWork));
                        }
                        else
                        {
                            Application.Run(new Staring(unitOfWork));
                        }
                    }

               // }
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
                    services.AddSingleton<Nursan.Caching.ILocalCache, Nursan.Caching.LocalCacheService>();
                    services.AddScoped<Nursan.Business.Services.ISynchronizationService, Nursan.Business.Services.SynchronizationService>();
                    services.AddLogging();
                    services.AddTransient<LoggerService>();
                });
            }
            public static System.Data.IDbConnection GetConnection()
            {
                return new SQLiteConnection($"Data Source={AppDomain.CurrentDomain.BaseDirectory}OptionDB.db; New=true; Password=myPassword;");
            }

            //public void ConfigureServices(IServiceCollection services)
            //{
            //    services.AddScoped<UnitOfWork>(); // Scoped      (request)
            //    services.AddSingleton<DonanimService>(); // Singleton ����� ���� ��������� �� ����� ������ ����� �� ������������
            //    services.AddSingleton<ToplamV769Services>();
            //    services.AddScoped<PersonalValidasyonu>();
            //    services.AddScoped<OzelReferansControlEt>();
            //}
        }

    }
}


