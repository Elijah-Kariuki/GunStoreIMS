// File: Application/Mapping/Form4473Profile.cs
using System;
using AutoMapper;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Shared.Dto;      // Make sure this references the project where Form4473RecordDto lives

namespace GunStoreIMS.Application.Mapping
{
    public class Form4473Profile : Profile
    {
        public Form4473Profile()
        {
            // ────────────────────────────────────
            // Domain → DTO
            // ────────────────────────────────────

            CreateMap<Form4473Record, Form4473RecordDto>()
                // Hydrate SectionA.Firearms from EF before mapping
                .BeforeMap((src, _) => src.MapFirearmLinesToSectionAForSerialization())
                .ForMember(d => d.Id,
                           o => o.MapFrom(s => s.Id))
                .ForMember(d => d.TransferorCertification,
                           o => o.MapFrom(s => s.TransferorCertification))
                .ForMember(d => d.SectionA,
                           o => o.MapFrom(s => s.SectionA))
                .ForMember(d => d.SectionB,
                           o => o.MapFrom(s => s.SectionB))
                .ForMember(d => d.SectionC,
                           o => o.MapFrom(s => s.SectionC))
                .ForMember(d => d.SectionD,
                           o => o.MapFrom(s => s.SectionD))
                .ForMember(d => d.SectionE,
                           o => o.MapFrom(s => s.SectionE));

            // SectionA → SectionADto
            CreateMap<SectionA, SectionADto>();

            // Form4473FirearmLine → FirearmLineDto
            CreateMap<Form4473FirearmLine, FirearmLineDto>()
                .ForMember(d => d.ManufacturerImporter,
                           o => o.MapFrom(s => s.ManufacturerImporter))
                .ForMember(d => d.Model,
                           o => o.MapFrom(s => s.Model))
                .ForMember(d => d.SerialNumber,
                           o => o.MapFrom(s => s.SerialNumber))
                .ForMember(d => d.Type,
                           o => o.MapFrom(s => Enum.Parse<GunStoreIMS.Shared.Enums.FirearmType>(s.Type)))
                .ForMember(d => d.CaliberGauge,
                           o => o.MapFrom(s => s.CaliberGauge));

            // ────────────────────────────────────
            // DTO → Domain
            // ────────────────────────────────────

            CreateMap<Form4473RecordDto, Form4473Record>()
                .ForMember(e => e.Id,
                           o => o.MapFrom(d => d.Id ?? Guid.NewGuid()))
                .ForMember(e => e.TransferorCertification,
                           o => o.MapFrom(d => d.TransferorCertification))
                // Let AutoMapper map Sections B–E automatically
                .ForMember(e => e.SectionA,
                           o => o.MapFrom(d => d.SectionA))
                .AfterMap((dto, entity) =>
                {
                    // Build the EF collection from the DTO list
                    entity.MapSectionAFirearmsToDomainCollection();
                });

            // SectionADto → SectionA
            CreateMap<SectionADto, SectionA>()
                // All five properties and the list will map conventionally
                // (AutoMapper will map List<FirearmLineDto> → SectionA.Firearms,
                // because we mapped Form4473FirearmLine ↔ FirearmLineDto above)
                ;
        }
    }
}
