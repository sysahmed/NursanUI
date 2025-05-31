using Nursan.Domain.Interfaces;
using Nursan.Domain.Models;
using Nursan.Core.Printing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursan.Core.Services
{
    /// <summary>
    /// Услуга за обработка на процеси с етикетиране
    /// </summary>
    public class TorkService : BaseService
    {
        private readonly DirectPrinting _directPrinting;
        private readonly IConfigurationService _configService;

        /// <summary>
        /// Създава нова инстанция на TorkService
        /// </summary>
        /// <param name="logger">Логър</param>
        /// <param name="configService">Конфигурационна услуга</param>
        public TorkService(ILogger logger, IConfigurationService configService) : base(logger)
        {
           // _directPrinting = new DirectPrinting(logger);
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
        }

        /// <summary>
        /// Обработва данни от измерване
        /// </summary>
        /// <param name="weightData">Входни данни</param>
        /// <returns>Обработени данни или null при грешка</returns>
        public async Task<WeightResult> ProcessWeightDataAsync(WeightData weightData)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                if (!ValidateNotNull(weightData, nameof(weightData), "Невалидни входни данни"))
                {
                    return null;
                }

                if (string.IsNullOrEmpty(weightData.ScaleId))
                {
                    _logger.LogError("Липсва идентификатор в данните");
                    return null;
                }

                // Обработка на данните
                WeightResult result = new WeightResult
                {
                    ScaleId = weightData.ScaleId,
                    Weight = weightData.Weight,
                    Material = weightData.Material,
                    Timestamp = DateTime.Now,
                    Status = WeightStatus.Processed,
                    IsSuccess = true
                };

                await Task.Delay(10); // Малко забавяне за асинхронността

                _logger.LogInformation($"Успешно обработени данни: {weightData.Weight} кг {weightData.Material}");
                return result;
            }, null, "Грешка при обработка на данни");
        }

        /// <summary>
        /// Генерира данни за етикет
        /// </summary>
        /// <param name="weightResult">Резултати за етикет</param>
        /// <returns>Данни за етикет или null при грешка</returns>
        public async Task<string> GenerateLabelDataAsync(WeightResult weightResult)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                if (!ValidateNotNull(weightResult, nameof(weightResult), "Невалидни резултати"))
                {
                    return null;
                }

                await Task.Delay(10); // Малко забавяне за асинхронността

                // Генериране на ZPL данни за етикета
                StringBuilder labelBuilder = new StringBuilder();
                labelBuilder.AppendLine("^XA");
                labelBuilder.AppendLine("^FO50,50^A0N,50,50^FDMaterial: " + weightResult.Material + "^FS");
                labelBuilder.AppendLine("^FO50,120^A0N,50,50^FDWeight: " + weightResult.Weight + " kg^FS");
                labelBuilder.AppendLine("^FO50,190^A0N,50,50^FDScale ID: " + weightResult.ScaleId + "^FS");
                labelBuilder.AppendLine("^FO50,260^A0N,50,50^FDTime: " + weightResult.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + "^FS");
                
                // Добавяне на баркод
                labelBuilder.AppendLine("^FO50,330^BY3");
                labelBuilder.AppendLine("^BCN,100,Y,N,N");
                labelBuilder.AppendLine("^FD" + weightResult.ScaleId + "|" + weightResult.Weight + "^FS");
                
                labelBuilder.AppendLine("^XZ");
                
                return labelBuilder.ToString();
            }, null, "Грешка при генериране на данни за етикет");
        }

    
        /// <summary>
        /// Генерира обобщен отчет
        /// </summary>
        /// <param name="weightResults">Списък с резултати</param>
        /// <returns>Текстов отчет или null при грешка</returns>
        public async Task<string> GenerateReportAsync(List<WeightResult> weightResults)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                if (!ValidateNotNull(weightResults, nameof(weightResults), "Невалиден списък с резултати"))
                {
                    return null;
                }

                if (weightResults.Count == 0)
                {
                    return "Няма данни за отчет.";
                }

                await Task.Delay(50); // Малко забавяне за асинхронността

                StringBuilder report = new StringBuilder();
                report.AppendLine("Отчет за данни");
                report.AppendLine("===================");
                report.AppendLine($"Дата: {DateTime.Now:yyyy-MM-dd}");
                report.AppendLine($"Брой записи: {weightResults.Count}");
                report.AppendLine();

                // Сумарно тегло по материали
                var materialGroups = weightResults
                    .GroupBy(r => r.Material)
                    .Select(g => new
                    {
                        Material = g.Key,
                        TotalWeight = g.Sum(r => r.Weight),
                        Count = g.Count()
                    })
                    .OrderByDescending(g => g.TotalWeight);

                report.AppendLine("Обобщение по материали:");
                report.AppendLine("---------------------");
                foreach (var group in materialGroups)
                {
                    report.AppendLine($"{group.Material}: {group.TotalWeight:F2} кг ({group.Count} бр.)");
                }
                report.AppendLine();

                // Списък на записите
                report.AppendLine("Детайли за записите:");
                report.AppendLine("------------------------");
                foreach (var result in weightResults.OrderBy(r => r.Timestamp))
                {
                    report.AppendLine($"{result.Timestamp:yyyy-MM-dd HH:mm:ss} | {result.ScaleId} | {result.Material} | {result.Weight:F2} кг");
                }

                return report.ToString();
            }, null, "Грешка при генериране на отчет");
        }

        /// <summary>
        /// Получава път до принтера от конфигурацията
        /// </summary>
        /// <returns>Път до принтера или null при грешка</returns>
        public string GetPrinterPath()
        {
            return _configService.GetSetting("PrinterPath", "");
        }

        /// <summary>
        /// Записва информация за операция с идентификатор
        /// </summary>
        /// <param name="message">Съобщение</param>
        /// <param name="id">Идентификатор</param>
        private void LogWithId(string message, string id)
        {
            _logger.LogInformation($"{message} [ID: {id}]");
        }
    }
} 