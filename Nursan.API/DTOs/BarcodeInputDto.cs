namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за входен баркод
    /// </summary>
    public class BarcodeInputDto
    {
        /// <summary>
        /// ID на баркод входа
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Име на баркода
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Съдържание на баркода
        /// </summary>
        public string BarcodeIcerik { get; set; }

        /// <summary>
        /// Специален символ
        /// </summary>
        public string? OzelChar { get; set; }

        /// <summary>
        /// Символ за разделяне
        /// </summary>
        public char? ParcalamaChar { get; set; }

        /// <summary>
        /// Regex за цели числа
        /// </summary>
        public string? RegexInt { get; set; }

        /// <summary>
        /// Regex за низове
        /// </summary>
        public string? RegexString { get; set; }

        /// <summary>
        /// Padding left
        /// </summary>
        public int? PadLeft { get; set; }

        /// <summary>
        /// ID на принтера
        /// </summary>
        public int? PrinterId { get; set; }
    }
}
