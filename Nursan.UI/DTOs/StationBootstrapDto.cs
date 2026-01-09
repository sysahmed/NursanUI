using System.Collections.Generic;

namespace Nursan.UI.DTOs
{
    /// <summary>
    /// Bootstrap пакет с контекст на станцията и правила за баркодове/печата.
    /// </summary>
    public class StationBootstrapDto
    {
        public StationContextDto Station { get; set; }
        public List<BarcodeRuleDto> BarcodeRules { get; set; }
        public PrintConfigDto PrintConfig { get; set; }
    }

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


