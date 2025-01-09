using System;
using System.Collections.Generic;

namespace Nursan.Domain.NursanBarcode;

public partial class Aradosya
{
    public int Id { get; set; }

    public string MalzemeKodu { get; set; } = null!;

    public string SeriNo { get; set; } = null!;

    public DateTime An { get; set; }

    public string Alckodu { get; set; } = null!;

    public bool? ArsiveAktarildi { get; set; }

    public string? UserId { get; set; }

    public int? FkUrunMasterid { get; set; }

    public string? K1 { get; set; }

    public string? DeviceIp { get; set; }

    public string? DeviceId { get; set; }
}
