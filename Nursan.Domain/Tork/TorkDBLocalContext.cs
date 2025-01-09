using Microsoft.EntityFrameworkCore;

namespace Nursan.Domain.Tork
{
    public partial class TorkDBLocalContext : DbContext
    {
        private readonly string sqlConnectionString = SystemClass.XMLSeverIp.XmlServerIP();
        public TorkDBLocalContext()
        {
        }

        public TorkDBLocalContext(DbContextOptions<TorkDBLocalContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;

        }

        public virtual DbSet<NRSCLSDEG> NRSCLSDEG { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

            => optionsBuilder.UseSqlServer($"Server={sqlConnectionString};Database = UretimOtomasyon; User Id = sa; Password = wrjkd34mk22; TrustServerCertificate = True");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NRSCLSDEG>()
                .Property(e => e.RECETE_NAME)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
