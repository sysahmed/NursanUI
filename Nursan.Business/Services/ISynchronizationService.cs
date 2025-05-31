using System.Threading.Tasks;

namespace Nursan.Business.Services
{
    public interface ISynchronizationService
    {
        /// <summary>
        /// Показва дали има връзка с базата данни
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Проверява връзката с базата данни и синхронизира данните
        /// </summary>
        Task CheckConnectionAndSync();

        /// <summary>
        /// Принудително синхронизира данните с базата данни
        /// </summary>
        Task ForceSync();
    }
} 