using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nursan.API.DTOs;
using Nursan.Domain.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nursan.API.Controllers.V1
{
    /// <summary>
    /// Контролер v1 за управление на баркодове със проверка на последователност (Toplam)
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BarcodeController : ControllerBase
    {
        private readonly UretimOtomasyonContext _context;
        private readonly ILogger<BarcodeController> _logger;

        public BarcodeController(UretimOtomasyonContext context, ILogger<BarcodeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Сканира баркод с проверка на последователност на станции (Toplam)
        /// </summary>
        /// <param name="request">Заявка за сканиране</param>
        /// <returns>Резултат от сканирането</returns>
        [HttpPost("scan")]
        public async Task<ActionResult<BarcodeScanResponseDto>> ScanBarcode([FromBody] BarcodeScanRequestDto request)
        {
            try
            {
                // Намираме станцията по MachineName
                var station = await _context.UrIstasyons
                    .Include(s => s.Mashin)
                    .Include(s => s.ModulerYapi)
                    .Include(s => s.Vardiya)
                    .Include(s => s.Family)
                    .Where(s => s.Mashin != null && 
                                s.Mashin.MasineName == request.MachineName && 
                                s.Activ == true)
                    .FirstOrDefaultAsync();

                if (station == null)
                {
                    return NotFound(new BarcodeScanResponseDto
                    {
                        Success = false,
                        Message = $"Не е намерена активна станция за машина '{request.MachineName}'"
                    });
                }

                // Проверка на последователност (Toplam)
                var sequenceCheck = await CheckStationSequenceAsync(station, request.Barcode);
                if (!sequenceCheck.IsValid)
                {
                    return BadRequest(new BarcodeScanResponseDto
                    {
                        Success = false,
                        Message = sequenceCheck.ErrorMessage,
                        PreviousStationId = sequenceCheck.PreviousStationId
                    });
                }

                // Проверка дали баркодът не е вече прочетен (еднократно прочитане)
                var duplicateCheck = await CheckBarcodeDuplicateAsync(station.Id, request.Barcode);
                if (duplicateCheck.IsDuplicate)
                {
                    return BadRequest(new BarcodeScanResponseDto
                    {
                        Success = false,
                        Message = $"Баркод '{request.Barcode}' е вече прочетен на станция {duplicateCheck.StationId}"
                    });
                }

                // Обработваме баркода според типа му
                var processResult = await ProcessBarcodeAsync(station, request);
                
                return Ok(processResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при сканиране на баркод {Barcode}", request.Barcode);
                return StatusCode(500, new BarcodeScanResponseDto
                {
                    Success = false,
                    Message = $"Вътрешна грешка: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Проверява последователността на станциите (Toplam)
        /// </summary>
        private async Task<(bool IsValid, string ErrorMessage, int? PreviousStationId)> CheckStationSequenceAsync(
            UrIstasyon currentStation, string barcode)
        {
            if (!currentStation.Toplam.HasValue)
            {
                // Ако няма Toplam, позволяваме (за обратна съвместимост)
                return (true, string.Empty, null);
            }

            var currentToplam = currentStation.Toplam.Value;

            // Намираме всички станции от същото Family с Toplam < currentToplam
            var previousStations = await _context.UrIstasyons
                .Where(s => s.FamilyId == currentStation.FamilyId &&
                           s.Toplam.HasValue &&
                           s.Toplam < currentToplam &&
                           s.Activ == true)
                .OrderByDescending(s => s.Toplam)
                .ToListAsync();

            // За всеки баркод, проверяваме дали е прочетен в предишните станции
            // Парсваме баркода за да вземем IdDonanim
            var donanimId = ExtractDonanimId(barcode);

            foreach (var prevStation in previousStations)
            {
                // Проверяваме дали този донаним е прочетен в предишната станция
                var isReadInPrevious = await _context.IzDonanimCounts
                    .AnyAsync(d => d.IdDonanim == donanimId &&
                                 d.UrIstasyonId == prevStation.Id &&
                                 d.Activ == true);

                if (!isReadInPrevious)
                {
                    return (false, 
                        $"Прочетете баркода първо в предишна станция '{prevStation.Name}' (Топлам: {prevStation.Toplam})",
                        prevStation.Id);
                }
            }

            return (true, string.Empty, null);
        }

        /// <summary>
        /// Проверява дали баркодът не е вече прочетен (еднократно прочитане)
        /// </summary>
        private async Task<(bool IsDuplicate, int? StationId)> CheckBarcodeDuplicateAsync(
            int stationId, string barcode)
        {
            var donanimId = ExtractDonanimId(barcode);

            var existing = await _context.IzDonanimCounts
                .Where(d => d.IdDonanim == donanimId && d.Activ == true)
                .FirstOrDefaultAsync();

            if (existing != null && existing.UrIstasyonId == stationId)
            {
                // Ако е прочетен на същата станция, не е дубликат (може да се обработва повторно)
                return (false, null);
            }

            if (existing != null && existing.UrIstasyonId != stationId)
            {
                // Ако е прочетен на друга станция, е дубликат
                return (true, existing.UrIstasyonId);
            }

            return (false, null);
        }

        /// <summary>
        /// Обработва баркода според неговия тип
        /// </summary>
        private async Task<BarcodeScanResponseDto> ProcessBarcodeAsync(
            UrIstasyon station, BarcodeScanRequestDto request)
        {
            // Това е опростена версия - реалната логика трябва да се извлече от TorkService
            // За сега връщаме успешен резултат
            
            var donanimId = ExtractDonanimId(request.Barcode);

            // Проверяваме дали трябва да принтираме баркод
            var shouldPrint = station.SyBarcodeOutId.HasValue;
            PrintBarcodeDataDto? printData = null;

            if (shouldPrint)
            {
                var barcodeOut = await _context.SyBarcodeOuts
                    .Include(b => b.Prinetr)
                    .FirstOrDefaultAsync(b => b.Id == station.SyBarcodeOutId.Value);

                if (barcodeOut?.Prinetr != null)
                {
                    printData = new PrintBarcodeDataDto
                    {
                        Barcode = request.Barcode,
                        PrinterId = barcodeOut.Prinetr.Id,
                        PrinterName = barcodeOut.Prinetr.Name ?? string.Empty,
                        PrinterIp = barcodeOut.Prinetr.Ip ?? string.Empty,
                        PrintTemplate = barcodeOut.Prinetr.PrintngFile ?? string.Empty
                    };
                }
            }

            // Намираме следващата станция
            var nextStation = await _context.UrIstasyons
                .Where(s => s.FamilyId == station.FamilyId &&
                           s.Toplam.HasValue &&
                           s.Toplam > station.Toplam &&
                           s.Activ == true)
                .OrderBy(s => s.Toplam)
                .FirstOrDefaultAsync();

            return new BarcodeScanResponseDto
            {
                Success = true,
                Message = "Баркод прочетен успешно",
                DonanimId = donanimId,
                CurrentStationId = station.Id,
                NextStationId = nextStation?.Id,
                ShouldPrintBarcode = shouldPrint,
                PrintData = printData
            };
        }

        /// <summary>
        /// Извлича IdDonanim от баркода
        /// </summary>
        private int ExtractDonanimId(string barcode)
        {
            // Парсваме баркода според формата: prefix-family-suffix_ID
            // Пример: R2X6-17K400-AAB_00001234 -> 1234
            var parts = barcode.Split('_');
            if (parts.Length > 1)
            {
                if (int.TryParse(parts[1], out int id))
                {
                    return id;
                }
            }

            // Fallback: опитваме се да извлечем цифри от края
            var digits = new string(barcode.Where(char.IsDigit).ToArray());
            if (int.TryParse(digits, out int fallbackId))
            {
                return fallbackId;
            }

            return 0;
        }
    }
}
