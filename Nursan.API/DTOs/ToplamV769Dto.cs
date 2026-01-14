namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за ToplamV769 операция
    /// </summary>
    public class ToplamV769Dto
    {
        /// <summary>
        /// Баркод
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Сицил (за Gromet операция)
        /// </summary>
        public string? Sicil { get; set; }

        /// <summary>
        /// Име на смяна
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
