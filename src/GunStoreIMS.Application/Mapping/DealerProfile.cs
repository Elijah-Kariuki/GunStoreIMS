using AutoMapper;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Shared.Enums;
using System;

namespace GunStoreIMS.Application.Mapping
{
    public class DealerProfile : Profile
    {
        public DealerProfile()
        {
            // Domain → DTO
            CreateMap<DealerRecord, DealerRecordDto>()
                .ForMember(dest => dest.LicenseExpirationDate, opt => opt.MapFrom(src => src.ExpirationDateUtc))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()))
                .ForMember(dest => dest.LicenseType, opt => opt.MapFrom(src => src.LicenseType.ToString()));

            // DTO → Domain
            CreateMap<DealerRecordDto, DealerRecord>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForMember(dest => dest.RecordDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedUtc, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.AcquisitionsFrom, opt => opt.Ignore())
                .ForMember(dest => dest.DispositionsTo, opt => opt.Ignore())
                .ForMember(dest => dest.Form4473s, opt => opt.Ignore())
                .ForMember(dest => dest.ExpirationDateUtc, opt => opt.MapFrom(src => src.LicenseExpirationDate))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => Enum.Parse<USState>(src.State, true)))
                .ForMember(dest => dest.LicenseType, opt => opt.MapFrom(src => Enum.Parse<FflLicenseType>(src.LicenseType, true)));
        }
    }
}
