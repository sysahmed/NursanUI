using System;
using System.Collections.Generic;

namespace Nursan.Domain.NursanBarcode;

public partial class UrunMaster
{
    public int Id { get; set; }

    public string Alckodu { get; set; } = null!;

    public string MalzemeKodu { get; set; } = null!;

    public string PartName { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string SupplyArea { get; set; } = null!;

    public string BinNo { get; set; } = null!;

    public string SigortaNo { get; set; } = null!;

    public string Abagkodu { get; set; } = null!;

    public short Miktar { get; set; }
}
