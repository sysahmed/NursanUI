using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "OrBoltProcessRunLog", IdentityColumn = "Id")]
    public partial class OrBoltProcessRunLog : BaseEntity
    {
        public string? HarnessModelName { get; set; }
        public DateTime? RunStartedAt { get; set; }
        public DateTime? RunFinishedAt { get; set; }
        public string? Result { get; set; }
        public string? VideoPath { get; set; }
        public string? OperatorName { get; set; }
        public string? Notes { get; set; }
    }
}


