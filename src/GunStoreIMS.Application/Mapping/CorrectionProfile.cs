using AutoMapper;
using GunStoreIMS.Shared.Dto;
using GunStoreIMS.Domain.Models;

namespace GunStoreIMS.Application.Mapping
{
    // --- Correction Profile ---
    public class CorrectionProfile : Profile
    {
        public CorrectionProfile()
        {
            CreateMap<FirearmCorrectionDto, Firearm>()
                .ForMember(dest => dest.Id, o => o.Ignore())
                .ForMember(dest => dest.SerialNumber, o => o.Ignore())
                .ForMember(dest => dest.CurrentStatus, o => o.Ignore())
                .ForMember(dest => dest.InitialAcquisitionDate, o => o.Ignore());
        }
    }
}
