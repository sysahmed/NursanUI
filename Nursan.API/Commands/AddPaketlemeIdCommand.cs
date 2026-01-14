using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за добавяне на Paketleme ID
    /// </summary>
    public class AddPaketlemeIdCommand : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Данни за донаним
        /// </summary>
        public DonanimCountDto DonanimCount { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
