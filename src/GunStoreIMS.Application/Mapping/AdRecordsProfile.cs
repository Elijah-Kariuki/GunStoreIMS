using AutoMapper;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Domain.Models;

namespace GunStoreIMS.Application.Mapping
{
    // --- A&D Records Profile ---
    public class AdRecordsProfile : Profile
    {
        public AdRecordsProfile()
        {
            // Domain -> DTO
            CreateMap<AcquisitionRecord, AcquisitionRecordDto>()
                .ForMember(dest => dest.SourceAddress, o => o.MapFrom(src => src.SourceFullAddress))
                .ForMember(dest => dest.SourceLicenseNumber, o => o.MapFrom(src => src.SourceFFLNumber));

            // DTO -> Domain
            CreateMap<AcquisitionRecordDto, AcquisitionRecord>()
                .ForMember(dest => dest.Id, o => o.Ignore())
                .ForMember(dest => dest.Firearm, o => o.Ignore())
                .ForMember(dest => dest.RowVersion, o => o.Ignore())
                .ForMember(dest => dest.SourceFullAddress, o => o.MapFrom(src => src.SourceAddress))
                .ForMember(dest => dest.SourceFFLNumber, o => o.MapFrom(src => src.SourceLicenseNumber));

            // Disposition mappings
            CreateMap<DispositionRecord, DispositionRecordDto>();
            CreateMap<DispositionRecordDto, DispositionRecord>()
                .ForMember(dest => dest.Id, o => o.Ignore())
                .ForMember(dest => dest.Firearm, o => o.Ignore());

            // Dealer mappings
            CreateMap<DealerRecord, DealerRecordDto>();
            CreateMap<DealerRecordDto, DealerRecord>()
                .ForMember(dest => dest.Id, o => o.Ignore());
        }
    }
}
