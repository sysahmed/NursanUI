using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "IzCoaxCableConfig")]
    public class IzCoaxCableConfig : BaseEntity
    {
        public IzCoaxCableConfig()
        {
            IzCoaxCableCrosses = new HashSet<IzCoaxCableCross>();
        }

        public string? CoaxCabloReferans { get; set; }
        public string? Supplier { get; set; }
        public virtual ICollection<IzCoaxCableCross> IzCoaxCableCrosses { get; set; }

    }
}
