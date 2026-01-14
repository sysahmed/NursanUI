using AutoMapper;
using Nursan.API.DTOs;
using Nursan.Domain.Entity;

namespace Nursan.API.Mapping
{
    /// <summary>
    /// AutoMapper профил за мапинг на обекти
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Мапинг от OrHarnessModel към HarnessModelDto
            CreateMap<OrHarnessModel, HarnessModelDto>()
                .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.FamilyNavigation != null ? src.FamilyNavigation.FamilyName : null));

            // Мапинг от HarnessModelDto към OrHarnessModel (за update операции)
            CreateMap<HarnessModelDto, OrHarnessModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FamilyNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IzCoaxCableCounts, opt => opt.Ignore())
                .ForMember(dest => dest.IzCoaxCableCrosses, opt => opt.Ignore())
                .ForMember(dest => dest.IzDonanimHedefs, opt => opt.Ignore())
                .ForMember(dest => dest.IzGenerateIds, opt => opt.Ignore())
                .ForMember(dest => dest.OrAlertBaglantis, opt => opt.Ignore())
                .ForMember(dest => dest.OrHarnessConfigs, opt => opt.Ignore());

            // Мапинг за BarcodeInputDto към SyBarcodeInput
            CreateMap<BarcodeInputDto, SyBarcodeInput>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.Activ, opt => opt.Ignore())
                .ForMember(dest => dest.Printer, opt => opt.Ignore())
                .ForMember(dest => dest.SyBarcodeInCrossIstasyons, opt => opt.Ignore())
                .ForMember(dest => dest.Leght, opt => opt.Ignore())
                .ForMember(dest => dest.SyBarcodeInCrossIstasyonId, opt => opt.Ignore())
                .ForMember(dest => dest.StartingSubstring, opt => opt.Ignore())
                .ForMember(dest => dest.StopingSubstring, opt => opt.Ignore());

            // Мапинг за DonanimCountDto към IzDonanimCount
            CreateMap<DonanimCountDto, IzDonanimCount>()
                .ForMember(dest => dest.IdDonanimNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Masa, opt => opt.Ignore())
                .ForMember(dest => dest.UrIstasyon, opt => opt.Ignore())
                .ForMember(dest => dest.Mashin, opt => opt.Ignore())
                .ForMember(dest => dest.Vardiya, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.Activ, opt => opt.Ignore());

            // Мапинг за GenerateIdDto към IzGenerateId
            CreateMap<GenerateIdDto, IzGenerateId>()
                .ForMember(dest => dest.HarnesModel, opt => opt.Ignore())
                .ForMember(dest => dest.UrIstasyon, opt => opt.Ignore())
                .ForMember(dest => dest.IzCoaxCableCounts, opt => opt.Ignore())
                .ForMember(dest => dest.IzDonanimCounts, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore())
                .ForMember(dest => dest.Activ, opt => opt.Ignore());
        }
    }
}
