namespace Nursan.API.Extensions
{
    /// <summary>
    /// Разширения за конфигурация на услуги
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавя всички необходими услуги за API
        /// </summary>
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Тук могат да се добавят допълнителни услуги
            return services;
        }
    }
}
