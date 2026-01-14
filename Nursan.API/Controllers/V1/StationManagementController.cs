using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nursan.API.DTOs;
using Nursan.Domain.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Nursan.API.Controllers.V1
{
    /// <summary>
    /// Контролер v1 за динамично управление на станции без спиране на производството
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StationManagementController : ControllerBase
    {
        private readonly UretimOtomasyonContext _context;
        private readonly ILogger<StationManagementController> _logger;

        public StationManagementController(
            UretimOtomasyonContext context, 
            ILogger<StationManagementController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Променя последователността (Toplam) на станциите динамично
        /// Пример: Ако имаме станции с Toplam 2, 3, 4 и искаме да добавим нова станция 3,
        /// старите 3 и 4 стават 4 и 5, а новата става 3
        /// </summary>
        /// <param name="request">Заявка за промяна на последователност</param>
        /// <returns>Резултат от операцията</returns>
        [HttpPost("reorder-sequence")]
        public async Task<ActionResult> ReorderStationSequence([FromBody] StationReorderRequestDto request)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                
                try
                {
                    // Намираме целевата станция
                    var targetStation = await _context.UrIstasyons
                        .FirstOrDefaultAsync(s => s.Id == request.StationId && s.Activ == true);

                    if (targetStation == null)
                    {
                        return NotFound($"Станция с ID {request.StationId} не е намерена или не е активна");
                    }

                    // Намираме всички станции от същото Family които трябва да се преместят
                    var affectedStations = await _context.UrIstasyons
                        .Where(s => s.FamilyId == targetStation.FamilyId &&
                                   s.Toplam.HasValue &&
                                   s.Toplam >= request.NewToplam &&
                                   s.Id != targetStation.Id &&
                                   s.Activ == true)
                        .OrderByDescending(s => s.Toplam)
                        .ToListAsync();

                    // Преместваме засегнатите станции с +1
                    foreach (var station in affectedStations)
                    {
                        station.Toplam = station.Toplam.Value + 1;
                        station.UpdateDate = DateTime.UtcNow;
                        _context.UrIstasyons.Update(station);
                        _logger.LogInformation(
                            "Преместване на станция {StationId} ({StationName}) от Toplam {OldToplam} към {NewToplam}",
                            station.Id, station.Name, station.Toplam - 1, station.Toplam);
                    }

                    // Задаваме новата последователност на целевата станция
                    var oldToplam = targetStation.Toplam;
                    targetStation.Toplam = request.NewToplam;
                    targetStation.UpdateDate = DateTime.UtcNow;
                    _context.UrIstasyons.Update(targetStation);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _logger.LogInformation(
                        "Успешно преподреждане: Станция {StationId} ({StationName}) от Toplam {OldToplam} към {NewToplam}",
                        targetStation.Id, targetStation.Name, oldToplam, request.NewToplam);

                    return Ok(new
                    {
                        Success = true,
                        Message = $"Станция '{targetStation.Name}' успешно преместена от Toplam {oldToplam} към {request.NewToplam}",
                        StationId = targetStation.Id,
                        OldToplam = oldToplam,
                        NewToplam = request.NewToplam,
                        AffectedStationsCount = affectedStations.Count
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при преподреждане на станции");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Вътрешна грешка: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Добавя нова станция в последователността без спиране на производството
        /// Автоматично премества съществуващите станции
        /// </summary>
        /// <param name="request">Заявка за добавяне на станция</param>
        /// <returns>Резултат от операцията</returns>
        [HttpPost("add-station")]
        public async Task<ActionResult> AddStationToSequence([FromBody] AddStationRequestDto request)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                
                try
                {
                    // Намираме станцията
                    var station = await _context.UrIstasyons
                        .Include(s => s.Family)
                        .FirstOrDefaultAsync(s => s.Id == request.StationId);

                    if (station == null)
                    {
                        return NotFound($"Станция с ID {request.StationId} не е намерена");
                    }

                    // Ако станцията вече е активна с Toplam, използваме Reorder
                    if (station.Activ == true && station.Toplam.HasValue)
                    {
                        return BadRequest(
                            $"Станция '{station.Name}' вече е активна с Toplam {station.Toplam}. " +
                            "Използвайте 'reorder-sequence' за промяна на последователността.");
                    }

                    // Намираме всички станции които трябва да се преместят
                    var affectedStations = await _context.UrIstasyons
                        .Where(s => s.FamilyId == station.FamilyId &&
                                   s.Toplam.HasValue &&
                                   s.Toplam >= request.InsertAtToplam &&
                                   s.Activ == true)
                        .OrderByDescending(s => s.Toplam)
                        .ToListAsync();

                    // Преместваме засегнатите станции с +1
                    foreach (var affectedStation in affectedStations)
                    {
                        affectedStation.Toplam = affectedStation.Toplam.Value + 1;
                        affectedStation.UpdateDate = DateTime.UtcNow;
                        _context.UrIstasyons.Update(affectedStation);
                    }

                    // Активираме и позиционираме новата станция
                    station.Toplam = request.InsertAtToplam;
                    station.Activ = true;
                    station.UpdateDate = DateTime.UtcNow;
                    _context.UrIstasyons.Update(station);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _logger.LogInformation(
                        "Успешно добавена станция {StationId} ({StationName}) на позиция Toplam {Toplam}",
                        station.Id, station.Name, request.InsertAtToplam);

                    return Ok(new
                    {
                        Success = true,
                        Message = $"Станция '{station.Name}' успешно добавена на позиция Toplam {request.InsertAtToplam}",
                        StationId = station.Id,
                        Toplam = request.InsertAtToplam,
                        AffectedStationsCount = affectedStations.Count
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при добавяне на станция");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Вътрешна грешка: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Деактивира станция и премества последващите
        /// </summary>
        /// <param name="stationId">ID на станцията за деактивиране</param>
        /// <returns>Резултат от операцията</returns>
        [HttpPost("deactivate/{stationId}")]
        public async Task<ActionResult> DeactivateStation(int stationId)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                
                try
                {
                    var station = await _context.UrIstasyons
                        .FirstOrDefaultAsync(s => s.Id == stationId);

                    if (station == null)
                    {
                        return NotFound($"Станция с ID {stationId} не е намерена");
                    }

                    if (station.Activ != true || !station.Toplam.HasValue)
                    {
                        return BadRequest($"Станция '{station.Name}' вече не е активна или няма Toplam");
                    }

                    var oldToplam = station.Toplam.Value;

                    // Намираме всички следващи станции
                    var followingStations = await _context.UrIstasyons
                        .Where(s => s.FamilyId == station.FamilyId &&
                                   s.Toplam.HasValue &&
                                   s.Toplam > oldToplam &&
                                   s.Activ == true)
                        .OrderBy(s => s.Toplam)
                        .ToListAsync();

                    // Преместваме следващите станции с -1
                    foreach (var followingStation in followingStations)
                    {
                        followingStation.Toplam = followingStation.Toplam.Value - 1;
                        followingStation.UpdateDate = DateTime.UtcNow;
                        _context.UrIstasyons.Update(followingStation);
                    }

                    // Деактивираме станцията и махаме Toplam
                    station.Activ = false;
                    station.Toplam = null;
                    station.UpdateDate = DateTime.UtcNow;
                    _context.UrIstasyons.Update(station);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _logger.LogInformation(
                        "Успешно деактивирана станция {StationId} ({StationName}) с Toplam {Toplam}",
                        station.Id, station.Name, oldToplam);

                    return Ok(new
                    {
                        Success = true,
                        Message = $"Станция '{station.Name}' успешно деактивирана",
                        StationId = station.Id,
                        OldToplam = oldToplam,
                        AffectedStationsCount = followingStations.Count
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при деактивиране на станция {StationId}", stationId);
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Вътрешна грешка: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Връща текущата последователност на станциите за дадено Family
        /// </summary>
        /// <param name="familyId">ID на Family</param>
        /// <returns>Списък със станции по последователност</returns>
        [HttpGet("sequence/{familyId}")]
        public async Task<ActionResult<List<StationSequenceDto>>> GetStationSequence(int familyId)
        {
            try
            {
                var stations = await _context.UrIstasyons
                    .Include(s => s.ModulerYapi)
                    .Where(s => s.FamilyId == familyId && s.Activ == true)
                    .OrderBy(s => s.Toplam)
                    .Select(s => new StationSequenceDto
                    {
                        StationId = s.Id,
                        StationName = s.Name ?? string.Empty,
                        CurrentToplam = s.Toplam,
                        Etap = s.ModulerYapi != null ? s.ModulerYapi.Etap : null,
                        IsActive = s.Activ == true
                    })
                    .ToListAsync();

                return Ok(stations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при извличане на последователност за Family {FamilyId}", familyId);
                return StatusCode(500, new
                {
                    Success = false,
                    Message = $"Вътрешна грешка: {ex.Message}"
                });
            }
        }
    }

    /// <summary>
    /// DTO за заявка за преподреждане на станции
    /// </summary>
    public class StationReorderRequestDto
    {
        public int StationId { get; set; }
        public int NewToplam { get; set; }
    }

    /// <summary>
    /// DTO за заявка за добавяне на станция
    /// </summary>
    public class AddStationRequestDto
    {
        public int StationId { get; set; }
        public int InsertAtToplam { get; set; }
    }
}
