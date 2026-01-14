using MediatR;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за проверка дали Alert е заключен
    /// </summary>
    public class CheckAlertGkLockedCommand : IRequest<CheckAlertGkLockedResult>
    {
        /// <summary>
        /// Баркод за проверка
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }

    /// <summary>
    /// Резултат от проверка на Alert GK
    /// </summary>
    public class CheckAlertGkLockedResult
    {
        /// <summary>
        /// Дали Alert е заключен
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Съобщение
        /// </summary>
        public string Message { get; set; }
    }
}
