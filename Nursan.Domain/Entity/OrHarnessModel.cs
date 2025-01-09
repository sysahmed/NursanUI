using Nursan.Domain.SystemClass;

namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "OrHarnessModel", IdentityColumn = "Id")]
    public partial class OrHarnessModel : BaseEntity
    {
        public OrHarnessModel(string? prefix, string? family, string? suffix, string? release, bool? access, bool? active, string? sideCode, int? alertnumber)
        {
            Prefix = prefix;
            Family = family;
            Suffix = suffix;
            AlertNumber = alertnumber;
            Release = release;
            Access = access;
            Activ = active;
            SideCode = sideCode;
            HarnessModelName = $"{Prefix}-{Family}-{Suffix}";
        }


        public OrHarnessModel()
        {
            IzCoaxCableCounts = new HashSet<IzCoaxCableCount>();
            IzCoaxCableCrosses = new HashSet<IzCoaxCableCross>();
            IzDonanimHedefs = new HashSet<IzDonanimHedef>();
            IzGenerateIds = new HashSet<IzGenerateId>();
            OrAlertBaglantis = new HashSet<OrAlertBaglanti>();
            OrHarnessConfigs = new HashSet<OrHarnessConfig>();
        }



        public string? Prefix { get; set; }
        public string? Family { get; set; }
        public string? Suffix { get; set; }
        public string? HarnessModelName { get; set; }
        public string? Release { get; set; }
        public bool? Access { get; set; }
        public string? SideCode { get; set; }
        public int? FamilyId { get; set; }
        public int? AlertNumber { get; set; }
        public int? OrHarnessConfigId { get; set; }
        public string? CustomerID { get; set; }

        public virtual OrFamily? FamilyNavigation { get; set; }
        public virtual ICollection<IzCoaxCableCount> IzCoaxCableCounts { get; set; }
        public virtual ICollection<IzCoaxCableCross> IzCoaxCableCrosses { get; set; }
        public virtual ICollection<IzDonanimHedef> IzDonanimHedefs { get; set; }
        public virtual ICollection<IzGenerateId> IzGenerateIds { get; set; }
        public virtual ICollection<OrAlertBaglanti> OrAlertBaglantis { get; set; }
        public virtual ICollection<OrHarnessConfig> OrHarnessConfigs { get; set; }
    }
}
