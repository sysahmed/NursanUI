using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "ErErrorCode")]
    public class ErErrorCode : BaseEntity
    {
        public string? ErrorCode { get; set; }
        public string? ErrorName { get; set; }
        public string? ErrorLocation { get; set; }
    }
}
