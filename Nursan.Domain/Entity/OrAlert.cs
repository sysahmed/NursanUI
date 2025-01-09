using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "OrAlert")]
    public partial class OrAlert : BaseEntity
    {
        public OrAlert()
        {
            OrAlertBaglantis = new HashSet<OrAlertBaglanti>();
        }


        public string? Name { get; set; }
        public bool? AlertAccess { get; set; }
        public int? AlertNumber { get; set; }


        public virtual ICollection<OrAlertBaglanti> OrAlertBaglantis { get; set; }
    }
}
