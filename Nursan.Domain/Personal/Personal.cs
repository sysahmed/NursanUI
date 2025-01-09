using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nursan.Domain.SystemClass;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursan.Domain.Personal;
[Nursan.Domain.Table.Table(TableName = "Personal", PrimaryColumn = "UserId", IdentityColumn = "Id")]
public partial class Personal : BaseEntityPersonal
{

    [Column("USER_ID")]
    public long? USER_ID { get; set; }
    [Column("USER_CODE")]
    public string? USER_CODE { get; set; }
    [Column("FIRST_NAME")]
    public string? FIRST_NAME { get; set; }
    [Column("SUR_NAME")]
    public string? SUR_NAME { get; set; }
    [Column("LAST_NAME")]
    public string? LAST_NAME { get; set; }
    [Column("CARD_ID")]
    public long? CARD_ID { get; set; }
    [Column("DEPARTMENT")]
    public long DEPARTMENT { get; set; }
    [Column("JOB_POSITION")]
    public long JOB_POSITION { get; set; }
    [Column("LAST_DIR")]
    public bool? LAST_DIR { get; set; }
    [Column("DIR_CHANGE")]
    public DateTime? DIR_CHANGE { get; set; }

    public virtual Department DepartmentNavigation { get; set; } = null!;
}
