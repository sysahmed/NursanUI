using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "SyStationCamera", IdentityColumn = "Id")]
    public partial class SyStationCamera : BaseEntity
    {
        public int? UrIstasyonId { get; set; }
        public byte? CameraSlot { get; set; }
        public string? RtspUrl { get; set; }
        public bool? IsActive { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}


