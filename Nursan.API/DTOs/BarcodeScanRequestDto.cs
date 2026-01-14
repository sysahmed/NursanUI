using System.ComponentModel.DataAnnotations;

namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за заявка за сканиране на баркод
    /// </summary>
    public class BarcodeScanRequestDto
    {
        /// <summary>
        /// Име на машината (MachineName)
        /// </summary>
        [Required(ErrorMessage = "MachineName е задължителен")]
        public string MachineName { get; set; } = string.Empty;

        /// <summary>
        /// Сканиран баркод
        /// </summary>
        [Required(ErrorMessage = "Barcode е задължителен")]
        public string Barcode { get; set; } = string.Empty;

        /// <summary>
        /// Име на смяната (Vardiya)
        /// </summary>
        public string? VardiyaName { get; set; }

        /// <summary>
        /// Сицил на оператора (опционално)
        /// </summary>
        public string? Sicil { get; set; }

        /// <summary>
        /// Тип на баркода (Masa, First, Final, Tork и т.н.)
        /// </summary>
        public string? BarcodeType { get; set; }
    }
}
