using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity;



[Nursan.Domain.Table.Table(TableName = "ErRework")]
public partial class ErRework : BaseEntity
{

    public int? IdDonanim { get; set; }

    public string? Istasyon { get; set; }

    public DateTime? IstasyonTarihi { get; set; }

    public string? Referans { get; set; }

    public string? ReworkInOperator { get; set; }

    public DateTime? ReworkInDate { get; set; }

    public string? ReworkFixOperator { get; set; }

    public DateTime? ReworkFixDate { get; set; }

    public string? ReworkOutOperator { get; set; }

    public DateTime? ReworkOutDate { get; set; }

    public string? Comment { get; set; }

    public string? NextIstasyon { get; set; }

    public int? ErrorCode { get; set; }

    public string? ErrorCodeDefination { get; set; }

    public string? FaultRegion { get; set; }

    public string? FaultGoz { get; set; }

    public string? FaultInIstasyn { get; set; }

    public string? CausOfFault { get; set; }

    public string? FaultInIstayon { get; set; }

    public bool? ReworkOut { get; set; }

    public int? IdDonanimChanges { get; set; }
}
