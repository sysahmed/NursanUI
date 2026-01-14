namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за отговор от сканиране на баркод
    /// </summary>
    public class BarcodeScanResponseDto
    {
        /// <summary>
        /// Успешна ли е операцията
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Съобщение за резултата
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// ID на донаним (ако е прочетен успешно)
        /// </summary>
        public int? DonanimId { get; set; }

        /// <summary>
        /// Референс на донаним
        /// </summary>
        public string? DonanimReferans { get; set; }

        /// <summary>
        /// Текуща станция ID
        /// </summary>
        public int? CurrentStationId { get; set; }

        /// <summary>
        /// Следваща станция ID (ако е налична)
        /// </summary>
        public int? NextStationId { get; set; }

        /// <summary>
        /// Предишна станция ID (ако не е прочетена)
        /// </summary>
        public int? PreviousStationId { get; set; }

        /// <summary>
        /// Флаг дали трябва да се принтира баркод
        /// </summary>
        public bool ShouldPrintBarcode { get; set; }

        /// <summary>
        /// Данни за принтиране на баркод (ако ShouldPrintBarcode = true)
        /// </summary>
        public PrintBarcodeDataDto? PrintData { get; set; }
    }

    /// <summary>
    /// Данни за принтиране на баркод
    /// </summary>
    public class PrintBarcodeDataDto
    {
        public string Barcode { get; set; } = string.Empty;
        public int PrinterId { get; set; }
        public string PrinterName { get; set; } = string.Empty;
        public string PrinterIp { get; set; } = string.Empty;
        public string PrintTemplate { get; set; } = string.Empty;
    }
}
