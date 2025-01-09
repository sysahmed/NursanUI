namespace Nursan.Domain.Entity
{
    [Nursan.Domain.Table.Table(TableName = "UrKonveyorNumara")]
    public partial class UrKonveyorNumara
    {
        public UrKonveyorNumara()
        {
            IzDonanimCounts = new HashSet<IzDonanimCount>();
        }

        public int Id { get; set; }
        public int? Konveyor { get; set; }
        public string? MasaNo { get; set; }
        public int? Numara { get; set; }
        public bool? Activ { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<IzDonanimCount>? IzDonanimCounts { get; set; }
    }
}
