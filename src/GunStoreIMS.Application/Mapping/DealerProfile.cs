using AutoMapper;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Shared.Dto;

namespace GunStoreIMS.Application.Mapping
{
    public class DealerProfile : Profile
    {
        public DealerProfile()
        {
            // Domain → DTO
            CreateMap<DealerRecord, DealerRecordDto>()
                .ForMember(d => d.DealerName, o => o.MapFrom(s => s.TradeName))
                .ForMember(d => d.StreetAddress, o => o.MapFrom(s => s.StreetAddress))
                .ForMember(d => d.City, o => o.MapFrom(s => s.City))
                .ForMember(d => d.State, o => o.MapFrom(s => s.State))
                .ForMember(d => d.Zip, o => o.MapFrom(s => s.Zip))
                .ForMember(d => d.FFLNumber, o => o.MapFrom(s => s.FFLNumber))
                .ForMember(d => d.LicenseExpirationDate, o => o.MapFrom(s => s.LicenseExpirationDate));

            // DTO → Domain
            CreateMap<DealerRecordDto, DealerRecord>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.TradeName, o => o.MapFrom(s => s.DealerName))
                .ForMember(d => d.StreetAddress, o => o.MapFrom(s => s.StreetAddress))
                .ForMember(d => d.City, o => o.MapFrom(s => s.City))
                .ForMember(d => d.State, o => o.MapFrom(s => s.State))
                .ForMember(d => d.Zip, o => o.MapFrom(s => s.Zip))
                .ForMember(d => d.FFLNumber, o => o.MapFrom(s => s.FFLNumber))
                .ForMember(d => d.LicenseExpirationDate, o => o.MapFrom(s => s.LicenseExpirationDate));
        }
    }
}
