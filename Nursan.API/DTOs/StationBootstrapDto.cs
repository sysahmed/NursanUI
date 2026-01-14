using System.Collections.Generic;

namespace Nursan.API.DTOs
{
    /// <summary>
    /// Bootstrap пакет с контекст на станцията и правила за баркодове/печата.
    /// Използва се от клиентите (ElTestvApi) при старт.
    /// </summary>
    public class StationBootstrapDto
    {
        /// <summary>
        /// Контекст за станцията (машина/станция/смяна/фамилия/етап).
        /// </summary>
        public StationContextDto Station { get; set; }

        /// <summary>
        /// Правила кои баркодове да се четат на тази станция (в реда им).
        /// </summary>
        public List<BarcodeRuleDto> BarcodeRules { get; set; }

        /// <summary>
        /// Конфигурация за печат (ако станцията има SyBarcodeOut).
        /// </summary>
        public PrintConfigDto PrintConfig { get; set; }
    }

    /// <summary>
    /// Контекст на станция, еквивалентен на това което се извлича в Program.cs (локалната версия).
    /// </summary>
    public class StationContextDto
    {
        public int MakineId { get; set; }
        public string MakineName { get; set; }

        public int IstasyonId { get; set; }
        public string IstasyonName { get; set; }

        public int? VardiyaId { get; set; }
        public string VardiyaName { get; set; }

        public int? ModulerYapiId { get; set; }
        public string ModulerYapiEtap { get; set; }

        public int? FamilyId { get; set; }
        public string FamilyName { get; set; }
    }

    /// <summary>
    /// DTO за правило/шаблон на входящ баркод (SyBarcodeInput).
    /// </summary>
    public class BarcodeRuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ParcalamaChar { get; set; }
        public string OzelChar { get; set; }

        public string RegexString { get; set; }
        public string RegexInt { get; set; }

        public int? PadLeft { get; set; }
    }

    /// <summary>
    /// Конфигурация за печат на етикет: barcode out + printer.
    /// </summary>
    public class PrintConfigDto
    {
        public BarcodeOutDto BarcodeOut { get; set; }
        public PrinterDto Printer { get; set; }
    }

    public class BarcodeOutDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PrinetrId { get; set; }
        public string RegexString { get; set; }
        public string RegexInt { get; set; }
        public int? PadLeft { get; set; }
    }

    public class PrinterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Interface { get; set; }
        public string PrintngFile { get; set; }
    }
}


