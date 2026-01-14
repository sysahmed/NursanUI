using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за добавяне на донаним (сигурност)
    /// </summary>
    public class AddDonanimCountSigortaCommand : IRequest<TorkResultDto>
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
        /// ID на станция
        /// </summary>
        public int IstasyonId { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
