using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nursan.API.Commands;
using Nursan.API.DTOs;

namespace Nursan.API.Controllers
{
    /// <summary>
    /// Контролер за Tork операции
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TorkController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TorkController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Обработва Tork баркодове
        /// </summary>
        /// <param name="command">Команда с баркодове</param>
        /// <returns>Резултат от обработката</returns>
        [HttpPost("process-barcode")]
        public async Task<ActionResult<TorkResultDto>> ProcessTorkBarcode([FromBody] ProcessTorkBarcodeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Обработва ElTest баркодове
        /// </summary>
        /// <param name="command">Команда с баркодове</param>
        /// <returns>Резултат от обработката</returns>
        [HttpPost("process-eltest-barcode")]
        public async Task<ActionResult<TorkResultDto>> ProcessElTestBarcode([FromBody] ProcessElTestBarcodeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Актуализира ToplamV769 запис
        /// </summary>
        /// <param name="command">Команда за актуализация</param>
        /// <returns>Резултат от актуализацията</returns>
        [HttpPost("update-toplam-v769")]
        public async Task<ActionResult<TorkResultDto>> UpdateToplamV769([FromBody] UpdateToplamV769Command command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Добавя Paketleme ID
        /// </summary>
        /// <param name="command">Команда за добавяне</param>
        /// <returns>Резултат от операцията</returns>
        [HttpPost("add-paketleme-id")]
        public async Task<ActionResult<TorkResultDto>> AddPaketlemeId([FromBody] AddPaketlemeIdCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Проверява системата за ElTest
        /// </summary>
        /// <param name="command">Команда за проверка</param>
        /// <returns>Резултат от проверката</returns>
        [HttpPost("check-system-eltest")]
        public async Task<ActionResult<TorkResultDto>> CheckSystemElTest([FromBody] CheckSystemElTestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Проверява системата
        /// </summary>
        /// <param name="command">Команда за проверка</param>
        /// <returns>Резултат от проверката</returns>
        [HttpPost("check-system")]
        public async Task<ActionResult<TorkResultDto>> CheckSystem([FromBody] CheckSystemCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Генерира/проверява ID
        /// </summary>
        /// <param name="command">Команда за генериране</param>
        /// <returns>Резултат от операцията</returns>
        [HttpPost("generate-id")]
        public async Task<ActionResult<TorkResultDto>> GenerateId([FromBody] GenerateIdCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Актуализира ToplamV769 с bypass
        /// </summary>
        /// <param name="command">Команда за актуализация</param>
        /// <returns>Резултат от актуализацията</returns>
        [HttpPost("update-toplam-v769-bypass")]
        public async Task<ActionResult<TorkResultDto>> UpdateToplamV769Bypass([FromBody] UpdateToplamV769BypassCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Добавя донаним
        /// </summary>
        /// <param name="command">Команда за добавяне</param>
        /// <returns>Резултат от операцията</returns>
        [HttpPost("add-donanim-count")]
        public async Task<ActionResult<TorkResultDto>> AddDonanimCount([FromBody] AddDonanimCountCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Добавя донаним (сигурност)
        /// </summary>
        /// <param name="command">Команда за добавяне</param>
        /// <returns>Резултат от операцията</returns>
        [HttpPost("add-donanim-count-sigorta")]
        public async Task<ActionResult<TorkResultDto>> AddDonanimCountSigorta([FromBody] AddDonanimCountSigortaCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Печата ElTest баркод
        /// </summary>
        /// <param name="command">Команда за печат</param>
        /// <returns>Резултат от операцията</returns>
        [HttpPost("print-eltest-barcode")]
        public async Task<ActionResult<TorkResultDto>> PrintElTestBarcode([FromBody] PrintElTestBarcodeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Печата баркод
        /// </summary>
        /// <param name="command">Команда за печат</param>
        /// <returns>Резултат от операцията</returns>
        [HttpPost("print-barcode")]
        public async Task<ActionResult<TorkResultDto>> PrintBarcode([FromBody] PrintBarcodeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Проверява дали Alert е заключен
        /// </summary>
        /// <param name="command">Команда за проверка</param>
        /// <returns>Резултат от проверката</returns>
        [HttpPost("check-alert-gk-locked")]
        public async Task<ActionResult<CheckAlertGkLockedResult>> CheckAlertGkLocked([FromBody] CheckAlertGkLockedCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
