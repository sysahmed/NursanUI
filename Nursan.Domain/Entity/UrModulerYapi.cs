using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "UrModulerYapi")]
    public partial class UrModulerYapi : BaseEntity
    {
        public UrModulerYapi()
        {
            UrIstasyons = new HashSet<UrIstasyon>();
        }


        public string Etap { get; set; } = null!;


        public virtual ICollection<UrIstasyon> UrIstasyons { get; set; }
    }
}
