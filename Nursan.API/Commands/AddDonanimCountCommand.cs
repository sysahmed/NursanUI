using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за добавяне на донаним
    /// </summary>
    public class AddDonanimCountCommand : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Баркод
        /// </summary>
        public BarcodeInputDto Barcode { get; set; }

        /// <summary>
        /// Данни за GenerateId
        /// </summary>
        public GenerateIdDto GenerateId { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
