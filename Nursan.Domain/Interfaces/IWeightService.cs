using Nursan.Domain.Models;
using System.Threading.Tasks;

namespace Nursan.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс за услуга за обработка на данни от претегляне
    /// </summary>
    public interface IWeightService
    {
        /// <summary>
        /// Обработва данни получени от везна
        /// </summary>
        /// <param name="data">Данни от везната</param>
        /// <returns>Резултат от обработката</returns>
        Task<WeightResult> ProcessWeightDataAsync(WeightData data);

        /// <summary>
        /// Получава последните данни от везна по идентификатор
        /// </summary>
        /// <param name="scaleId">Идентификатор на везната</param>
        /// <returns>Последните данни от везната или null ако няма данни</returns>
        Task<WeightData> GetLastWeightDataAsync(string scaleId);

        /// <summary>
        /// Печата етикет за претегляне
        /// </summary>
        /// <param name="weightResult">Резултат от претегляне</param>
        /// <returns>true ако печатането е успешно, false в противен случай</returns>
        Task<bool> PrintLabelAsync(WeightResult weightResult);

        /// <summary>
        /// Проверява статуса на претеглянето
        /// </summary>
        /// <param name="scaleId">Идентификатор на везната</param>
        /// <returns>Статус на последното претегляне</returns>
        Task<WeightStatus> CheckWeightStatusAsync(string scaleId);

        /// <summary>
        /// Записва статистика за претеглянето
        /// </summary>
        /// <param name="weightResult">Резултат от претегляне</param>
        /// <returns>true ако записването е успешно, false в противен случай</returns>
        Task<bool> LogWeightStatisticsAsync(WeightResult weightResult);
    }
} 