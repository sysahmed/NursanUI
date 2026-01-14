namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за резултат от Tork операция
    /// </summary>
    public class TorkResultDto
    {
        /// <summary>
        /// Дали операцията е успешна
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Съобщение за резултата
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Данни (ако има)
        /// </summary>
        public object? Data { get; set; }
    }
}
