using Microsoft.EntityFrameworkCore;

namespace Nursan.Domain.TORKS;

public partial class NursandatabaseContext : DbContext
{
    private readonly string sqlConnectionString = SystemClass.XMLSeverIp.XmllServerIP();
    public NursandatabaseContext()
    {
    }

    public NursandatabaseContext(DbContextOptions<NursandatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Nrsclsdeg> Nrsclsdegs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer($"Server={sqlConnectionString}\\NRS_SCADA;Database=NURSANDATABASE;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");
        modelBuilder.Entity<Nrsclsdeg>(entity =>
        {
            entity
                .ToTable("NRSCLSDEG");
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
            entity.Property(e => e.Nr).HasMaxLength(10);
            entity.Property(e => e.ReceteName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RECETE_NAME");
            entity.Property(e => e.ReceteNo).HasColumnName("RECETE_NO");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
