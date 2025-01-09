using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Nursan.Domain.NursanBarcode;

public partial class NbgUretim05Context : DbContext
{
    public NbgUretim05Context()
    {
    }

    public NbgUretim05Context(DbContextOptions<NbgUretim05Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Aile> Ailes { get; set; }

    public virtual DbSet<Aradosya> Aradosyas { get; set; }

    public virtual DbSet<DublicateKasaNo> DublicateKasaNos { get; set; }

    public virtual DbSet<EtiketArsiv> EtiketArsivs { get; set; }

    public virtual DbSet<KaseUser> KaseUsers { get; set; }

    public virtual DbSet<Koliicidetay> Koliicidetays { get; set; }

    public virtual DbSet<SiraNo> SiraNos { get; set; }

    public virtual DbSet<UrunMaster> UrunMasters { get; set; }

    public virtual DbSet<UrunMasterOld> UrunMasterOlds { get; set; }

    public virtual DbSet<Yazici> Yazicis { get; set; }

    public virtual DbSet<YaziciKuyrugu> YaziciKuyrugus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.168.0.5;Database=NBG_URETIM_05 ;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aile>(entity =>
        {
            entity.ToTable("AILE");

            entity.Property(e => e.Ad).HasMaxLength(50);
            entity.Property(e => e.ToplamNewKasa).HasDefaultValue(true);
        });

        modelBuilder.Entity<Aradosya>(entity =>
        {
            entity.ToTable("ARADOSYA");

            entity.Property(e => e.Alckodu)
                .HasMaxLength(30)
                .HasColumnName("ALCKodu");
            entity.Property(e => e.An)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeviceId)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("DeviceID");
            entity.Property(e => e.DeviceIp)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("DeviceIP");
            entity.Property(e => e.FkUrunMasterid).HasColumnName("Fk_URUN_MASTERId");
            entity.Property(e => e.K1).HasMaxLength(10);
            entity.Property(e => e.MalzemeKodu).HasMaxLength(30);
            entity.Property(e => e.SeriNo).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(15);
        });

        modelBuilder.Entity<DublicateKasaNo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DublicateKasaNo");

            entity.Property(e => e.Nr).HasColumnName("nr");
            entity.Property(e => e.SeriNo).HasMaxLength(30);
        });

        modelBuilder.Entity<EtiketArsiv>(entity =>
        {
            entity.ToTable("ETIKET_ARSIV");

            entity.HasIndex(e => e.SeriNo, "IX_ETIKET_ARSIV_SeriNo").IsUnique();

            entity.Property(e => e.Aciklama).HasMaxLength(100);
            entity.Property(e => e.Alckodu)
                .HasMaxLength(30)
                .HasColumnName("ALCKodu");
            entity.Property(e => e.As400akt).HasColumnName("AS400AKT");
            entity.Property(e => e.As400aktan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("AS400AKTAn");
            entity.Property(e => e.BinNo).HasMaxLength(30);
            entity.Property(e => e.MalzemeKodu).HasMaxLength(30);
            entity.Property(e => e.Model).HasMaxLength(30);
            entity.Property(e => e.PartName).HasMaxLength(40);
            entity.Property(e => e.SeriNo).HasMaxLength(30);
            entity.Property(e => e.SigortaNo).HasMaxLength(40);
            entity.Property(e => e.SiraNo).HasMaxLength(30);
            entity.Property(e => e.SupplyArea).HasMaxLength(30);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(15);
            entity.Property(e => e.YazdirildiAn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<KaseUser>(entity =>
        {
            entity.HasKey(e => e.Model);

            entity.ToTable("KASE_USER");

            entity.Property(e => e.Model).HasColumnName("model");
            entity.Property(e => e.Datet)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("datet");
            entity.Property(e => e.KaseNew)
                .HasMaxLength(50)
                .HasColumnName("kase_new");
            entity.Property(e => e.KasenoOld)
                .HasMaxLength(50)
                .HasColumnName("kaseno_old");
            entity.Property(e => e.Username)
                .HasMaxLength(25)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Koliicidetay>(entity =>
        {
            entity.ToTable("KOLIICIDETAY");

            entity.Property(e => e.An)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FkEtiketArsivid).HasColumnName("Fk_ETIKET_ARSIVId");
            entity.Property(e => e.TestSeriNo).HasMaxLength(50);

            entity.HasOne(d => d.FkEtiketArsiv).WithMany(p => p.Koliicidetays)
                .HasForeignKey(d => d.FkEtiketArsivid)
                .HasConstraintName("FK_ETIKET_ARSIVKOLIICIDETAY");
        });

        modelBuilder.Entity<SiraNo>(entity =>
        {
            entity.ToTable("SIRA_NO");

            entity.Property(e => e.ProjeKodu).HasMaxLength(10);
            entity.Property(e => e.SiraNo1).HasColumnName("SiraNo");
        });

        modelBuilder.Entity<UrunMaster>(entity =>
        {
            entity.ToTable("URUN_MASTER");

            entity.Property(e => e.Abagkodu)
                .IsUnicode(false)
                .HasColumnName("ABAGKodu");
            entity.Property(e => e.Alckodu)
                .HasMaxLength(30)
                .HasColumnName("ALCKodu");
            entity.Property(e => e.BinNo).HasMaxLength(30);
            entity.Property(e => e.MalzemeKodu).HasMaxLength(30);
            entity.Property(e => e.Model).HasMaxLength(30);
            entity.Property(e => e.PartName).HasMaxLength(40);
            entity.Property(e => e.SigortaNo).HasMaxLength(40);
            entity.Property(e => e.SupplyArea).HasMaxLength(30);
        });

        modelBuilder.Entity<UrunMasterOld>(entity =>
        {
            entity.ToTable("URUN_MASTER_Old");

            entity.Property(e => e.Abagkodu)
                .IsUnicode(false)
                .HasColumnName("ABAGKodu");
            entity.Property(e => e.Alckodu)
                .HasMaxLength(30)
                .HasColumnName("ALCKodu");
            entity.Property(e => e.BinNo).HasMaxLength(30);
            entity.Property(e => e.MalzemeKodu).HasMaxLength(30);
            entity.Property(e => e.Model).HasMaxLength(30);
            entity.Property(e => e.PartName).HasMaxLength(40);
            entity.Property(e => e.SigortaNo).HasMaxLength(40);
            entity.Property(e => e.SupplyArea).HasMaxLength(30);
        });

        modelBuilder.Entity<Yazici>(entity =>
        {
            entity.ToTable("YAZICI");

            entity.Property(e => e.Ip).HasMaxLength(30);
            entity.Property(e => e.KullaniciAdi).HasMaxLength(10);
            entity.Property(e => e.OrtamDosyasi).HasMaxLength(20);
            entity.Property(e => e.OrtamKitapligi).HasMaxLength(20);
        });

        modelBuilder.Entity<YaziciKuyrugu>(entity =>
        {
            entity.ToTable("YAZICI_KUYRUGU");

            entity.Property(e => e.As400akt).HasColumnName("AS400AKT");
            entity.Property(e => e.FkEtiketArsivid).HasColumnName("Fk_ETIKET_ARSIVId");
            entity.Property(e => e.FkYaziciid).HasColumnName("Fk_YAZICIId");

            entity.HasOne(d => d.FkEtiketArsiv).WithMany(p => p.YaziciKuyrugus)
                .HasForeignKey(d => d.FkEtiketArsivid)
                .HasConstraintName("FK_ETIKET_ARSIVYAZICI_KUYRUGU");

            entity.HasOne(d => d.FkYazici).WithMany(p => p.YaziciKuyrugus)
                .HasForeignKey(d => d.FkYaziciid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_YAZICIYAZICI_KUYRUGU");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
