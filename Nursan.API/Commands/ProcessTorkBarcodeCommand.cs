using MediatR;
using Nursan.API.DTOs;
using Nursan.Persistanse.Result;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за обработка на Tork баркодове
    /// </summary>
    public class ProcessTorkBarcodeCommand : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Баркодове за обработка
        /// </summary>
        public List<BarcodeInputDto> Barcodes { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
