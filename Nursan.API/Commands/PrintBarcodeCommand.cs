using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за печат на баркод
    /// </summary>
    public class PrintBarcodeCommand : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Списък от баркодове за печат
        /// </summary>
        public List<BarcodeInputDto> Barcodes { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
