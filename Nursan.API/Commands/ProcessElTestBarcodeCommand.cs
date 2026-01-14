using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за обработка на ElTest баркодове
    /// </summary>
    public class ProcessElTestBarcodeCommand : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Масив от баркодове
        /// </summary>
        public string[] Barcodes { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
