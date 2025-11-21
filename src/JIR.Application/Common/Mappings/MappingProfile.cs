using AutoMapper;
using JIR.Application.Cotisations.Queries.GetCotisationById;
using JIR.Application.Cotisations.Queries.GetCotisations;
using JIR.Application.Sections.Queries.GetSectionById;
using JIR.Application.Sections.Queries.GetSections;
using JIR.Domain.Entities;

namespace JIR.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Section mappings
        CreateMap<Section, SectionDto>()
            .ForMember(d => d.ResponsableNom, opt => opt.MapFrom(s => 
                s.Responsable != null ? $"{s.Responsable.Prenom} {s.Responsable.Nom}" : null));

        CreateMap<Section, SectionDetailDto>()
            .ForMember(d => d.ResponsableNom, opt => opt.MapFrom(s => 
                s.Responsable != null ? $"{s.Responsable.Prenom} {s.Responsable.Nom}" : null))
            .ForMember(d => d.NombreMembres, opt => opt.MapFrom(s => s.Members.Count))
            .ForMember(d => d.NombreCotisations, opt => opt.MapFrom(s => s.Cotisations.Count))
            .ForMember(d => d.NombreDepenses, opt => opt.MapFrom(s => s.Depenses.Count));

        // Cotisation mappings
        CreateMap<Cotisation, CotisationDto>()
            .ForMember(d => d.SectionNom, opt => opt.MapFrom(s => s.Section.Nom));

        CreateMap<Cotisation, CotisationDetailDto>()
            .ForMember(d => d.SectionNom, opt => opt.MapFrom(s => s.Section.Nom))
            .ForMember(d => d.SectionCode, opt => opt.MapFrom(s => s.Section.Code));
    }
}
