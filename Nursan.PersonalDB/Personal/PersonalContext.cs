using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Nursan.XMLTools;

namespace Nursan.PersonalDB.Personal;

public partial class PersonalContext : DbContext
{
    public PersonalContext()
    {
    }

    public PersonalContext(DbContextOptions<PersonalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Arkaplan> Arkaplans { get; set; }

    public virtual DbSet<ArkaplanMasa> ArkaplanMasas { get; set; }

    public virtual DbSet<ArkaplanTsk> ArkaplanTsks { get; set; }

    public virtual DbSet<ArkaplanVardiya> ArkaplanVardiyas { get; set; }

    public virtual DbSet<ArklaplanSicil> ArklaplanSicils { get; set; }

    public virtual DbSet<Bolge> Bolges { get; set; }

    public virtual DbSet<BolgeArkaplan> BolgeArkaplans { get; set; }

    public virtual DbSet<BolgeKstEk> BolgeKstEks { get; set; }

    public virtual DbSet<BolgeMashinkst> BolgeMashinksts { get; set; }

    public virtual DbSet<Controller> Controllers { get; set; }

    public virtual DbSet<DepartamentView> DepartamentViews { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Faktura> Fakturas { get; set; }

    public virtual DbSet<Hat> Hats { get; set; }

    public virtual DbSet<InternetSicil> InternetSicils { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Konveyor> Konveyors { get; set; }

    public virtual DbSet<KonveyorPersonal> KonveyorPersonals { get; set; }

    public virtual DbSet<Kst> Ksts { get; set; }

    public virtual DbSet<KstEk> KstEks { get; set; }

    public virtual DbSet<Locasyon> Locasyons { get; set; }

    public virtual DbSet<NrjLog> NrjLogs { get; set; }

    public virtual DbSet<Oneri> Oneris { get; set; }

    public virtual DbSet<OperatorVardiya> OperatorVardiyas { get; set; }

    public virtual DbSet<Personal> Personals { get; set; }

    public virtual DbSet<PersonalName> PersonalNames { get; set; }

    public virtual DbSet<PersonalNew> PersonalNews { get; set; }

    public virtual DbSet<PersonalTakip> PersonalTakips { get; set; }

    public virtual DbSet<Saat> Saats { get; set; }

    public virtual DbSet<Scaflar> Scaflars { get; set; }

    public virtual DbSet<Vardiya> Vardiyas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer($"Server={XMLSeverIp.XmlServerIP};Database=Personal;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Arkaplan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ARKAPLAN__3213E83F7167D3BD");

            entity.ToTable("ARKAPLAN");

            entity.HasIndex(e => new { e.ArkaplanVardiya, e.BolgelerId }, "Bolgerkaplan").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BolgelerId).HasColumnName("BolgelerID");
            entity.Property(e => e.Padi).HasColumnName("PAdi");
            entity.Property(e => e.PsoyAdi).HasColumnName("PSoyAdi");
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ArkaplanMasa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ArkaplanMasaName");

            entity.ToTable("ArkaplanMasa");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArkaplanMasaName).HasMaxLength(50);
        });

        modelBuilder.Entity<ArkaplanTsk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ArkaplanMasa");

            entity.ToTable("ArkaplanTSK");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ArkaplanMasa).ValueGeneratedOnAdd();
            entity.Property(e => e.Sicil).HasColumnName("SIcil");
            entity.Property(e => e.UnicId).HasColumnName("UnicID");
        });

        modelBuilder.Entity<ArkaplanVardiya>(entity =>
        {
            entity.ToTable("ArkaplanVardiya");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ArkaplanVardiyaName).HasMaxLength(50);
        });

        modelBuilder.Entity<ArklaplanSicil>(entity =>
        {
            entity.ToTable("ArklaplanSicil");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Operator1).HasMaxLength(50);
            entity.Property(e => e.Operator2).HasMaxLength(50);
            entity.Property(e => e.Operator3).HasMaxLength(50);
            entity.Property(e => e.Operator4).HasMaxLength(50);
            entity.Property(e => e.Operator5).HasMaxLength(50);
            entity.Property(e => e.Operator6).HasMaxLength(50);
            entity.Property(e => e.Text).HasColumnType("text");
            entity.Property(e => e.UnikId)
                .HasMaxLength(20)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("UnikID");
            entity.Property(e => e.Vardiya).HasMaxLength(10);
        });

        modelBuilder.Entity<Bolge>(entity =>
        {
            entity.ToTable("Bolge");

            entity.Property(e => e.BolgeId).HasColumnName("BolgeID");
            entity.Property(e => e.BolgeName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<BolgeArkaplan>(entity =>
        {
            entity.HasKey(e => e.BolgeId).HasName("PK__BOLGE_AR__15A917326D9742D9");

            entity.ToTable("BOLGE_ARKAPLAN");

            entity.Property(e => e.BolgeId).HasColumnName("BolgeID");
            entity.Property(e => e.BolgeName).HasMaxLength(50);
        });

        modelBuilder.Entity<BolgeKstEk>(entity =>
        {
            entity.HasKey(e => e.BolgeId).HasName("PK__BOLGE_KS__15A9173269C6B1F5");

            entity.ToTable("BOLGE_KST_EK");

            entity.Property(e => e.BolgeId).HasColumnName("BolgeID");
            entity.Property(e => e.BolgeName).HasMaxLength(50);
        });

        modelBuilder.Entity<BolgeMashinkst>(entity =>
        {
            entity.HasKey(e => e.BolgeId);

            entity.ToTable("BOLGE_MASHINKST");

            entity.Property(e => e.BolgeId).HasColumnName("BolgeID");
            entity.Property(e => e.BolgeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Controller>(entity =>
        {
            entity.HasKey(e => e.ControllerId).HasName("PK__Controll__3AEFC43B50C5FA01");

            entity.ToTable("Controller");

            entity.Property(e => e.ControllerId)
                .ValueGeneratedNever()
                .HasColumnName("CONTROLLER_ID");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.Rfid1Worktime).HasColumnName("RFID1_WORKTIME");
            entity.Property(e => e.Rfid2Worktime).HasColumnName("RFID2_WORKTIME");
        });

        modelBuilder.Entity<DepartamentView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DepartamentView");

            entity.Property(e => e.Departament)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FabrikaDisindaPersonal).HasColumnName("Fabrika Disinda Personal");
            entity.Property(e => e.FabrikaIcindePersonal).HasColumnName("Fabrika Icinde Personal");
            entity.Property(e => e.ToplamPersonal).HasColumnName("Toplam   Personal");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__63E6136254968AE5");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId)
                .ValueGeneratedNever()
                .HasColumnName("DEPARTMENT_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Nomer).HasColumnName("NOMER");
        });

        modelBuilder.Entity<Faktura>(entity =>
        {
            entity.ToTable("Faktura");

            entity.Property(e => e.Bgid)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("BGID");
            entity.Property(e => e.DataCreate).HasColumnType("datetime");
            entity.Property(e => e.DoId)
                .HasMaxLength(50)
                .HasColumnName("DoID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
        });

        modelBuilder.Entity<Hat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_KonveyorNO");

            entity.ToTable("Hat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Hat1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("Hat");
            entity.Property(e => e.KonveyorNo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("KonveyorNO");
        });

        modelBuilder.Entity<InternetSicil>(entity =>
        {
            entity.ToTable("InternetSicil");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bdate).HasMaxLength(4);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Sigil).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__Job__2AC9D60A58671BC9");

            entity.ToTable("Job");

            entity.Property(e => e.JobId)
                .ValueGeneratedNever()
                .HasColumnName("JOB_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Konveyor>(entity =>
        {
            entity.ToTable("Konveyor", tb => tb.HasTrigger("UpdateSilme"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BolgeId).HasColumnName("BolgeID");
            entity.Property(e => e.Durum).HasComment("0");
            entity.Property(e => e.Konveyor1).HasColumnName("Konveyor");
            entity.Property(e => e.Padi).HasColumnName("PAdi");
            entity.Property(e => e.PsoyAdi).HasColumnName("PSoyAdi");
            entity.Property(e => e.Sicil).HasColumnName("SIcil");
            entity.Property(e => e.SicilGun).HasColumnName("SIcilGun");
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasComment("")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Konveyor1Navigation).WithMany(p => p.Konveyors)
                .HasForeignKey(d => d.Konveyor1)
                .HasConstraintName("FK_Konveyor_Hat1");
        });

        modelBuilder.Entity<KonveyorPersonal>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("KonveyorPersonal");

            entity.Property(e => e.BolgeId).HasColumnName("BolgeID");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Padi).HasColumnName("PAdi");
            entity.Property(e => e.PsoyAdi).HasColumnName("PSoyAdi");
            entity.Property(e => e.Sicil).HasColumnName("SIcil");
            entity.Property(e => e.SicilGun).HasColumnName("SIcilGun");
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Kst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KST__3213E83F5E54FF49");

            entity.ToTable("KST");

            entity.HasIndex(e => new { e.KstVardiya, e.BolgelerId }, "IX_KST").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BolgelerId).HasColumnName("BolgelerID");
            entity.Property(e => e.Padi).HasColumnName("PAdi");
            entity.Property(e => e.PsoyAdi).HasColumnName("PSoyAdi");
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<KstEk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KST_EK__3213E83F6225902D");

            entity.ToTable("KST_EK");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BolgelerId).HasColumnName("BolgelerID");
            entity.Property(e => e.Padi).HasColumnName("PAdi");
            entity.Property(e => e.PsoyAdi).HasColumnName("PSoyAdi");
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Bolgeler).WithMany(p => p.KstEks)
                .HasForeignKey(d => d.BolgelerId)
                .HasConstraintName("FK_KST_EK_BOLGE_KST_EK");

            entity.HasOne(d => d.KstEkVardiyaNavigation).WithMany(p => p.KstEks)
                .HasForeignKey(d => d.KstEkVardiya)
                .HasConstraintName("FK_KST_EK_Vardiya");
        });

        modelBuilder.Entity<Locasyon>(entity =>
        {
            entity.ToTable("Locasyon");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Locasyon1)
                .HasMaxLength(50)
                .HasColumnName("Locasyon");
        });

        modelBuilder.Entity<NrjLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__nrj_log__3214EC273DB3258D");

            entity.ToTable("nrj_log", tb =>
                {
                    tb.HasTrigger("OtoKayitGuncelle");
                    tb.HasTrigger("OtoKayitGuncelleKonveyorPersdonal");
                });

            entity.HasIndex(e => e.XrefId, "indID");

            entity.HasIndex(e => e.UserId, "indUSERID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CardId).HasColumnName("CARD_ID");
            entity.Property(e => e.ControllerId).HasColumnName("CONTROLLER_ID");
            entity.Property(e => e.InOut).HasColumnName("IN_OUT");
            entity.Property(e => e.Timestamp).HasColumnName("TIMESTAMP");
            entity.Property(e => e.UserCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_CODE");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.XrefId).HasColumnName("XREF_ID");
        });

        modelBuilder.Entity<Oneri>(entity =>
        {
            entity.ToTable("Oneri");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ad).HasMaxLength(10);
            entity.Property(e => e.Oneri1)
                .HasColumnType("text")
                .HasColumnName("Oneri");
            entity.Property(e => e.Sicil).HasMaxLength(10);
            entity.Property(e => e.Soyad).HasMaxLength(10);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<OperatorVardiya>(entity =>
        {
            entity.ToTable("OperatorVardiya");

            entity.HasIndex(e => new { e.Sicil, e.UnikVardiya }, "IX_OperatorVardiya").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CalismaHat).HasMaxLength(20);
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UnikVardiya).HasMaxLength(8);
            entity.Property(e => e.Vardiya).HasMaxLength(20);
        });

        modelBuilder.Entity<Personal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Personal__3214EC27308E3499");

            entity.ToTable("Personal", tb => tb.HasTrigger("LastIndex"));

            entity.HasIndex(e => e.UserId, "indUSERID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CardId).HasColumnName("CARD_ID");
            entity.Property(e => e.Department).HasColumnName("DEPARTMENT");
            entity.Property(e => e.DirChange)
                .HasColumnType("datetime")
                .HasColumnName("DIR_CHANGE");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.JobPosition).HasColumnName("JOB_POSITION");
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

            entity.HasOne(d => d.DepartmentNavigation).WithMany(p => p.Personals)
                .HasForeignKey(d => d.Department)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Personal_Department");
        });

        modelBuilder.Entity<PersonalName>(entity =>
        {
            entity.HasKey(e => e.SicilId).HasName("PK_PersonalName_1");

            entity.ToTable("PersonalName");

            entity.Property(e => e.SicilId)
                .ValueGeneratedNever()
                .HasColumnName("SicilID");
            entity.Property(e => e.Adi)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CikisAn).HasColumnType("datetime");
            entity.Property(e => e.GirisAn).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.SicilGelen).HasMaxLength(50);
            entity.Property(e => e.SoyAdi)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Tarih).HasColumnType("date");
        });

        modelBuilder.Entity<PersonalNew>(entity =>
        {
            entity.HasKey(e => e.Sicilno);

            entity.ToTable("PersonalNew");

            entity.Property(e => e.Sicilno)
                .ValueGeneratedNever()
                .HasColumnName("SICILNO");
            entity.Property(e => e.Adi)
                .HasMaxLength(25)
                .HasColumnName("ADI");
            entity.Property(e => e.Bolge)
                .HasMaxLength(25)
                .HasColumnName("BOLGE");
            entity.Property(e => e.Hat)
                .HasMaxLength(25)
                .HasColumnName("HAT");
            entity.Property(e => e.Ikamet)
                .HasMaxLength(25)
                .HasColumnName("IKAMET");
            entity.Property(e => e.Soyadi)
                .HasMaxLength(25)
                .HasColumnName("SOYADI");
        });

        modelBuilder.Entity<PersonalTakip>(entity =>
        {
            entity.ToTable("PersonalTakip");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("id");
        });

        modelBuilder.Entity<Saat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SAAT_1");

            entity.ToTable("SAAT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CikisAn).HasColumnType("datetime");
            entity.Property(e => e.GirisAn).HasColumnType("datetime");
            entity.Property(e => e.Saat1).HasColumnName("Saat");
            entity.Property(e => e.SicilId).HasColumnName("SicilID");
            entity.Property(e => e.Tarih).HasColumnType("date");
        });

        modelBuilder.Entity<Scaflar>(entity =>
        {
            entity.ToTable("Scaflar");

            entity.Property(e => e.Sicil).HasMaxLength(10);
            entity.Property(e => e.VerilmeTarih).HasColumnType("datetime");

            entity.HasOne(d => d.LocasyonNavigation).WithMany(p => p.Scaflars)
                .HasForeignKey(d => d.Locasyon)
                .HasConstraintName("FK_Scaflar_Locasyon");
        });

        modelBuilder.Entity<Vardiya>(entity =>
        {
            entity.ToTable("Vardiya");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Vardiya1)
                .HasMaxLength(50)
                .HasColumnName("Vardiya");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
