using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nursan.API.DTOs;
using Nursan.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursan.API.Controllers
{
    /// <summary>
    /// Endpoint за зареждане на "bootstrap" конфигурация за станция при старт на клиента.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/station")]
    public class StationBootstrapController : ControllerBase
    {
        private readonly UretimOtomasyonContext _context;

        public StationBootstrapController(UretimOtomasyonContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Връща контекст на станцията и правила за баркодове/печата според machineName.
        /// Изисква валиден X-API-Key (проверява се от middleware).
        /// </summary>
        [HttpGet("bootstrap")]
        public async Task<ActionResult<StationBootstrapDto>> GetBootstrap([FromQuery] string machineName)
        {
            if (string.IsNullOrWhiteSpace(machineName))
            {
                return BadRequest("machineName е задължителен.");
            }

            var stationRow = await (
                from i in _context.UrIstasyons.AsNoTracking()
                join p in _context.OpMashins.AsNoTracking() on i.MashinId equals p.Id
                join f in _context.OrFamilies.AsNoTracking() on i.FamilyId equals f.Id into ff
                from f in ff.DefaultIfEmpty()
                join m in _context.UrModulerYapis.AsNoTracking() on i.ModulerYapiId equals m.Id into mm
                from m in mm.DefaultIfEmpty()
                join v in _context.UrVardiyas.AsNoTracking() on i.VardiyaId equals v.Id into vv
                from v in vv.DefaultIfEmpty()
                where p.MasineName == machineName && i.Activ == true
                select new
                {
                    IstasyonId = i.Id,
                    IstasyonName = i.Name,
                    MakineId = p.Id,
                    MakineName = p.MasineName,
                    VardiyaId = (int?)i.VardiyaId,
                    VardiyaName = v != null ? v.Name : null,
                    ModulerYapiId = (int?)i.ModulerYapiId,
                    ModulerYapiEtap = m != null ? m.Etap : null,
                    FamilyId = (int?)i.FamilyId,
                    FamilyName = f != null ? f.FamilyName : null,
                    SyBarcodeOutId = (int?)i.SyBarcodeOutId
                }).FirstOrDefaultAsync();

            if (stationRow == null)
            {
                return NotFound($"Не е намерена активна станция за машина '{machineName}'.");
            }

            List<int> sysBarcodeInIds = await _context.SyBarcodeInCrossIstasyons.AsNoTracking()
                .Where(x => x.UrIstasyonId == stationRow.IstasyonId)
                .OrderBy(x => x.Id)
                .Where(x => x.SysBarcodeInId.HasValue)
                .Select(x => x.SysBarcodeInId.Value)
                .ToListAsync();

            List<BarcodeRuleDto> rules = new List<BarcodeRuleDto>();
            if (sysBarcodeInIds.Count > 0)
            {
                var inputs = await _context.SyBarcodeInputs.AsNoTracking()
                    .Where(x => sysBarcodeInIds.Contains(x.Id))
                    .ToListAsync();

                // Подреждаме в реда на Cross таблицата
                foreach (int id in sysBarcodeInIds)
                {
                    var input = inputs.FirstOrDefault(x => x.Id == id);
                    if (input == null)
                    {
                        continue;
                    }

                    rules.Add(new BarcodeRuleDto
                    {
                        Id = input.Id,
                        Name = input.Name,
                        ParcalamaChar = input.ParcalamaChar.HasValue ? input.ParcalamaChar.Value.ToString() : null,
                        OzelChar = input.OzelChar,
                        RegexString = input.RegexString,
                        RegexInt = input.RegexInt,
                        PadLeft = input.PadLeft
                    });
                }
            }

            PrintConfigDto printConfig = null;
            if (stationRow.SyBarcodeOutId.HasValue)
            {
                var barcodeOut = await _context.SyBarcodeOuts.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == stationRow.SyBarcodeOutId.Value);

                if (barcodeOut != null)
                {
                    var printer = barcodeOut.PrinetrId.HasValue
                        ? await _context.SyPrinters.AsNoTracking().FirstOrDefaultAsync(x => x.Id == barcodeOut.PrinetrId.Value)
                        : null;

                    printConfig = new PrintConfigDto
                    {
                        BarcodeOut = new BarcodeOutDto
                        {
                            Id = barcodeOut.Id,
                            Name = barcodeOut.Name,
                            PrinetrId = barcodeOut.PrinetrId,
                            RegexString = barcodeOut.RegexString,
                            RegexInt = barcodeOut.RegexInt,
                            PadLeft = barcodeOut.PadLeft
                        },
                        Printer = printer == null ? null : new PrinterDto
                        {
                            Id = printer.Id,
                            Name = printer.Name,
                            Ip = printer.Ip,
                            Interface = printer.Interface,
                            PrintngFile = printer.PrintngFile
                        }
                    };
                }
            }

            StationBootstrapDto dto = new StationBootstrapDto
            {
                Station = new StationContextDto
                {
                    MakineId = stationRow.MakineId,
                    MakineName = stationRow.MakineName,
                    IstasyonId = stationRow.IstasyonId,
                    IstasyonName = stationRow.IstasyonName,
                    VardiyaId = stationRow.VardiyaId,
                    VardiyaName = stationRow.VardiyaName,
                    ModulerYapiId = stationRow.ModulerYapiId,
                    ModulerYapiEtap = stationRow.ModulerYapiEtap,
                    FamilyId = stationRow.FamilyId,
                    FamilyName = stationRow.FamilyName
                },
                BarcodeRules = rules,
                PrintConfig = printConfig
            };

            return Ok(dto);
        }
    }
}


