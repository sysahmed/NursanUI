using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "IzCoaxCableCross")]
    public partial class IzCoaxCableCross : BaseEntity
    {
        public IzCoaxCableCross()
        {
            IzCoaxCableCounts = new HashSet<IzCoaxCableCount>();
        }


        public int? CoaxCableBarcodeId { get; set; }
        public int? HarnessModelId { get; set; }

        public virtual IzCoaxCableConfig? CoaxCableBarcode { get; set; }
        public virtual OrHarnessModel? HarnessModel { get; set; }
        public virtual ICollection<IzCoaxCableCount> IzCoaxCableCounts { get; set; }
    }
}
