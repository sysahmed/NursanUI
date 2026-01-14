using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за генериране/проверка на ID
    /// </summary>
    public class GenerateIdCommand : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Баркод за обработка
        /// </summary>
        public BarcodeInputDto Barcode { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
