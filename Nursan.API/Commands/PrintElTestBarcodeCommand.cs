using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за печат на ElTest баркод
    /// </summary>
    public class PrintElTestBarcodeCommand : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Баркод за печат
        /// </summary>
        public BarcodeInputDto Barcode { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
