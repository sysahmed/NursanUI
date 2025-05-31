using Microsoft.EntityFrameworkCore;

namespace Nursan.Domain.Entity
{
    public partial class UretimOtomasyonContext : DbContext
    {
        private readonly string sqlConnectionString = SystemClass.XMLSeverIp.XmlServerIP();
        public UretimOtomasyonContext()
        {
        }

        public UretimOtomasyonContext(DbContextOptions<UretimOtomasyonContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;

        }
        public virtual DbSet<OrOzelReferans> OrOzelReferans { get; set; }
        public virtual DbSet<Arac1> Arac1s { get; set; }
        public virtual DbSet<OrAlertGk> OrAlertGk { get; set; }
        public virtual DbSet<Arac2> Arac2s { get; set; }
        public virtual DbSet<SyTicketName> SyTicketName { get; set; }
        public virtual DbSet<Arac3> Arac3s { get; set; }

        public virtual DbSet<ErErrorCode> ErErrorCodes { get; set; }

        public virtual DbSet<ErRework> ErReworks { get; set; }

        public virtual DbSet<ErTestHatalari> ErTestHatalaris { get; set; }

        public virtual DbSet<IzCoaxCableConfig> IzCoaxCableConfigs { get; set; }

        public virtual DbSet<IzCoaxCableCount> IzCoaxCableCounts { get; set; }

        public virtual DbSet<IzCoaxCableCross> IzCoaxCableCrosses { get; set; }

        public virtual DbSet<IzDonanimCount> IzDonanimCounts { get; set; }

        public virtual DbSet<UrFabrika> UrFabrikas { get; set; }
        public virtual DbSet<IzDonanimCountArhiv> IzDonanimCountArhivs { get; set; }

        public virtual DbSet<IzDonanimHedef> IzDonanimHedefs { get; set; }

        public virtual DbSet<IzGenerateId> IzGenerateIds { get; set; }

        public virtual DbSet<IzGenerateIdArhiv> IzGenerateIdArhivs { get; set; }

        public virtual DbSet<IzPaketCount> IzPaketCounts { get; set; }

        public virtual DbSet<IzToplamV769> IzToplamV769s { get; set; }

        public virtual DbSet<IzTorkDeger> IzTorkDegers { get; set; }

        public virtual DbSet<OpMashin> OpMashins { get; set; }

        public virtual DbSet<OrAlert> OrAlerts { get; set; }

        public virtual DbSet<OrAlertBaglanti> OrAlertBaglantis { get; set; }

        public virtual DbSet<OrFamily> OrFamilies { get; set; }

        public virtual DbSet<OrHarnessConfig> OrHarnessConfigs { get; set; }

        public virtual DbSet<OrHarnessModel> OrHarnessModels { get; set; }

        public virtual DbSet<SyBarcodeInCrossIstasyon> SyBarcodeInCrossIstasyons { get; set; }

        public virtual DbSet<SyBarcodeInput> SyBarcodeInputs { get; set; }

        public virtual DbSet<SyBarcodeOut> SyBarcodeOuts { get; set; }

        public virtual DbSet<SyPrinter> SyPrinters { get; set; }

        public virtual DbSet<UrIstasyon> UrIstasyons { get; set; }

        public virtual DbSet<UrKonveyorNumara> UrKonveyorNumaras { get; set; }

        public virtual DbSet<UrModulerYapi> UrModulerYapis { get; set; }
        public virtual DbSet<UrPersonalTakib> UrPersonalTakibs { get; set; }
        public virtual DbSet<UrVardiya> UrVardiyas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

            => optionsBuilder.UseSqlServer($"Server={sqlConnectionString};Database = UretimOtomasyon; User Id = sa; Password = wrjkd34mk22; TrustServerCertificate = True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ErErrorCode>(entity =>
            {
                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.ToTable("ErErrorCode");

                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.ErrorCode).HasMaxLength(4);
                entity.Property(e => e.ErrorLocation).HasMaxLength(50);
                entity.Property(e => e.ErrorName).HasMaxLength(50);
                entity.Property(e => e.UpdateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<ErRework>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_errework");

                entity.ToTable("ErRework");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CausOfFault).HasMaxLength(50);
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.ErrorCodeDefination).HasMaxLength(50);
                entity.Property(e => e.FaultGoz).HasMaxLength(20);
                entity.Property(e => e.FaultInIstasyn).HasMaxLength(15);
                entity.Property(e => e.FaultInIstayon).HasMaxLength(25);
                entity.Property(e => e.FaultRegion).HasMaxLength(20);
                entity.Property(e => e.Istasyon).HasMaxLength(20);
                entity.Property(e => e.IstasyonTarihi).HasColumnType("datetime");
                entity.Property(e => e.NextIstasyon).HasMaxLength(20);
                entity.Property(e => e.Referans).HasMaxLength(50);
                entity.Property(e => e.ReworkFixDate).HasColumnType("datetime");
                entity.Property(e => e.ReworkFixOperator).HasMaxLength(25);
                entity.Property(e => e.ReworkInDate).HasColumnType("datetime");
                entity.Property(e => e.ReworkInOperator).HasMaxLength(25);
                entity.Property(e => e.ReworkOutDate).HasColumnType("datetime");
                entity.Property(e => e.ReworkOutOperator).HasMaxLength(25);
                entity.Property(e => e.UpdateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<ErTestHatalari>(entity =>
            {
                entity.ToTable("ErTestHatalari");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.Bolge1).HasMaxLength(20);
                entity.Property(e => e.Bolge2).HasMaxLength(20);
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.Goz1).HasMaxLength(20);
                entity.Property(e => e.Goz2).HasMaxLength(20);
                entity.Property(e => e.IdDonanim).HasColumnName("idDonanim");
                entity.Property(e => e.KonVeyorTarih).HasColumnType("datetime");
                entity.Property(e => e.Konveyor).HasMaxLength(20);
                entity.Property(e => e.Onarma).HasMaxLength(20);
                entity.Property(e => e.Operator).HasMaxLength(25);
                entity.Property(e => e.Referans).HasMaxLength(25);
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
                entity.Property(e => e.Vardiya).HasMaxLength(20);
            });

            modelBuilder.Entity<IzCoaxCableConfig>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_IzCoaxCableCross");

                entity.ToTable("IzCoaxCableConfig");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CoaxCabloReferans).HasMaxLength(50);
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Supplier).HasMaxLength(50);
                entity.Property(e => e.UpdateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<IzCoaxCableCount>(entity =>
            {
                entity.ToTable("IzCoaxCableCount");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.UpdateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.CoaxCable).WithMany(p => p.IzCoaxCableCounts)
                    .HasForeignKey(d => d.CoaxCableId)
                    .HasConstraintName("FK_IzCoaxCableCount_IzCoaxCableCross");

                entity.HasOne(d => d.DonanimRederans).WithMany(p => p.IzCoaxCableCounts)
                    .HasForeignKey(d => d.DonanimRederansId)
                    .HasConstraintName("FK_IzCoaxCableCount_IzGenerateId");

                entity.HasOne(d => d.HarnessModel).WithMany(p => p.IzCoaxCableCounts)
                    .HasForeignKey(d => d.HarnessModelId)
                    .HasConstraintName("FK_IzCoaxCableCount_OrHarnessModel");

                entity.HasOne(d => d.OrPcName).WithMany(p => p.IzCoaxCableCounts)
                    .HasForeignKey(d => d.OrPcNameId)
                    .HasConstraintName("FK_IzCoaxCableCount_OpMashin");

                entity.HasOne(d => d.UrIstasyon).WithMany(p => p.IzCoaxCableCounts)
                    .HasForeignKey(d => d.UrIstasyonId)
                    .HasConstraintName("FK_IzCoaxCableCount_UrIstasyon1");

                entity.HasOne(d => d.UrVardiyaNavigation).WithMany(p => p.IzCoaxCableCounts)
                    .HasForeignKey(d => d.VardiyaId)
                    .HasConstraintName("FK_IzCoaxCableCount_UrVardiya1");
            });

            modelBuilder.Entity<IzCoaxCableCross>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_IzCoaxCableConfig");

                entity.ToTable("IzCoaxCableCross");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.UpdateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.CoaxCableBarcode).WithMany(p => p.IzCoaxCableCrosses)
                    .HasForeignKey(d => d.CoaxCableBarcodeId)
                    .HasConstraintName("FK_IzCoaxCableCross_IzCoaxCableConfig");

                entity.HasOne(d => d.HarnessModel).WithMany(p => p.IzCoaxCableCrosses)
                    .HasForeignKey(d => d.HarnessModelId)
                    .HasConstraintName("FK_IzCoaxCableConfig_OrHarnessModel");
            });

            modelBuilder.Entity<IzDonanimCount>(entity =>
            {
                entity.ToTable("IzDonanimCount");

                entity.HasIndex(e => e.Id, "IX_UrDonanimCount")
                    .IsUnique();

                entity.HasIndex(e => e.IdDonanim, "IX_UrDonanimCount_IdDonanim");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DonanimReferans).HasMaxLength(50);

                entity.Property(e => e.OrHarnessModel).HasMaxLength(50);
                entity.Property(e => e.Revork).HasDefaultValueSql("((0))");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdDonanimNavigation)
                    .WithMany(p => p.IzDonanimCounts)
                    .HasForeignKey(d => d.IdDonanim)
                    .HasConstraintName("FK_UrDonanimCount_GenerateId");

                entity.HasOne(d => d.Masa)
                    .WithMany(p => p.IzDonanimCounts)
                    .HasForeignKey(d => d.MasaId)
                    .HasConstraintName("FK_IzDonanimCount_UrKonveyorNumaras");

                entity.HasOne(d => d.Mashin)
                    .WithMany(p => p.IzDonanimCounts)
                    .HasForeignKey(d => d.MashinId)
                    .HasConstraintName("FK_IzDonanimCount_OpMashin");

                entity.HasOne(d => d.UrIstasyon)
                    .WithMany(p => p.IzDonanimCounts)
                    .HasForeignKey(d => d.UrIstasyonId)
                    .HasConstraintName("FK_IzDonanimCount_UrIstasyon");

                entity.HasOne(d => d.Vardiya)
                    .WithMany(p => p.IzDonanimCounts)
                    .HasForeignKey(d => d.VardiyaId)
                    .HasConstraintName("FK_IzDonanimCount_UrVardiya");
            });

            modelBuilder.Entity<IzToplamV769>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_toplam_1");

                entity.ToTable("IzToplamV769");

                entity.HasIndex(e => e.Id, "IX_IzToplamV769");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ID");
                entity.Property(e => e.Alert).HasDefaultValueSql("('')");
                entity.Property(e => e.Anten)
                    .HasMaxLength(150)
                    .HasColumnName("anten");
                entity.Property(e => e.Antenb).HasColumnName("antenb");
                entity.Property(e => e.Antendate)
                    .HasColumnType("datetime")
                    .HasColumnName("antendate");
                entity.Property(e => e.Antengec).HasColumnName("antengec");
                entity.Property(e => e.Antenvar)
                    .HasMaxLength(150)
                    .HasColumnName("antenvar");
                entity.Property(e => e.CustomId)
                    .HasMaxLength(50)
                    .HasColumnName("CustomID");
                entity.Property(e => e.Eltest)
                    .HasDefaultValue("")
                    .HasColumnName("eltest");
                entity.Property(e => e.Eltestb)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("eltestb");
                entity.Property(e => e.Eltestdata)
                    .HasColumnType("datetime")
                    .HasColumnName("eltestdata");
                entity.Property(e => e.Eltestgec)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("eltestgec");
                entity.Property(e => e.Elvar)
                    .HasDefaultValue("")
                    .HasColumnName("elvar");
                entity.Property(e => e.Goz)
                    .HasDefaultValue("")
                    .HasColumnName("goz");
                entity.Property(e => e.Gozb)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("gozb");
                entity.Property(e => e.Gozdata)
                    .HasColumnType("datetime")
                    .HasColumnName("gozdata");
                entity.Property(e => e.Gozgec)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("gozgec");
                entity.Property(e => e.Gozvar)
                    .HasDefaultValue("")
                    .HasColumnName("gozvar");
                entity.Property(e => e.Gromet)
                    .HasMaxLength(50)
                    .HasColumnName("gromet");
                entity.Property(e => e.Grometb).HasColumnName("grometb");
                entity.Property(e => e.Grometdata)
                    .HasColumnType("datetime")
                    .HasColumnName("grometdata");
                entity.Property(e => e.Grometgec).HasColumnName("grometgec");
                entity.Property(e => e.Grometvar)
                    .HasMaxLength(50)
                    .HasColumnName("grometvar");
                entity.Property(e => e.IdDonanim)
                    .HasMaxLength(10)
                    .HasColumnName("id_donanim");
                entity.Property(e => e.Kasa)
                    .HasDefaultValue("")
                    .HasColumnName("kasa");
                entity.Property(e => e.Kasatarih)
                    .HasColumnType("datetime")
                    .HasColumnName("kasatarih");
                entity.Property(e => e.Klipgec)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("klipgec");
                entity.Property(e => e.Kliptest)
                    .HasDefaultValue("")
                    .HasColumnName("kliptest");
                entity.Property(e => e.Kliptestb)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("kliptestb");
                entity.Property(e => e.Kliptestdata)
                    .HasColumnType("datetime")
                    .HasColumnName("kliptestdata");
                entity.Property(e => e.Klipvar)
                    .HasDefaultValue("")
                    .HasColumnName("klipvar");
                entity.Property(e => e.Koli)
                    .HasDefaultValue("")
                    .HasColumnName("koli");
                entity.Property(e => e.Kolib)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("kolib");
                entity.Property(e => e.Kolidata)
                    .HasColumnType("datetime")
                    .HasColumnName("kolidata");
                entity.Property(e => e.Kondata)
                    .HasComment("getdate()")
                    .HasColumnType("datetime")
                    .HasColumnName("kondata");
                entity.Property(e => e.Konvar)
                    .HasDefaultValue("")
                    .HasColumnName("konvar");
                entity.Property(e => e.Konveyor)
                    .HasDefaultValue("")
                    .HasColumnName("konveyor");
                entity.Property(e => e.Konveyorb).HasColumnName("konveyorb");
                entity.Property(e => e.Konveyorgec)
                    .HasDefaultValue(false)
                    .HasComment("false")
                    .HasColumnName("konveyorgec");
                entity.Property(e => e.Paketdata)
                    .HasColumnType("datetime")
                    .HasColumnName("paketdata");
                entity.Property(e => e.Paketgec)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("paketgec");
                entity.Property(e => e.Paketleme)
                    .HasDefaultValue("")
                    .HasColumnName("paketleme");
                entity.Property(e => e.Paketlemeb)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("paketlemeb");
                entity.Property(e => e.Paketvar)
                    .HasDefaultValue("")
                    .HasColumnName("paketvar");
                entity.Property(e => e.Qc)
                    .HasMaxLength(10)
                    .HasColumnName("QC");
                entity.Property(e => e.Referans)
                    .HasMaxLength(20)
                    .HasColumnName("referans");
                entity.Property(e => e.Revork)
                    .HasDefaultValue("")
                    .HasColumnName("revork");
                entity.Property(e => e.Revorkb)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("revorkb");
                entity.Property(e => e.Revorkdata)
                    .HasColumnType("datetime")
                    .HasColumnName("revorkdata");
                entity.Property(e => e.Revorkgec)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("revorkgec");
                entity.Property(e => e.Revorkta)
                    .HasDefaultValue(false)
                    .HasComment("0")
                    .HasColumnName("revorkta");
                entity.Property(e => e.Revorkvar)
                    .HasDefaultValue("")
                    .HasColumnName("revorkvar");
                entity.Property(e => e.SideCode)
                    .HasMaxLength(50)
                    .HasDefaultValue("");
                entity.Property(e => e.Sigorta)
                    .HasDefaultValue("")
                    .HasColumnName("sigorta");
                entity.Property(e => e.Sigortadate)
                    .HasColumnType("datetime")
                    .HasColumnName("sigortadate");
                entity.Property(e => e.Sigortagec).HasColumnName("sigortagec");
                entity.Property(e => e.Sigortavar)
                    .HasDefaultValue("")
                    .HasColumnName("sigortavar");
                entity.Property(e => e.Tork)
                    .HasDefaultValue("")
                    .HasColumnName("tork");
                entity.Property(e => e.Torkb).HasColumnName("torkb");
                entity.Property(e => e.Torkdate)
                    .HasColumnType("datetime")
                    .HasColumnName("torkdate");
                entity.Property(e => e.Torkgec).HasColumnName("torkgec");
                entity.Property(e => e.Torkvar)
                    .HasDefaultValue("")
                    .HasColumnName("torkvar");
            });



            modelBuilder.Entity<IzDonanimHedef>(entity =>
            {
                entity.ToTable("IzDonanimHedef");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.UpdateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.OrFamily).WithMany(p => p.IzDonanimHedefs)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("FK_IzDonanimHedef_OrFamilies");

                entity.HasOne(d => d.OrHarnessModel).WithMany(p => p.IzDonanimHedefs)
                    .HasForeignKey(d => d.HarnesModelId)
                    .HasConstraintName("FK_IzDonanimHedef_OrHarnessModel");

                entity.HasOne(d => d.Istasuon).WithMany(p => p.IzDonanimHedefs)
                    .HasForeignKey(d => d.IstasyonId)
                    .HasConstraintName("FK_IzDonanimHedef_UrIstasyon");
            });

            modelBuilder.Entity<IzGenerateId>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_GenerateId");

                entity.ToTable("IzGenerateId");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.Revork).HasDefaultValueSql("((1))");
                entity.Property(e => e.Barcode).HasMaxLength(50);
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.PFBSocket)
                    .HasMaxLength(50)
                    .HasColumnName("PFBSocket");
                entity.Property(e => e.UpdateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.HarnesModel).WithMany(p => p.IzGenerateIds)
                    .HasForeignKey(d => d.HarnesModelId)
                    .HasConstraintName("FK_IzGenerateId_OrHarnessModel");

                entity.HasOne(d => d.UrIstasyon).WithMany(p => p.IzGenerateIds)
                    .HasForeignKey(d => d.UrIstasyonId)
                    .HasConstraintName("FK_IzGenerateId_UrIstasyon");
            });
            modelBuilder.Entity<IzPaketCount>(entity =>
            {
                entity.ToTable("IzPaketCount");

                entity.HasIndex(e => e.DonanimReferans, "IOX");

                entity.HasIndex(e => e.KasaSerialNo, "IOX1");

                entity.HasIndex(e => e.CreateDate, "IOX2");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.DonanimReferans).HasMaxLength(50);
                entity.Property(e => e.KasaCreateDate).HasColumnType("datetime");
                entity.Property(e => e.KasaSerialNo).HasMaxLength(50);
                entity.Property(e => e.Koli).HasMaxLength(10);
                entity.Property(e => e.KoliCreateDate).HasColumnType("datetime");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<IzTorkDeger>(entity =>
            {
                entity.ToTable("IzTorkDeger");

                entity.Property(e => e.Aci1)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_1");
                entity.Property(e => e.Aci10)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_10");
                entity.Property(e => e.Aci11)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_11");
                entity.Property(e => e.Aci12)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_12");
                entity.Property(e => e.Aci13)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_13");
                entity.Property(e => e.Aci14)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_14");
                entity.Property(e => e.Aci15)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_15");
                entity.Property(e => e.Aci16)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_16");
                entity.Property(e => e.Aci17)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_17");
                entity.Property(e => e.Aci18)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_18");
                entity.Property(e => e.Aci19)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_19");
                entity.Property(e => e.Aci2)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_2");
                entity.Property(e => e.Aci20)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_20");
                entity.Property(e => e.Aci21)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_21");
                entity.Property(e => e.Aci22)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_22");
                entity.Property(e => e.Aci23)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_23");
                entity.Property(e => e.Aci24)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_24");
                entity.Property(e => e.Aci25)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_25");
                entity.Property(e => e.Aci26)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_26");
                entity.Property(e => e.Aci27)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_27");
                entity.Property(e => e.Aci28)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_28");
                entity.Property(e => e.Aci29)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_29");
                entity.Property(e => e.Aci3)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_3");
                entity.Property(e => e.Aci30)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_30");
                entity.Property(e => e.Aci31)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_31");
                entity.Property(e => e.Aci32)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_32");
                entity.Property(e => e.Aci33)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_33");
                entity.Property(e => e.Aci34)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_34");
                entity.Property(e => e.Aci35)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_35");
                entity.Property(e => e.Aci36)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_36");
                entity.Property(e => e.Aci37)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_37");
                entity.Property(e => e.Aci38)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_38");
                entity.Property(e => e.Aci39)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_39");
                entity.Property(e => e.Aci4)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_4");
                entity.Property(e => e.Aci40)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_40");
                entity.Property(e => e.Aci41)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_41");
                entity.Property(e => e.Aci42)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_42");
                entity.Property(e => e.Aci43)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_43");
                entity.Property(e => e.Aci44)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_44");
                entity.Property(e => e.Aci45)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_45");
                entity.Property(e => e.Aci46)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_46");
                entity.Property(e => e.Aci47)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_47");
                entity.Property(e => e.Aci48)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_48");
                entity.Property(e => e.Aci49)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_49");
                entity.Property(e => e.Aci5)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_5");
                entity.Property(e => e.Aci50)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_50");
                entity.Property(e => e.Aci6)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_6");
                entity.Property(e => e.Aci7)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_7");
                entity.Property(e => e.Aci8)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_8");
                entity.Property(e => e.Aci9)
                    .HasMaxLength(10)
                    .HasColumnName("ACI_9");
                entity.Property(e => e.Calisan)
                    .HasMaxLength(10)
                    .HasColumnName("CALISAN");
                entity.Property(e => e.Config1)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_1");
                entity.Property(e => e.Config10)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_10");
                entity.Property(e => e.Config11)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_11");
                entity.Property(e => e.Config12)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_12");
                entity.Property(e => e.Config13)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_13");
                entity.Property(e => e.Config14)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_14");
                entity.Property(e => e.Config15)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_15");
                entity.Property(e => e.Config16)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_16");
                entity.Property(e => e.Config17)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_17");
                entity.Property(e => e.Config18)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_18");
                entity.Property(e => e.Config19)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_19");
                entity.Property(e => e.Config2)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_2");
                entity.Property(e => e.Config20)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_20");
                entity.Property(e => e.Config21)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_21");
                entity.Property(e => e.Config22)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_22");
                entity.Property(e => e.Config23)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_23");
                entity.Property(e => e.Config24)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_24");
                entity.Property(e => e.Config25)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_25");
                entity.Property(e => e.Config26)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_26");
                entity.Property(e => e.Config27)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_27");
                entity.Property(e => e.Config28)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_28");
                entity.Property(e => e.Config29)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_29");
                entity.Property(e => e.Config3)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_3");
                entity.Property(e => e.Config30)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_30");
                entity.Property(e => e.Config31)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_31");
                entity.Property(e => e.Config32)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_32");
                entity.Property(e => e.Config33)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_33");
                entity.Property(e => e.Config34)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_34");
                entity.Property(e => e.Config35)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_35");
                entity.Property(e => e.Config36)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_36");
                entity.Property(e => e.Config37)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_37");
                entity.Property(e => e.Config38)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_38");
                entity.Property(e => e.Config39)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_39");
                entity.Property(e => e.Config4)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_4");
                entity.Property(e => e.Config40)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_40");
                entity.Property(e => e.Config41)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_41");
                entity.Property(e => e.Config42)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_42");
                entity.Property(e => e.Config43)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_43");
                entity.Property(e => e.Config44)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_44");
                entity.Property(e => e.Config45)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_45");
                entity.Property(e => e.Config46)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_46");
                entity.Property(e => e.Config47)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_47");
                entity.Property(e => e.Config48)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_48");
                entity.Property(e => e.Config49)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_49");
                entity.Property(e => e.Config5)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_5");
                entity.Property(e => e.Config50)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_50");
                entity.Property(e => e.Config6)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_6");
                entity.Property(e => e.Config7)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_7");
                entity.Property(e => e.Config8)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_8");
                entity.Property(e => e.Config9)
                    .HasMaxLength(10)
                    .HasColumnName("CONFIG_9");
                entity.Property(e => e.Hata1)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_1");
                entity.Property(e => e.Hata10)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_10");
                entity.Property(e => e.Hata11)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_11");
                entity.Property(e => e.Hata12)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_12");
                entity.Property(e => e.Hata13)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_13");
                entity.Property(e => e.Hata14)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_14");
                entity.Property(e => e.Hata15)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_15");
                entity.Property(e => e.Hata16)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_16");
                entity.Property(e => e.Hata17)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_17");
                entity.Property(e => e.Hata18)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_18");
                entity.Property(e => e.Hata19)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_19");
                entity.Property(e => e.Hata2)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_2");
                entity.Property(e => e.Hata20)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_20");
                entity.Property(e => e.Hata21)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_21");
                entity.Property(e => e.Hata22)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_22");
                entity.Property(e => e.Hata23)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_23");
                entity.Property(e => e.Hata24)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_24");
                entity.Property(e => e.Hata25)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_25");
                entity.Property(e => e.Hata26)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_26");
                entity.Property(e => e.Hata27)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_27");
                entity.Property(e => e.Hata28)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_28");
                entity.Property(e => e.Hata29)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_29");
                entity.Property(e => e.Hata3)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_3");
                entity.Property(e => e.Hata30)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_30");
                entity.Property(e => e.Hata31)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_31");
                entity.Property(e => e.Hata32)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_32");
                entity.Property(e => e.Hata33)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_33");
                entity.Property(e => e.Hata34)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_34");
                entity.Property(e => e.Hata35)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_35");
                entity.Property(e => e.Hata36)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_36");
                entity.Property(e => e.Hata37)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_37");
                entity.Property(e => e.Hata38)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_38");
                entity.Property(e => e.Hata39)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_39");
                entity.Property(e => e.Hata4)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_4");
                entity.Property(e => e.Hata40)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_40");
                entity.Property(e => e.Hata41)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_41");
                entity.Property(e => e.Hata42)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_42");
                entity.Property(e => e.Hata43)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_43");
                entity.Property(e => e.Hata44)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_44");
                entity.Property(e => e.Hata45)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_45");
                entity.Property(e => e.Hata46)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_46");
                entity.Property(e => e.Hata47)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_47");
                entity.Property(e => e.Hata48)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_48");
                entity.Property(e => e.Hata49)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_49");
                entity.Property(e => e.Hata5)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_5");
                entity.Property(e => e.Hata50)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_50");
                entity.Property(e => e.Hata6)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_6");
                entity.Property(e => e.Hata7)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_7");
                entity.Property(e => e.Hata8)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_8");
                entity.Property(e => e.Hata9)
                    .HasMaxLength(10)
                    .HasColumnName("HATA_9");
                entity.Property(e => e.Kacstep)
                    .HasMaxLength(10)
                    .HasColumnName("KACSTEP");
                entity.Property(e => e.Kutu1)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_1");
                entity.Property(e => e.Kutu10)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_10");
                entity.Property(e => e.Kutu11)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_11");
                entity.Property(e => e.Kutu12)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_12");
                entity.Property(e => e.Kutu13)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_13");
                entity.Property(e => e.Kutu14)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_14");
                entity.Property(e => e.Kutu15)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_15");
                entity.Property(e => e.Kutu16)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_16");
                entity.Property(e => e.Kutu17)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_17");
                entity.Property(e => e.Kutu18)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_18");
                entity.Property(e => e.Kutu19)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_19");
                entity.Property(e => e.Kutu2)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_2");
                entity.Property(e => e.Kutu20)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_20");
                entity.Property(e => e.Kutu21)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_21");
                entity.Property(e => e.Kutu22)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_22");
                entity.Property(e => e.Kutu23)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_23");
                entity.Property(e => e.Kutu24)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_24");
                entity.Property(e => e.Kutu25)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_25");
                entity.Property(e => e.Kutu26)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_26");
                entity.Property(e => e.Kutu27)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_27");
                entity.Property(e => e.Kutu28)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_28");
                entity.Property(e => e.Kutu29)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_29");
                entity.Property(e => e.Kutu3)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_3");
                entity.Property(e => e.Kutu30)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_30");
                entity.Property(e => e.Kutu31)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_31");
                entity.Property(e => e.Kutu32)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_32");
                entity.Property(e => e.Kutu33)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_33");
                entity.Property(e => e.Kutu34)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_34");
                entity.Property(e => e.Kutu35)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_35");
                entity.Property(e => e.Kutu36)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_36");
                entity.Property(e => e.Kutu37)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_37");
                entity.Property(e => e.Kutu38)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_38");
                entity.Property(e => e.Kutu39)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_39");
                entity.Property(e => e.Kutu4)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_4");
                entity.Property(e => e.Kutu40)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_40");
                entity.Property(e => e.Kutu41)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_41");
                entity.Property(e => e.Kutu42)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_42");
                entity.Property(e => e.Kutu43)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_43");
                entity.Property(e => e.Kutu44)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_44");
                entity.Property(e => e.Kutu45)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_45");
                entity.Property(e => e.Kutu46)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_46");
                entity.Property(e => e.Kutu47)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_47");
                entity.Property(e => e.Kutu48)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_48");
                entity.Property(e => e.Kutu49)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_49");
                entity.Property(e => e.Kutu5)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_5");
                entity.Property(e => e.Kutu50)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_50");
                entity.Property(e => e.Kutu6)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_6");
                entity.Property(e => e.Kutu7)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_7");
                entity.Property(e => e.Kutu8)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_8");
                entity.Property(e => e.Kutu9)
                    .HasMaxLength(10)
                    .HasColumnName("KUTU_9");
                entity.Property(e => e.Makine)
                    .HasMaxLength(20)
                    .HasColumnName("MAKINE");
                entity.Property(e => e.ReceteName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("RECETE_NAME");
                entity.Property(e => e.ReceteNo).HasColumnName("RECETE_NO");
                entity.Property(e => e.Saat).HasMaxLength(10);
                entity.Property(e => e.Tabanca1)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_1");
                entity.Property(e => e.Tabanca10)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_10");
                entity.Property(e => e.Tabanca11)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_11");
                entity.Property(e => e.Tabanca12)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_12");
                entity.Property(e => e.Tabanca13)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_13");
                entity.Property(e => e.Tabanca14)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_14");
                entity.Property(e => e.Tabanca15)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_15");
                entity.Property(e => e.Tabanca16)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_16");
                entity.Property(e => e.Tabanca17)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_17");
                entity.Property(e => e.Tabanca18)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_18");
                entity.Property(e => e.Tabanca19)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_19");
                entity.Property(e => e.Tabanca2)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_2");
                entity.Property(e => e.Tabanca20)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_20");
                entity.Property(e => e.Tabanca21)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_21");
                entity.Property(e => e.Tabanca22)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_22");
                entity.Property(e => e.Tabanca23)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_23");
                entity.Property(e => e.Tabanca24)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_24");
                entity.Property(e => e.Tabanca25)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_25");
                entity.Property(e => e.Tabanca26)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_26");
                entity.Property(e => e.Tabanca27)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_27");
                entity.Property(e => e.Tabanca28)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_28");
                entity.Property(e => e.Tabanca29)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_29");
                entity.Property(e => e.Tabanca3)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_3");
                entity.Property(e => e.Tabanca30)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_30");
                entity.Property(e => e.Tabanca31)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_31");
                entity.Property(e => e.Tabanca32)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_32");
                entity.Property(e => e.Tabanca33)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_33");
                entity.Property(e => e.Tabanca34)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_34");
                entity.Property(e => e.Tabanca35)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_35");
                entity.Property(e => e.Tabanca36)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_36");
                entity.Property(e => e.Tabanca37)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_37");
                entity.Property(e => e.Tabanca38)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_38");
                entity.Property(e => e.Tabanca39)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_39");
                entity.Property(e => e.Tabanca4)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_4");
                entity.Property(e => e.Tabanca40)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_40");
                entity.Property(e => e.Tabanca41)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_41");
                entity.Property(e => e.Tabanca42)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_42");
                entity.Property(e => e.Tabanca43)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_43");
                entity.Property(e => e.Tabanca44)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_44");
                entity.Property(e => e.Tabanca45)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_45");
                entity.Property(e => e.Tabanca46)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_46");
                entity.Property(e => e.Tabanca47)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_47");
                entity.Property(e => e.Tabanca48)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_48");
                entity.Property(e => e.Tabanca49)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_49");
                entity.Property(e => e.Tabanca5)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_5");
                entity.Property(e => e.Tabanca50)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_50");
                entity.Property(e => e.Tabanca6)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_6");
                entity.Property(e => e.Tabanca7)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_7");
                entity.Property(e => e.Tabanca8)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_8");
                entity.Property(e => e.Tabanca9)
                    .HasMaxLength(10)
                    .HasColumnName("TABANCA_9");
                entity.Property(e => e.Tarih).HasMaxLength(10);
                entity.Property(e => e.Tork1)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_1");
                entity.Property(e => e.Tork10)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_10");
                entity.Property(e => e.Tork11)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_11");
                entity.Property(e => e.Tork12)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_12");
                entity.Property(e => e.Tork13)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_13");
                entity.Property(e => e.Tork14)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_14");
                entity.Property(e => e.Tork15)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_15");
                entity.Property(e => e.Tork16)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_16");
                entity.Property(e => e.Tork17)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_17");
                entity.Property(e => e.Tork18)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_18");
                entity.Property(e => e.Tork19)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_19");
                entity.Property(e => e.Tork2)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_2");
                entity.Property(e => e.Tork20)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_20");
                entity.Property(e => e.Tork21)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_21");
                entity.Property(e => e.Tork22)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_22");
                entity.Property(e => e.Tork23)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_23");
                entity.Property(e => e.Tork24)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_24");
                entity.Property(e => e.Tork25)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_25");
                entity.Property(e => e.Tork26)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_26");
                entity.Property(e => e.Tork27)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_27");
                entity.Property(e => e.Tork28)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_28");
                entity.Property(e => e.Tork29)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_29");
                entity.Property(e => e.Tork3)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_3");
                entity.Property(e => e.Tork30)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_30");
                entity.Property(e => e.Tork31)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_31");
                entity.Property(e => e.Tork32)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_32");
                entity.Property(e => e.Tork33)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_33");
                entity.Property(e => e.Tork34)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_34");
                entity.Property(e => e.Tork35)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_35");
                entity.Property(e => e.Tork36)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_36");
                entity.Property(e => e.Tork37)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_37");
                entity.Property(e => e.Tork38)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_38");
                entity.Property(e => e.Tork39)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_39");
                entity.Property(e => e.Tork4)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_4");
                entity.Property(e => e.Tork40)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_40");
                entity.Property(e => e.Tork41)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_41");
                entity.Property(e => e.Tork42)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_42");
                entity.Property(e => e.Tork43)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_43");
                entity.Property(e => e.Tork44)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_44");
                entity.Property(e => e.Tork45)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_45");
                entity.Property(e => e.Tork46)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_46");
                entity.Property(e => e.Tork47)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_47");
                entity.Property(e => e.Tork48)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_48");
                entity.Property(e => e.Tork49)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_49");
                entity.Property(e => e.Tork5)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_5");
                entity.Property(e => e.Tork50)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_50");
                entity.Property(e => e.Tork6)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_6");
                entity.Property(e => e.Tork7)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_7");
                entity.Property(e => e.Tork8)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_8");
                entity.Property(e => e.Tork9)
                    .HasMaxLength(10)
                    .HasColumnName("TORK_9");
                entity.Property(e => e.Vida1)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_1");
                entity.Property(e => e.Vida10)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_10");
                entity.Property(e => e.Vida11)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_11");
                entity.Property(e => e.Vida12)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_12");
                entity.Property(e => e.Vida13)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_13");
                entity.Property(e => e.Vida14)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_14");
                entity.Property(e => e.Vida15)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_15");
                entity.Property(e => e.Vida16)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_16");
                entity.Property(e => e.Vida17)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_17");
                entity.Property(e => e.Vida18)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_18");
                entity.Property(e => e.Vida19)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_19");
                entity.Property(e => e.Vida2)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_2");
                entity.Property(e => e.Vida20)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_20");
                entity.Property(e => e.Vida21)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_21");
                entity.Property(e => e.Vida22)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_22");
                entity.Property(e => e.Vida23)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_23");
                entity.Property(e => e.Vida24)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_24");
                entity.Property(e => e.Vida25)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_25");
                entity.Property(e => e.Vida26)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_26");
                entity.Property(e => e.Vida27)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_27");
                entity.Property(e => e.Vida28)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_28");
                entity.Property(e => e.Vida29)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_29");
                entity.Property(e => e.Vida3)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_3");
                entity.Property(e => e.Vida30)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_30");
                entity.Property(e => e.Vida31)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_31");
                entity.Property(e => e.Vida32)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_32");
                entity.Property(e => e.Vida33)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_33");
                entity.Property(e => e.Vida34)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_34");
                entity.Property(e => e.Vida35)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_35");
                entity.Property(e => e.Vida36)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_36");
                entity.Property(e => e.Vida37)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_37");
                entity.Property(e => e.Vida38)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_38");
                entity.Property(e => e.Vida39)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_39");
                entity.Property(e => e.Vida4)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_4");
                entity.Property(e => e.Vida40)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_40");
                entity.Property(e => e.Vida41)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_41");
                entity.Property(e => e.Vida42)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_42");
                entity.Property(e => e.Vida43)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_43");
                entity.Property(e => e.Vida44)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_44");
                entity.Property(e => e.Vida45)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_45");
                entity.Property(e => e.Vida46)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_46");
                entity.Property(e => e.Vida47)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_47");
                entity.Property(e => e.Vida48)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_48");
                entity.Property(e => e.Vida49)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_49");
                entity.Property(e => e.Vida5)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_5");
                entity.Property(e => e.Vida50)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_50");
                entity.Property(e => e.Vida6)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_6");
                entity.Property(e => e.Vida7)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_7");
                entity.Property(e => e.Vida8)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_8");
                entity.Property(e => e.Vida9)
                    .HasMaxLength(10)
                    .HasColumnName("VIDA_9");
            });

            modelBuilder.Entity<OpMashin>(entity =>
            {
                entity.ToTable("OpMashin");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IpAddress).HasMaxLength(50);
                entity.Property(e => e.MasineName).HasMaxLength(50);
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<OrAlert>(entity =>
            {
                entity.ToTable("OrAlert");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<OrAlertBaglanti>(entity =>
            {
                entity.ToTable("OrAlertBaglanti");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.UpdateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Alert).WithMany(p => p.OrAlertBaglantis)
                    .HasForeignKey(d => d.AlertId)
                    .HasConstraintName("FK_OrAlertBaglanti_OrAlert");

                entity.HasOne(d => d.Harness).WithMany(p => p.OrAlertBaglantis)
                    .HasForeignKey(d => d.HarnessId)
                    .HasConstraintName("FK_OrAlertBaglanti_OrHarnessModel");
            });

            modelBuilder.Entity<OrFamily>(entity =>
            {
                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<OrHarnessConfig>(entity =>
            {
                entity.ToTable("OrHarnessConfig");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.ConfigTork).HasMaxLength(5);
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.PFBSocket)
                    .HasMaxLength(50)
                    .HasColumnName("PFBSocket");
                entity.Property(e => e.UpdateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.OrHarnessModel).WithMany(p => p.OrHarnessConfigs)
                    .HasForeignKey(d => d.OrHarnessModelId)
                    .HasConstraintName("FK_OrHarnessConfig_OrHarnessModel");
            });

            modelBuilder.Entity<OrHarnessModel>(entity =>
            {
                entity.ToTable("OrHarnessModel");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.CustomerID)
                    .HasMaxLength(50)
                    .HasColumnName("CustomerID");
                entity.Property(e => e.Family).HasMaxLength(10);
                entity.Property(e => e.HarnessModelName).HasMaxLength(22);
                entity.Property(e => e.Prefix).HasMaxLength(6);
                entity.Property(e => e.Release).HasMaxLength(50);
                entity.Property(e => e.Suffix).HasMaxLength(6);
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.FamilyNavigation).WithMany(p => p.OrHarnessModels)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("FK_OrHarnessModel_OrFamilies");
            });

            modelBuilder.Entity<SyBarcodeInCrossIstasyon>(entity =>
            {
                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.SysBarcodeIn).WithMany(p => p.SyBarcodeInCrossIstasyons)
                    .HasForeignKey(d => d.SysBarcodeInId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SyBarcodeInCrossIstasyons_SyBarcodeINPUT");

                entity.HasOne(d => d.UrIstasyon).WithMany(p => p.SyBarcodeInCrossIstasyons)
                    .HasForeignKey(d => d.UrIstasyonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_SyBarcodeInCrossIstasyons_UrIstasyon");
            });

            modelBuilder.Entity<SyBarcodeInput>(entity =>
            {
                entity.ToTable("SyBarcodeINPUT");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.OzelChar).HasMaxLength(1);
                entity.Property(e => e.ParcalamaChar).HasMaxLength(1);
                entity.Property(e => e.RegexInt).HasMaxLength(50);
                entity.Property(e => e.RegexString).HasMaxLength(50);
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Printer).WithMany(p => p.SyBarcodeInputs)
                    .HasForeignKey(d => d.PrinterId)
                    .HasConstraintName("FK_BarcodeINPUT_Printer");
            });

            modelBuilder.Entity<SyBarcodeOut>(entity =>
            {
                entity.ToTable("SyBarcodeOUT");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.BarcodeIcerik).HasMaxLength(250);
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.eclntcode)
                    .HasMaxLength(10)
                    .IsFixedLength()
                    .HasColumnName("eclntcode");
                entity.Property(e => e.family)
                    .HasMaxLength(10)
                    .HasColumnName("family");
                entity.Property(e => e.IdDonanim).HasMaxLength(10);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.OzelChar).HasMaxLength(1);
                entity.Property(e => e.ParcalamaChar).HasMaxLength(1);
                entity.Property(e => e.prefix)
                    .HasMaxLength(10)
                    .HasColumnName("prefix");
                entity.Property(e => e.Promenliva).HasMaxLength(50);
                entity.Property(e => e.PsName).HasMaxLength(30);
                entity.Property(e => e.RegexInt).HasMaxLength(50);
                entity.Property(e => e.RegexString).HasMaxLength(50);
                entity.Property(e => e.releace)
                    .HasMaxLength(50)
                    .HasColumnName("releace");
                entity.Property(e => e.Sira1).HasMaxLength(50);
                entity.Property(e => e.Sira2).HasMaxLength(50);
                entity.Property(e => e.Sira3).HasMaxLength(50);
                entity.Property(e => e.suffix)
                    .HasMaxLength(10)
                    .HasColumnName("suffix");
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Prinetr).WithMany(p => p.SyBarcodeOuts)
                    .HasForeignKey(d => d.PrinetrId)
                    .HasConstraintName("FK_BarcodeOUT_Printer");
            });

            modelBuilder.Entity<SyPrinter>(entity =>
            {
                entity.ToTable("SyPrinter");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Interface).HasMaxLength(50);
                entity.Property(e => e.Ip).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.PrintngFile).HasMaxLength(250);
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<UrIstasyon>(entity =>
            {
                entity.ToTable("UrIstasyon");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.Calismasati).HasDefaultValueSql("(N'')");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Durus).HasDefaultValueSql("(N'')");
                entity.Property(e => e.FamilyId).HasDefaultValueSql("((0))");
                entity.Property(e => e.MashinId).HasDefaultValueSql("((0))");
                entity.Property(e => e.Orta).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.UnikId).HasDefaultValueSql("(N'')");
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.VardiyaId).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Family).WithMany(p => p.UrIstasyons)
                    .HasForeignKey(d => d.FamilyId)
                    .HasConstraintName("FK_UrIstasyon_OrFamilies");

                entity.HasOne(d => d.Mashin).WithMany(p => p.UrIstasyons)
                    .HasForeignKey(d => d.MashinId)
                    .HasConstraintName("FK_UrIstasyon_OpMashin");

                entity.HasOne(d => d.ModulerYapi).WithMany(p => p.UrIstasyons)
                    .HasForeignKey(d => d.ModulerYapiId)
                    .HasConstraintName("FK_UrIstasyon_UrModulerYapi");

                entity.HasOne(d => d.SyBarcodeOut).WithMany(p => p.UrIstasyons)
                    .HasForeignKey(d => d.SyBarcodeOutId)
                    .HasConstraintName("FK_UrIstasyon_SyBarcodeOUT");

                entity.HasOne(d => d.Vardiya).WithMany(p => p.UrIstasyons)
                    .HasForeignKey(d => d.VardiyaId)
                    .HasConstraintName("FK_UrIstasyon_UrVardiya");
            });

            modelBuilder.Entity<UrFabrika>(entity =>
            {
                entity.ToTable("UrFabrika");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.Fabrika).HasMaxLength(50);
                entity.Property(e => e.Locasion).HasMaxLength(50);
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UrKonveyorNumara>(entity =>
            {
                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<UrModulerYapi>(entity =>
            {
                entity.ToTable("UrModulerYapi");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Etap).HasMaxLength(50);
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
            });
            modelBuilder.Entity<UrPersonalTakib>(entity =>
            {
                entity.ToTable("UrPersonalTakib");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.FullName).HasMaxLength(50);
                entity.Property(e => e.DayOfYear).HasMaxLength(50);
                entity.Property(e => e.Sicil).HasMaxLength(11);
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UrVardiya>(entity =>
            {
                entity.ToTable("UrVardiya");

                entity.Property(e => e.Activ).HasDefaultValueSql("((1))");
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.UpdateDate).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}