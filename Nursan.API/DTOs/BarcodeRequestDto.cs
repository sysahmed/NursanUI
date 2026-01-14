namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за заявка с баркодове
    /// </summary>
    public class BarcodeRequestDto
    {
        /// <summary>
        /// Масив от баркодове
        /// </summary>
        public string[] Barcodes { get; set; }

        /// <summary>
        /// Име на смяна (Vardiya)
        /// </summary>
        public string VardiyaName { get; set; }
    }
}
