using MediatR;
using Nursan.API.DTOs;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за актуализация на ToplamV769
    /// </summary>
    public class UpdateToplamV769Command : IRequest<TorkResultDto>
    {
        /// <summary>
        /// Данни за актуализация
        /// </summary>
        public ToplamV769Dto Data { get; set; }
    }
}
