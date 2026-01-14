namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за последователност на станции (Toplam)
    /// </summary>
    public class StationSequenceDto
    {
        /// <summary>
        /// ID на станцията
        /// </summary>
        public int StationId { get; set; }

        /// <summary>
        /// Име на станцията
        /// </summary>
        public string StationName { get; set; } = string.Empty;

        /// <summary>
        /// Текущ Toplam (ред/последователност)
        /// </summary>
        public int? CurrentToplam { get; set; }

        /// <summary>
        /// Нов Toplam (за промяна)
        /// </summary>
        public int? NewToplam { get; set; }

        /// <summary>
        /// Етап на станцията (Gromet, ElTest, Tork и т.н.)
        /// </summary>
        public string? Etap { get; set; }

        /// <summary>
        /// Флаг дали станцията е активна
        /// </summary>
        public bool IsActive { get; set; }
    }
}
