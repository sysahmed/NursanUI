using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за проверка на системата
    /// </summary>
    public class CheckSystemCommand : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Баркод за проверка
        /// </summary>
        public BarcodeInputDto Barcode { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
