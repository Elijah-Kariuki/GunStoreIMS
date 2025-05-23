// Application/Mapping/FirearmProfile.cs
using AutoMapper;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Shared.Dto;

namespace GunStoreIMS.Application.Mapping
{
    public class FirearmProfile : Profile
    {
        public FirearmProfile()
        {
            // 1) DOMAIN → LIST
            CreateMap<Firearm, FirearmLineDto>()
                .ForMember(d => d.ManufacturerImporter, o => o.MapFrom(s => s.Manufacturer))
                .ForMember(d => d.Model, o => o.MapFrom(s => s.Model))
                .ForMember(d => d.SerialNumber, o => o.MapFrom(s => s.SerialNumber))
                .ForMember(d => d.FirearmType, o => o.MapFrom(s => s.FirearmType))
                .ForMember(d => d.CaliberGauge, o => o.MapFrom(s => s.Caliber.Name))
                .ForMember(d => d.IsNFAItem, o => o.MapFrom(s => s.NfaClass != null))
                .ForMember(d => d.NfaClass, o => o.MapFrom(s => s.NfaClass))
                .ForMember(d => d.FFLId, o => o.MapFrom(s => s.FFLId))
                .ForMember(d => d.InitialAcquisitionDate, o => o.MapFrom(s => s.InitialAcquisitionDate))
                .ForMember(d => d.CurrentStatus, o => o.MapFrom(s => s.CurrentStatus));

            // 2) DOMAIN → DETAIL
            CreateMap<Firearm, FirearmDetailDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.SerialNumber, o => o.MapFrom(s => s.SerialNumber))
                .ForMember(d => d.Manufacturer, o => o.MapFrom(s => s.Manufacturer))
                .ForMember(d => d.Model, o => o.MapFrom(s => s.Model))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.FirearmType))
                .ForMember(d => d.CaliberOrGauge, o => o.MapFrom(s => s.Caliber.Name))
                .ForMember(d => d.ImporterName, o => o.MapFrom(s => s.ImporterName))
                .ForMember(d => d.ImporterAddress, o => o.MapFrom(s => $"{s.ImporterCity}, {s.ImporterState}"))
                .ForMember(d => d.CountryOfManufacture, o => o.MapFrom(s => s.CountryOfOrigin))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.AdditionalMarkings))
                .ForMember(d => d.IsNew, o => o.MapFrom(s => !s.IsAntique))
                .ForMember(d => d.Notes, o => o.MapFrom(s => s.InternalNotes))
                .ForMember(d => d.BarrelLength, o => o.MapFrom(s => s.BarrelLength))
                // note: no OverallLength on the DTO, so don’t map it
                .ForMember(d => d.Finish, o => o.Ignore())          // domain has no Finish
                .ForMember(d => d.DateOfManufacture, o => o.MapFrom(s => s.ManufactureDate));

            // 3) SPECIALIZED
            CreateMap<Firearm, HandgunDto>().IncludeBase<Firearm, FirearmDetailDto>();
            CreateMap<Firearm, PistolDto>().IncludeBase<HandgunDto, PistolDto>();
            CreateMap<Firearm, RevolverDto>().IncludeBase<HandgunDto, RevolverDto>();
            CreateMap<Firearm, LongGunDto>().IncludeBase<Firearm, FirearmDetailDto>();
            CreateMap<Firearm, RifleDto>().IncludeBase<LongGunDto, RifleDto>();
            CreateMap<Firearm, ShotgunDto>().IncludeBase<LongGunDto, ShotgunDto>();
            CreateMap<Firearm, SilencerDto>().IncludeBase<Firearm, FirearmDetailDto>();
        }
    }
}
