using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursan.Domain.Entity;
[Nursan.Domain.Table.Table(TableName = "IzToplamV769", IdentityColumn = "ID", PrimaryColumn = "id_donanim")]
public partial class IzToplamV769
{
    public decimal Id { get; set; }

    [Column("id_donanim")]
    public string IdDonanim { get; set; } = null!;

    public string Referans { get; set; } = null!;

    public bool? Sigortagec { get; set; }

    public string? Sigorta { get; set; }

    public string? Sigortavar { get; set; }

    public DateTime? Sigortadate { get; set; }

    public bool? Antengec { get; set; }

    public bool? Antenb { get; set; }

    public string? Anten { get; set; }

    public string? Antenvar { get; set; }

    public DateTime? Antendate { get; set; }

    /// <summary>
    /// false
    /// </summary>
    public bool? Konveyorgec { get; set; }

    public bool? Konveyorb { get; set; }

    public string? Konveyor { get; set; }

    public string? Konvar { get; set; }

    /// <summary>
    /// getdate()
    /// </summary>
    public DateTime? Kondata { get; set; }

    public bool? Grometgec { get; set; }

    public bool? Grometb { get; set; }

    public string? Gromet { get; set; }

    public string? Grometvar { get; set; }

    public DateTime? Grometdata { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Kliptestb { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Klipgec { get; set; }

    public string? Kliptest { get; set; }

    public string? Klipvar { get; set; }

    public DateTime? Kliptestdata { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Eltestb { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Eltestgec { get; set; }

    public string? Eltest { get; set; }

    public string? Elvar { get; set; }

    public DateTime? Eltestdata { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Gozb { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Gozgec { get; set; }

    public string? Goz { get; set; }

    public string? Gozvar { get; set; }

    public DateTime? Gozdata { get; set; }

    public bool? Torkb { get; set; }

    public bool? Torkgec { get; set; }

    public string? Tork { get; set; }

    public string? Torkvar { get; set; }

    public DateTime? Torkdate { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Paketlemeb { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Paketgec { get; set; }

    public string? Paketleme { get; set; }

    public string? Paketvar { get; set; }

    public DateTime? Paketdata { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Kolib { get; set; }

    public string? Koli { get; set; }

    public DateTime? Kolidata { get; set; }

    public string? Kasa { get; set; }

    public DateTime? Kasatarih { get; set; }

    public string? Revork { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Revorkta { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Revorkgec { get; set; }

    public DateTime? Revorkdata { get; set; }

    public string? Revorkvar { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public bool? Revorkb { get; set; }

    public int Alert { get; set; }

    public string SideCode { get; set; } = null!;

    public string CustomId { get; set; } = null!;

    public short Fabrika { get; set; }

    public string? Qc { get; set; }
}