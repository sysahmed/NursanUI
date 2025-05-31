using Nursan.Domain.Entity;
using Nursan.Core.Interfaces;
using Nursan.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursan.Core.Services
{
    /// <summary>
    /// Услуга за работа с аларми
    /// </summary>
    public class AlertService : BaseService, IAlertService
    {
        private readonly IOrajSV _orajService;

        /// <summary>
        /// Създава нова инстанция на AlertService
        /// </summary>
        /// <param name="orajService">Услуга за ORAJ сървър</param>
        /// <param name="logger">Логър</param>
        public AlertService(IOrajSV orajService, ILogger logger) : base(logger)
        {
            _orajService = orajService ?? throw new ArgumentNullException(nameof(orajService));
        }

        /// <inheritdoc/>
        public async Task<List<OrAlert>> GetAlerts(string harnessModelName)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                _logger.LogInformation($"Извличане на аларми за харнес модел: {harnessModelName}");

                // Вземаме всички аларми, които са свързани с този харнес модел
                var allAlerts = await _orajService.GetAllAlerts();
                var harnessAlerts = allAlerts
                    .Where(alert => alert.AlertNumber.HasValue && 
                           _orajService.IsAlertLinkedToHarness(alert.AlertNumber.Value, harnessModelName))
                    .ToList();
                
                if (!harnessAlerts.Any())
                {
                    _logger.LogInformation($"Не са намерени аларми за харнес модел: {harnessModelName}");
                    return new List<OrAlert>();
                }

                _logger.LogInformation($"Намерени са {harnessAlerts.Count} аларми за харнес модел: {harnessModelName}");
                return harnessAlerts;
            }, 
            new List<OrAlert>(), 
            $"Грешка при извличане на аларми за харнес модел: {harnessModelName}");
        }

        /// <inheritdoc/>
        public async Task<OrAlert> GetAlert(int alertId)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                _logger.LogInformation($"Извличане на детайли за аларма с ID: {alertId}");
                
                var alert = await _orajService.GetAlert(alertId);
                
                if (alert == null)
                {
                    _logger.LogError($"Не е намерена аларма с ID: {alertId}");
                    return null;
                }
                
                return alert;
            }, 
            null, 
            $"Грешка при извличане на аларма с ID: {alertId}");
        }
        
        /// <inheritdoc/>
        public async Task<OrAlert> GetAlertByNumber(int alertNumber)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                _logger.LogInformation($"Извличане на детайли за аларма с номер: {alertNumber}");
                
                var alert = await _orajService.GetAlertByNumber(alertNumber);
                
                if (alert == null)
                {
                    _logger.LogError($"Не е намерена аларма с номер: {alertNumber}");
                    return null;
                }
                
                return alert;
            }, 
            null, 
            $"Грешка при извличане на аларма с номер: {alertNumber}");
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAlertStatus(int alertId, int count)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                _logger.LogInformation($"Актуализиране на статус на аларма с ID: {alertId}, брой: {count}");
                
                var result = await _orajService.UpdateAlert(alertId, count);
                
                if (!result)
                {
                    _logger.LogError($"Неуспешна актуализация на аларма с ID: {alertId}");
                }
                
                return result;
            }, 
            false, 
            $"Грешка при актуализиране на аларма с ID: {alertId}");
        }
    }
} 