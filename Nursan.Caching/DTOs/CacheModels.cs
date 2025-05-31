using System;
using System.Collections.Generic;

namespace Nursan.Caching.DTOs
{
    /// <summary>
    /// DTO за OpMashin, без циклични референции
    /// </summary>
    public class OpMashinDto
    {
        public int Id { get; set; }
        public string MasineName { get; set; }
        public string IpAddress { get; set; }
        public string OperationSystems { get; set; }
        public bool? Activ { get; set; }
        
        // Няма колекция UrIstasyons тук, за да избегнем цикличност
    }

    /// <summary>
    /// DTO за UrIstasyon, без циклични референции
    /// </summary>
    public class UrIstasyonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ModulerYapiId { get; set; }
        public short? FabrikaId { get; set; }
        public int? MashinId { get; set; }
        public int? VardiyaId { get; set; }
        public int? Toplam { get; set; }
        public string Calismasati { get; set; }
        public string Durus { get; set; }
        public int? FamilyId { get; set; }
        public int? Hedef { get; set; }
        public decimal? Orta { get; set; }
        public int? Realadet { get; set; }
        public int? Sayi { get; set; }
        public int? Sayicarp { get; set; }
        public bool? Sifirla { get; set; }
        public DateTime? Sonokuma { get; set; }
        public int? SyBarcodeOutId { get; set; }
        public string UnikId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? Activ { get; set; }
        
        // Референция към OpMashinDto вместо OpMashin, но без референция към колекцията
        public OpMashinDto Mashin { get; set; }
    }

    /// <summary>
    /// DTO за UrVardiya, без циклични референции
    /// </summary>
    public class UrVardiyaDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Activ { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    /// <summary>
    /// DTO за OrFamily, без циклични референции
    /// </summary>
    public class OrFamilyDto
    {
        public int Id { get; set; }
        public string FamilyName { get; set; }
        public bool? Activ { get; set; }
    }

    /// <summary>
    /// DTO за UrModulerYapi, без циклични референции
    /// </summary>
    public class UrModulerYapiDto
    {
        public int Id { get; set; }
        public string Etap { get; set; }
        public bool? Activ { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    /// <summary>
    /// DTO за IzGenerateId, без циклични референции
    /// </summary>
    public class IzGenerateIdDto
    {
        public int Id { get; set; }
        public int? UrIstasyonId { get; set; }
        public int? HarnesModelId { get; set; }
        public string Barcode { get; set; }
        public string PFBSocket { get; set; }
        public int? ReferasnLeght { get; set; }
        public int? DonanimIdLeght { get; set; }
        public int? DonanimTorkReferansId { get; set; }
        public int? AlertNumber { get; set; }
        public bool? Revork { get; set; }
        public bool? Activ { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    /// <summary>
    /// DTO за IzDonanimCount, без циклични референции
    /// </summary>
    public class IzDonanimCountDto
    {
        public int Id { get; set; }
        public int IdDonanim { get; set; }
        public string DonanimReferans { get; set; }
        public string OrHarnessModel { get; set; }
        public int? AlertNumber { get; set; }
        public int UrIstasyonId { get; set; }
        public int? MasaId { get; set; }
        public int? MashinId { get; set; }
        public int VardiyaId { get; set; }
        public bool? Revork { get; set; }
        public bool? Activ { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
} 