using Microsoft.EntityFrameworkCore;

namespace Nursan.Domain.AmbarModels;

public partial class AmbarContext : DbContext
{
    private readonly string sqlConnectionString = SystemClass.XMLSeverIp.XmlServerIP();
    public AmbarContext()
    {
    }

    public AmbarContext(DbContextOptions<AmbarContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public virtual DbSet<Adress> Adresses { get; set; }

    public virtual DbSet<Aktarim> Aktarims { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Barcode> Barcodes { get; set; }

    public virtual DbSet<Bolge> Bolges { get; set; }

    public virtual DbSet<Faktura> Fakturas { get; set; }

    public virtual DbSet<FakturaBilgeri> FakturaBilgeris { get; set; }

    public virtual DbSet<Islemler> Islemlers { get; set; }

    public virtual DbSet<Kontragent> Kontragents { get; set; }

    public virtual DbSet<Kullanici> Kullanicis { get; set; }

    public virtual DbSet<MakineGruplari> MakineGruplaris { get; set; }

    public virtual DbSet<MakineGruplariDahil> MakineGruplariDahils { get; set; }

    public virtual DbSet<Onay> Onays { get; set; }

    public virtual DbSet<Onaylayici> Onaylayicis { get; set; }

    public virtual DbSet<PcName> PcNames { get; set; }

    public virtual DbSet<PeriodikBakim> PeriodikBakims { get; set; }

    public virtual DbSet<Personal> Personals { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductBrack> ProductBracks { get; set; }

    public virtual DbSet<ProductGroup> ProductGroups { get; set; }

    public virtual DbSet<ProductImport> ProductImports { get; set; }

    public virtual DbSet<ProductToplam> ProductToplams { get; set; }

    public virtual DbSet<Roller> Rollers { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<TalepFormu> TalepFormus { get; set; }

    public virtual DbSet<TalepGroup> TalepGroups { get; set; }

    public virtual DbSet<YedekParca> YedekParcas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

         => optionsBuilder.UseSqlServer($"Server={sqlConnectionString};Database = Ambar; User Id = sa; Password = wrjkd34mk22; TrustServerCertificate = True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adress>(entity =>
        {
            entity.ToTable("Adress");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adress1)
                .HasMaxLength(50)
                .HasColumnName("Adress");
            entity.Property(e => e.Tarih).HasColumnType("datetime");
        });

        modelBuilder.Entity<Aktarim>(entity =>
        {
            entity.ToTable("Aktarim");

            entity.HasIndex(e => e.KullaniciId, "IX_Aktarim_KullaniciID");

            entity.Property(e => e.AktarimId).HasColumnName("AktarimID");
            entity.Property(e => e.AktarimTarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IslemId).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.KullaniciId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("KullaniciID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");

            entity.HasOne(d => d.Product).WithMany(p => p.Aktarims)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Aktarim_Product");

            entity.HasOne(d => d.Store).WithMany(p => p.Aktarims)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Aktarim_Store");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Barcode>(entity =>
        {
            entity.ToTable("Barcode");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Barcode1)
                .HasMaxLength(50)
                .HasColumnName("Barcode");
            entity.Property(e => e.Onem).HasMaxLength(50);
        });

        modelBuilder.Entity<Bolge>(entity =>
        {
            entity.ToTable("Bolge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bolge1)
                .HasMaxLength(50)
                .HasColumnName("Bolge");
        });

        modelBuilder.Entity<Faktura>(entity =>
        {
            entity.ToTable("Faktura");

            entity.Property(e => e.DataCreate).HasColumnType("datetime");
            entity.Property(e => e.FaturaBilgileriId).HasColumnName("FaturaBilgileriID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.FaturaBilgileri).WithMany(p => p.Fakturas)
                .HasForeignKey(d => d.FaturaBilgileriId)
                .HasConstraintName("FK_Faktura_FakturaBilgeri");

            entity.HasOne(d => d.Product).WithMany(p => p.Fakturas)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Faktura_Product");
        });

        modelBuilder.Entity<FakturaBilgeri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FakturaBilgileri");

            entity.ToTable("FakturaBilgeri");

            entity.Property(e => e.Bgid)
                .HasMaxLength(15)
                .HasColumnName("BGID");
            entity.Property(e => e.DataCreate).HasColumnType("datetime");
            entity.Property(e => e.DoId)
                .HasMaxLength(50)
                .HasColumnName("DoID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Kontragent).WithMany(p => p.FakturaBilgeris)
                .HasForeignKey(d => d.KontragentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FakturaBilgeri_Kontragent");
        });

        modelBuilder.Entity<Islemler>(entity =>
        {
            entity.HasKey(e => e.IslemId);

            entity.ToTable("Islemler");

            entity.HasIndex(e => e.IslemPersonal, "IX_Islemler_IslemPersonal");

            entity.HasIndex(e => e.ParcaDegisimi, "IX_Islemler_ParcaDegisimi");

            entity.HasIndex(e => e.PcId, "IX_Islemler_PcID");

            entity.HasIndex(e => e.Role, "IX_Islemler_Role");

            entity.Property(e => e.IslemId).HasColumnName("IslemID");
            entity.Property(e => e.Ariza).HasMaxLength(50);
            entity.Property(e => e.BakimSicil).HasMaxLength(50);
            entity.Property(e => e.BitisTarihi).HasColumnType("datetime");
            entity.Property(e => e.Bolge).HasMaxLength(50);
            entity.Property(e => e.PcId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("PcID");
            entity.Property(e => e.Tarih).HasColumnType("datetime");

            entity.HasOne(d => d.Pc).WithMany(p => p.Islemlers)
                .HasForeignKey(d => d.PcId)
                .HasConstraintName("FK_Islemler_PcName");
        });

        modelBuilder.Entity<Kontragent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0771580961");

            entity.ToTable("Kontragent");

            entity.Property(e => e.Bgid)
                .HasMaxLength(15)
                .HasColumnName("BGID");
            entity.Property(e => e.KontragentName).HasMaxLength(50);
        });

        modelBuilder.Entity<Kullanici>(entity =>
        {
            entity.ToTable("Kullanici");

            entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");
            entity.Property(e => e.KullaniciAdi).HasMaxLength(50);
            entity.Property(e => e.KullaniciBolge).HasMaxLength(50);
            entity.Property(e => e.NormalId).HasColumnName("NormalID");
        });

        modelBuilder.Entity<MakineGruplari>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MakineGruplari_1");

            entity.ToTable("MakineGruplari");

            entity.Property(e => e.GroupName).HasMaxLength(50);
        });

        modelBuilder.Entity<MakineGruplariDahil>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MakineGruplari");

            entity.ToTable("MakineGruplariDahil");

            entity.Property(e => e.MakineNumara).HasColumnType("numeric(18, 0)");

            entity.HasOne(d => d.GroupNameNavigation).WithMany(p => p.MakineGruplariDahils)
                .HasForeignKey(d => d.GroupName)
                .HasConstraintName("FK_MakineGruplariDahil_MakineGruplari");
        });

        modelBuilder.Entity<Onay>(entity =>
        {
            entity.HasKey(e => e.OnayIid);

            entity.ToTable("Onay");

            entity.HasIndex(e => e.OnaylayiciId, "IX_Onay_OnaylayiciID");

            entity.HasIndex(e => e.ProductId, "IX_Onay_ProductID");

            entity.HasIndex(e => e.StoreId, "IX_Onay_StoreID");

            entity.Property(e => e.OnayIid).HasColumnName("OnayIID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.OnaylayiciId).HasColumnName("OnaylayiciID");
            entity.Property(e => e.PcId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("PcID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Fatura).WithMany(p => p.Onays)
                .HasForeignKey(d => d.FaturaId)
                .HasConstraintName("FK_Onay_Faktura");

            entity.HasOne(d => d.Kontragent).WithMany(p => p.Onays)
                .HasForeignKey(d => d.KontragentId)
                .HasConstraintName("FK_Onay_Kontragent");

            entity.HasOne(d => d.Onaylayici).WithMany(p => p.Onays)
                .HasForeignKey(d => d.OnaylayiciId)
                .HasConstraintName("FK_Onay_Onaylayici");

            entity.HasOne(d => d.Pc).WithMany(p => p.Onays)
                .HasForeignKey(d => d.PcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Onay_PcName");

            entity.HasOne(d => d.Product).WithMany(p => p.Onays)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Onay_Product");

            entity.HasOne(d => d.Store).WithMany(p => p.Onays)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_Onay_Store");
        });

        modelBuilder.Entity<Onaylayici>(entity =>
        {
            entity.ToTable("Onaylayici");

            entity.Property(e => e.OnaylayiciId).HasColumnName("OnaylayiciID");
            entity.Property(e => e.NormalId).HasColumnName("NormalID");
            entity.Property(e => e.OnaylayiciIsim).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.Onaylayicis)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Onaylayici_AspNetUsers");
        });

        modelBuilder.Entity<PcName>(entity =>
        {
            entity.HasKey(e => e.Pcid);

            entity.ToTable("PcName");

            entity.Property(e => e.Pcid)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("PCID");
            entity.Property(e => e.Antivirus).HasColumnName("antivirus");
            entity.Property(e => e.Officelicens).HasColumnName("officelicens");
            entity.Property(e => e.Pcipadress)
                .HasMaxLength(50)
                .HasColumnName("PCIPADRESS");
            entity.Property(e => e.Pcmak)
                .HasMaxLength(50)
                .HasColumnName("PCMAK");
            entity.Property(e => e.Pcname1)
                .HasMaxLength(50)
                .HasColumnName("PCNAME");
            entity.Property(e => e.PerosnalOtdel).HasMaxLength(50);
            entity.Property(e => e.PerosonalIme).HasMaxLength(50);
            entity.Property(e => e.Tarih).HasColumnType("datetime");
            entity.Property(e => e.Windowslicens).HasColumnName("windowslicens");
        });

        modelBuilder.Entity<PeriodikBakim>(entity =>
        {
            entity.ToTable("PeriodikBakim");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ekip).HasColumnName("EKip");
            entity.Property(e => e.Islem).HasMaxLength(50);
            entity.Property(e => e.Makine).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.PeriodikBakimKapanisTarih).HasColumnType("datetime");
            entity.Property(e => e.PeriodikBakimTarih).HasColumnType("datetime");
        });

        modelBuilder.Entity<Personal>(entity =>
        {
            entity.ToTable("Personal");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CardId).HasColumnName("CARD_ID");
            entity.Property(e => e.Department)
                .HasDefaultValueSql("((0))")
                .HasColumnName("DEPARTMENT");
            entity.Property(e => e.DirChange)
                .HasColumnType("datetime")
                .HasColumnName("DIR_CHANGE");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.JobPosition)
                .HasDefaultValueSql("((0))")
                .HasColumnName("JOB_POSITION");
            entity.Property(e => e.LastDir).HasColumnName("LAST_DIR");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.SurName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SUR_NAME");
            entity.Property(e => e.UserCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_CODE");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.ProductGroup, "IX_Product_ProductGroup");

            entity.HasIndex(e => e.ProductKullanan, "IX_Product_ProductKullanan");

            entity.HasIndex(e => e.ProductStore, "IX_Product_ProductStore");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductBarcode).HasMaxLength(50);
            entity.Property(e => e.ProductKullanan).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.ProductTarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ProductGroupNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductGroup)
                .HasConstraintName("FK_Product_ProductGroup");
        });

        modelBuilder.Entity<ProductBrack>(entity =>
        {
            entity.HasKey(e => e.BrackId);

            entity.ToTable("ProductBrack");

            entity.HasIndex(e => e.BrackKullanici, "IX_ProductBrack_BrackKullanici");

            entity.Property(e => e.BrackId).HasColumnName("BrackID");
            entity.Property(e => e.BrackKullanici).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.BrackTarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.BrackProductNavigation).WithMany(p => p.ProductBracks)
                .HasForeignKey(d => d.BrackProduct)
                .HasConstraintName("FK_ProductBrack_Product");
        });

        modelBuilder.Entity<ProductGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId);

            entity.ToTable("ProductGroup");

            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.GroupName).HasMaxLength(50);
        });

        modelBuilder.Entity<ProductImport>(entity =>
        {
            entity.ToTable("ProductImport");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Tarih).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImports)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductImport_Product");

            entity.HasOne(d => d.StoreNavigation).WithMany(p => p.ProductImports)
                .HasForeignKey(d => d.Store)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductImport_Store");
        });

        modelBuilder.Entity<ProductToplam>(entity =>
        {
            entity.ToTable("ProductToplam");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IslemId).HasColumnName("IslemID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Islem).WithMany(p => p.ProductToplams)
                .HasForeignKey(d => d.IslemId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductToplam_Islemler");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductToplams)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductToplam_Product");
        });

        modelBuilder.Entity<Roller>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("Roller");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.ToTable("Store");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.StoreName).HasMaxLength(50);
        });

        modelBuilder.Entity<TalepFormu>(entity =>
        {
            entity.ToTable("TalepFormu");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Talep).HasMaxLength(250);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TalepGroup>(entity =>
        {
            entity.ToTable("TalepGroup");

            entity.Property(e => e.TalepGroupName)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<YedekParca>(entity =>
        {
            entity.ToTable("YedekParca");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
