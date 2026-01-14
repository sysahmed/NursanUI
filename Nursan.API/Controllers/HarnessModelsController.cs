using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nursan.API.Commands;
using Nursan.API.DTOs;
using Nursan.API.Queries;

namespace Nursan.API.Controllers
{
    /// <summary>
    /// Контролер за управление на модели на кабелни комплекти
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HarnessModelsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HarnessModelsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Получава всички модели на кабелни комплекти
        /// </summary>
        /// <param name="isActive">Филтриране по активност</param>
        /// <param name="familyId">Филтриране по семейство</param>
        /// <returns>Списък с модели</returns>
        [HttpGet]
        public async Task<ActionResult<List<HarnessModelDto>>> GetHarnessModels([FromQuery] bool? isActive, [FromQuery] int? familyId)
        {
            var query = new GetHarnessModelsQuery
            {
                IsActive = isActive,
                FamilyId = familyId
            };

            var result = await _mediator.Send(query);
            var dto = _mapper.Map<List<HarnessModelDto>>(result);

            return Ok(dto);
        }

        /// <summary>
        /// Създава нов модел на кабелен комплект
        /// </summary>
        /// <param name="command">Команда за създаване</param>
        /// <returns>Създаденият модел</returns>
        [HttpPost]
        public async Task<ActionResult<HarnessModelDto>> CreateHarnessModel([FromBody] CreateHarnessModelCommand command)
        {
            var result = await _mediator.Send(command);
            var dto = _mapper.Map<HarnessModelDto>(result);

            return CreatedAtAction(nameof(GetHarnessModelById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Получава модел по идентификатор
        /// </summary>
        /// <param name="id">Идентификатор на модела</param>
        /// <returns>Модел на кабелен комплект</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<HarnessModelDto>> GetHarnessModelById(int id)
        {
            var query = new GetHarnessModelsQuery();
            var result = await _mediator.Send(query);
            var model = result.FirstOrDefault(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<HarnessModelDto>(model);
            return Ok(dto);
        }
    }
}
