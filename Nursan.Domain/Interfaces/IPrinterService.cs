using Nursan.Domain.Models;
using System.Threading.Tasks;

namespace Nursan.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс за услуга за печат на етикети и други документи
    /// </summary>
    public interface IPrinterService
    {
        /// <summary>
        /// Печата етикет със зададените данни
        /// </summary>
        /// <param name="weightResult">Резултат от претегляне</param>
        /// <returns>true ако печатането е успешно, false в противен случай</returns>
        Task<bool> PrintLabelAsync(WeightResult weightResult);

        /// <summary>
        /// Получава статуса на принтера
        /// </summary>
        /// <returns>Статус на принтера</returns>
        Task<string> GetPrinterStatusAsync();

        /// <summary>
        /// Проверява дали принтерът е готов за печат
        /// </summary>
        /// <returns>true ако принтерът е готов, false в противен случай</returns>
        Task<bool> IsPrinterReadyAsync();

        /// <summary>
        /// Печата тестова страница
        /// </summary>
        /// <returns>true ако печатането е успешно, false в противен случай</returns>
        Task<bool> PrintTestPageAsync();
    }
} 