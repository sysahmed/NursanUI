using Microsoft.EntityFrameworkCore;

namespace Nursan.API.Extensions
{
    /// <summary>
    /// Extension методи за настройка на DbContext
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Настройва DbContext с connection string, като гарантира че не се използва OnConfiguring
        /// </summary>
        public static void ConfigureDbContext<TContext>(
            this DbContextOptionsBuilder<TContext> optionsBuilder,
            string connectionString) where TContext : DbContext
        {
            // Използваме UseSqlServer преди всички други настройки, за да гарантираме че OnConfiguring няма да се извиква
            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(TContext).Assembly.GetName().Name);
            });
            
            // Допълнителни настройки
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
