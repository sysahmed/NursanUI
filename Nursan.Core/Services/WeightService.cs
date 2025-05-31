using Nursan.Domain.Interfaces;
using Nursan.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Nursan.Core.Services
{
    /// <summary>
    /// Услуга за обработка на данни от претегляне
    /// </summary>
    public class WeightService : BaseService, IWeightService
    {
        private readonly IPrinterService _printerService;

        /// <summary>
        /// Създава нова инстанция на услугата
        /// </summary>
        /// <param name="logger">Логър</param>
        /// <param name="printerService">Услуга за печат</param>
        public WeightService(ILogger logger, IPrinterService printerService) 
            : base(logger)
        {
            _printerService = printerService ?? throw new ArgumentNullException(nameof(printerService));
        }

        /// <summary>
        /// Обработва данни получени от везна
        /// </summary>
        /// <param name="data">Данни от везната</param>
        /// <returns>Резултат от обработката</returns>
        public async Task<WeightResult> ProcessWeightDataAsync(WeightData data)
        {
            if (!ValidateNotNull(data, nameof(data), "Невалидни данни от претегляне"))
            {
                return new WeightResult
                {
                    Status = WeightStatus.Error,
                    ErrorMessage = "Липсващи данни от претегляне",
                    Timestamp = DateTime.Now
                };
            }

            if (!ValidateNotNullOrEmpty(data.ScaleId, nameof(data.ScaleId), "Липсващ идентификатор на везна"))
            {
                return new WeightResult
                {
                    Status = WeightStatus.Error,
                    ErrorMessage = "Липсващ идентификатор на везна",
                    Timestamp = DateTime.Now
                };
            }

            //_logger.LogInformation($"Получени данни от везна: {data.Weight} кг.", data.ScaleId);

            return await ExecuteWithErrorHandling(async () =>
            {
                // Тук би се извършила допълнителна обработка и валидация на данните
                // като проверка за допустимия обхват на теглото, връзка с база данни и т.н.
                
                var result = new WeightResult
                {
                    ScaleId = data.ScaleId,
                    Weight = data.Weight,
                    Material = data.Material,
                    Status = WeightStatus.Processed,
                    Timestamp = DateTime.Now,
                    IsLabelPrinted = false
                };

                // Опитваме се да отпечатаме етикет, ако е необходимо
                try
                {
                    result.IsLabelPrinted = await _printerService.PrintLabelAsync(result);
                    if (!result.IsLabelPrinted)
                    {
                        _logger.LogWarning($"Неуспешен печат на етикет за претегляне {result.ScaleId}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Грешка при печат на етикет: {ex.Message}");
                }

                // Записваме статистика
                await LogWeightStatisticsAsync(result);

                return result;
            }, 
            new WeightResult
            {
                ScaleId = data.ScaleId,
                Status = WeightStatus.Error,
                ErrorMessage = "Грешка при обработка на данни от претегляне",
                Timestamp = DateTime.Now
            }, 
            "Грешка при обработка на данни от претегляне");
        }

        /// <summary>
        /// Получава последните данни от везна по идентификатор
        /// </summary>
        /// <param name="scaleId">Идентификатор на везната</param>
        /// <returns>Последните данни от везната или null ако няма данни</returns>
        public Task<WeightData> GetLastWeightDataAsync(string scaleId)
        {
            if (!ValidateNotNullOrEmpty(scaleId, nameof(scaleId), "Липсващ идентификатор на везна"))
            {
                return Task.FromResult<WeightData>(null);
            }

            return ExecuteWithErrorHandling(async () =>
            {
                // Тук би се извършила заявка до база данни за последните данни
                // Това е заместител на реална имплементация
                await Task.Delay(100); // Симулиране на асинхронна операция

                // Примерни данни
                return new WeightData
                {
                    ScaleId = scaleId,
                    Weight = 123.45m,
                    Material = "Метал",
                    WeightTime = DateTime.Now.AddMinutes(-5)
                };
            },
            null,
            $"Грешка при получаване на последни данни за везна {scaleId}");
        }

        /// <summary>
        /// Печата етикет за претегляне
        /// </summary>
        /// <param name="weightResult">Резултат от претегляне</param>
        /// <returns>true ако печатането е успешно, false в противен случай</returns>
        public async Task<bool> PrintLabelAsync(WeightResult weightResult)
        {
            if (!ValidateNotNull(weightResult, nameof(weightResult), "Липсващ резултат от претегляне"))
            {
                return false;
            }

            return await ExecuteWithErrorHandling(async () =>
            {
                bool isPrinted = await _printerService.PrintLabelAsync(weightResult);
                
                if (isPrinted)
                {
                    weightResult.IsLabelPrinted = true;
                    //_logger.LogInformation($"Отпечатан етикет за претегляне: {weightResult.Weight} кг.", weightResult.ScaleId);
                }
                else
                {
                    _logger.LogWarning($"Не може да се отпечата етикет за претегляне {weightResult.ScaleId}");
                }
                
                return isPrinted;
            },
            false,
            $"Грешка при печат на етикет за везна {weightResult.ScaleId}");
        }

        /// <summary>
        /// Проверява статуса на претеглянето
        /// </summary>
        /// <param name="scaleId">Идентификатор на везната</param>
        /// <returns>Статус на последното претегляне</returns>
        public Task<WeightStatus> CheckWeightStatusAsync(string scaleId)
        {
            if (!ValidateNotNullOrEmpty(scaleId, nameof(scaleId), "Липсващ идентификатор на везна"))
            {
                return Task.FromResult(WeightStatus.Error);
            }

            return ExecuteWithErrorHandling(async () =>
            {
                // Тук би се извършила заявка до база данни за последния статус
                // Това е заместител на реална имплементация
                await Task.Delay(50); // Симулиране на асинхронна операция

                // Примерен статус
                return WeightStatus.Processed;
            },
            WeightStatus.Error,
            $"Грешка при проверка на статус за везна {scaleId}");
        }

        /// <summary>
        /// Записва статистика за претеглянето
        /// </summary>
        /// <param name="weightResult">Резултат от претегляне</param>
        /// <returns>true ако записването е успешно, false в противен случай</returns>
        public Task<bool> LogWeightStatisticsAsync(WeightResult weightResult)
        {
            if (!ValidateNotNull(weightResult, nameof(weightResult), "Липсващ резултат от претегляне"))
            {
                return Task.FromResult(false);
            }

            return ExecuteWithErrorHandling(async () =>
            {
                // Тук би се извършило записване на статистиката в база данни
                // Това е заместител на реална имплементация
                await Task.Delay(50); // Симулиране на асинхронна операция

               // _logger.LogWithId($"Записана статистика за претегляне: {weightResult.Weight} кг.", weightResult.ScaleId);
                return true;
            },
            false,
            $"Грешка при записване на статистика за везна {weightResult.ScaleId}");
        }
    }
} 