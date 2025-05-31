using Nursan.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nursan.Caching.DTOs
{
    /// <summary>
    /// Mapper за конвертиране на Entity класовете към DTO класове без циклични референции
    /// </summary>
    public static class EntityToDtoMapper
    {
        /// <summary>
        /// Конвертира OpMashin към OpMashinDto
        /// </summary>
        public static OpMashinDto ToDto(this OpMashin entity)
        {
            if (entity == null) return null;
            
            return new OpMashinDto
            {
                Id = entity.Id,
                MasineName = entity.MasineName,
                IpAddress = entity.IpAddress,
                OperationSystems = entity.OperationSystems,
                Activ = entity.Activ
            };
        }

        /// <summary>
        /// Конвертира UrIstasyon към UrIstasyonDto
        /// </summary>
        public static UrIstasyonDto ToDto(this UrIstasyon entity)
        {
            if (entity == null) return null;
            
            var dto = new UrIstasyonDto
            {
                Id = entity.Id,
                Name = entity.Name,
                ModulerYapiId = entity.ModulerYapiId,
                FabrikaId = entity.FabrikaId,
                MashinId = entity.MashinId,
                VardiyaId = entity.VardiyaId,
                Toplam = entity.Toplam,
                Calismasati = entity.Calismasati,
                Durus = entity.Durus,
                FamilyId = entity.FamilyId,
                Hedef = entity.Hedef,
                Orta = entity.Orta,
                Realadet = entity.Realadet,
                Sayi = entity.Sayi,
                Sayicarp = entity.Sayicarp,
                Sifirla = entity.Sifirla,
                Sonokuma = entity.Sonokuma,
                SyBarcodeOutId = entity.SyBarcodeOutId,
                UnikId = entity.UnikId,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate,
                Activ = entity.Activ
            };
            
            // Внимателно задаваме машината, за да избегнем цикличност
            if (entity.Mashin != null)
            {
                dto.Mashin = entity.Mashin.ToDto();
            }
            
            return dto;
        }

        /// <summary>
        /// Конвертира UrVardiya към UrVardiyaDto
        /// </summary>
        public static UrVardiyaDto ToDto(this UrVardiya entity)
        {
            if (entity == null) return null;
            
            return new UrVardiyaDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Activ = entity.Activ,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate
            };
        }

        /// <summary>
        /// Конвертира OrFamily към OrFamilyDto
        /// </summary>
        public static OrFamilyDto ToDto(this OrFamily entity)
        {
            if (entity == null) return null;
            
            return new OrFamilyDto
            {
                Id = entity.Id,
                FamilyName = entity.FamilyName,
                Activ = entity.Activ
            };
        }

        /// <summary>
        /// Конвертира UrModulerYapi към UrModulerYapiDto
        /// </summary>
        public static UrModulerYapiDto ToDto(this UrModulerYapi entity)
        {
            if (entity == null) return null;
            
            return new UrModulerYapiDto
            {
                Id = entity.Id,
                Etap = entity.Etap,
                Activ = entity.Activ,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate
            };
        }

        /// <summary>
        /// Конвертира колекция от UrModulerYapi към колекция от UrModulerYapiDto
        /// </summary>
        public static IEnumerable<UrModulerYapiDto> ToDto(this IEnumerable<UrModulerYapi> entities)
        {
            if (entities == null) return null;
            
            return entities.Select(e => e.ToDto()).ToList();
        }

        /// <summary>
        /// Конвертира IzGenerateId към IzGenerateIdDto
        /// </summary>
        public static IzGenerateIdDto ToDto(this IzGenerateId entity)
        {
            if (entity == null) return null;
            
            return new IzGenerateIdDto
            {
                Id = entity.Id,
                UrIstasyonId = entity.UrIstasyonId,
                HarnesModelId = entity.HarnesModelId,
                Barcode = entity.Barcode,
                PFBSocket = entity.PFBSocket,
                ReferasnLeght = entity.ReferasnLeght,
                DonanimIdLeght = entity.DonanimIdLeght,
                DonanimTorkReferansId = entity.DonanimTorkReferansId,
                AlertNumber = entity.AlertNumber,
                Revork = entity.Revork,
                Activ = entity.Activ,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate
            };
        }

        /// <summary>
        /// Конвертира колекция от IzGenerateId към колекция от IzGenerateIdDto
        /// </summary>
        public static IEnumerable<IzGenerateIdDto> ToDto(this IEnumerable<IzGenerateId> entities)
        {
            if (entities == null) return null;
            
            return entities.Select(e => e.ToDto()).ToList();
        }

        /// <summary>
        /// Конвертира IzDonanimCount към IzDonanimCountDto
        /// </summary>
        public static IzDonanimCountDto ToDto(this IzDonanimCount entity)
        {
            if (entity == null) return null;
            
            return new IzDonanimCountDto
            {
                Id = entity.Id,
                IdDonanim = entity.IdDonanim,
                DonanimReferans = entity.DonanimReferans,
                OrHarnessModel = entity.OrHarnessModel,
                AlertNumber = entity.AlertNumber,
                UrIstasyonId = entity.UrIstasyonId,
                MasaId = entity.MasaId,
                MashinId = entity.MashinId,
                VardiyaId = entity.VardiyaId,
                Revork = entity.Revork,
                Activ = entity.Activ,
                CreateDate = entity.CreateDate,
                UpdateDate = entity.UpdateDate
            };
        }

        /// <summary>
        /// Конвертира колекция от IzDonanimCount към колекция от IzDonanimCountDto
        /// </summary>
        public static IEnumerable<IzDonanimCountDto> ToDto(this IEnumerable<IzDonanimCount> entities)
        {
            if (entities == null) return null;
            
            return entities.Select(e => e.ToDto()).ToList();
        }
    }
} 