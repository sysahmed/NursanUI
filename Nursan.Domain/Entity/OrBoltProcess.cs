using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "OrBoltProcess", IdentityColumn = "Id")]
    public partial class OrBoltProcess : BaseEntity
    {
        public string? HarnessModelName { get; set; }
        public byte? StepIndex { get; set; }

        public int? RoiX { get; set; }
        public int? RoiY { get; set; }
        public int? RoiWidth { get; set; }
        public int? RoiHeight { get; set; }

        public decimal? TorqueTarget { get; set; }
        public decimal? TorqueMin { get; set; }
        public decimal? TorqueMax { get; set; }
        public decimal? AngleTarget { get; set; }
        public decimal? AngleMin { get; set; }
        public decimal? AngleMax { get; set; }

        public bool? IsEnabled { get; set; }
        public string? Notes { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}


