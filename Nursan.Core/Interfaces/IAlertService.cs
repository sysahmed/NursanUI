using Nursan.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nursan.Core.Interfaces
{
    /// <summary>
    /// Интерфейс за услуга за работа с аларми
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// Взема всички аларми за даден харнес модел
        /// </summary>
        /// <param name="harnessModelName">Име на харнес модела</param>
        /// <returns>Списък с аларми за харнес модела</returns>
        Task<List<OrAlert>> GetAlerts(string harnessModelName);
        
        /// <summary>
        /// Актуализира статуса на аларма
        /// </summary>
        /// <param name="alertId">Идентификатор на алармата</param>
        /// <param name="count">Нов брой на алармата</param>
        /// <returns>Успешна ли е операцията</returns>
        Task<bool> UpdateAlertStatus(int alertId, int count);
        
        /// <summary>
        /// Взема детайлна информация за аларма по ID
        /// </summary>
        /// <param name="alertId">ID на алармата</param>
        /// <returns>Детайлна информация за аларма</returns>
        Task<OrAlert> GetAlert(int alertId);
        
        /// <summary>
        /// Взема аларма по нейния номер
        /// </summary>
        /// <param name="alertNumber">Номер на алармата</param>
        /// <returns>Аларма с посочения номер</returns>
        Task<OrAlert> GetAlertByNumber(int alertNumber);
    }
} 