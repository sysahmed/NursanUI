using Nursan.Domain.SystemClass;

namespace Nursan.Domain.ModelsDisposible
{
    public class HarnesDonanimHedef : BaseEntity
    {
        public string harnessModel { get; set; }
        public int? Adet { get; set; }
        public int? Hedef { get; set; }
        public int? IstasyonId { get; set; }
        public int? HarnesId { get; set; }

    }
}
