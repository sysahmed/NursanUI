using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за актуализация на ToplamV769 с bypass
    /// </summary>
    public class UpdateToplamV769BypassCommand : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Баркод
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
