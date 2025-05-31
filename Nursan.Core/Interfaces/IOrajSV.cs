using Nursan.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nursan.Core.Interfaces
{
    /// <summary>
    /// Интерфейс за комуникация с ORAJ сървъра
    /// </summary>
    public interface IOrajSV
    {
        /// <summary>
        /// Взема връзки между аларми и харнес модели
        /// </summary>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <returns>Списък с връзки между аларми и модела</returns>
        Task<List<OrAlertBaglanti>> GetAlertBaglanti(string harnessModelName);
        
        /// <summary>
        /// Взема детайлна информация за аларма по ID
        /// </summary>
        /// <param name="alertId">ID на алармата</param>
        /// <returns>Детайлна информация за аларма</returns>
        Task<OrAlert> GetAlert(int alertId);
        
        /// <summary>
        /// Взема детайлна информация за аларма по номер
        /// </summary>
        /// <param name="alertNumber">Номер на алармата</param>
        /// <returns>Детайлна информация за аларма</returns>
        Task<OrAlert> GetAlertByNumber(int alertNumber);
        
        /// <summary>
        /// Взема всички аларми от системата
        /// </summary>
        /// <returns>Списък с всички аларми</returns>
        Task<List<OrAlert>> GetAllAlerts();
        
        /// <summary>
        /// Проверява дали аларма с определен номер е свързана с даден харнес модел
        /// </summary>
        /// <param name="alertNumber">Номер на алармата</param>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <returns>true ако алармата е свързана с харнес модела, false в противен случай</returns>
        bool IsAlertLinkedToHarness(int alertNumber, string harnessModelName);
        
        /// <summary>
        /// Взема информация за харнес модел
        /// </summary>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <returns>Информация за харнес модел</returns>
        Task<OrHarnessModel> GetHarnessModel(string harnessModelName);
        
        /// <summary>
        /// Актуализира статуса на аларма
        /// </summary>
        /// <param name="alertId">Идентификатор на алармата</param>
        /// <param name="count">Нов брой на алармата</param>
        /// <returns>Успешна ли е операцията</returns>
        Task<bool> UpdateAlert(int alertId, int count);
    }
} 