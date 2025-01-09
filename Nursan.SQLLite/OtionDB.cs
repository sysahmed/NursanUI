using Microsoft.EntityFrameworkCore;

namespace Nursan.SQLLite
{
    public class OtionDB : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder otion)
        {
            otion.UseSqlite($"Data Source ={AppDomain.CurrentDomain.BaseDirectory}OptionDB.dbVersion=3;Password=myPassword; ");
            base.OnConfiguring(otion);
        }
        public DbSet<User> Users { get; set; }
    }
}
