using Microsoft.EntityFrameworkCore;
using Nursan.Domain.SystemClass;

namespace Nursan.Domain.VideoModels
{
    /// <summary>
    /// DbContext за видео системата.
    /// </summary>
    public partial class VideoDbContext : DbContext
    {
        private readonly string? _connectionString;

        public VideoDbContext()
        {
            _connectionString = SystemClass.XMLSeverIp.XmlServerIP();
        }

        public VideoDbContext(DbContextOptions<VideoDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public VideoDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<VideoDocument> VideoDocuments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Използваме директно connection string за VideoStreaming базата
                string videoConnectionString = "Server=10.168.0.252;Database=VideoStreaming;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True;MultipleActiveResultSets=True";
                optionsBuilder.UseSqlServer(videoConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация за Video
            modelBuilder.Entity<Video>(entity =>
            {
                entity.ToTable("Videos");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.FileName)
                    .IsRequired();

                entity.Property(e => e.VideoUrl)
                    .IsRequired();

                entity.Property(e => e.AbsoluteVideoUrl);

                entity.Property(e => e.QrCodePath);

                entity.Property(e => e.UploadDate)
                    .IsRequired();

                entity.Property(e => e.UploadedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.IsMobileOptimized)
                    .HasDefaultValue(false);

                // Индекси за по-бързо търсене
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.UploadDate);
                entity.HasIndex(e => e.Title);
            });
        }
    }
}
