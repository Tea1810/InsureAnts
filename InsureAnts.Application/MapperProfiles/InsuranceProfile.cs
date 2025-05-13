using AutoMapper;
using InsureAnts.Application.Features.Insurances;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.MapperProfiles;

internal class InsuranceProfile : Profile
{
    public InsuranceProfile() 
    {
        CreateMap<AddInsuranceCommand, Insurance>();
        CreateMap<Insurance, AddInsuranceCommand>();

        CreateMap<EditInsuranceCommand, Insurance>();
        CreateMap<Insurance, EditInsuranceCommand>();
    }
}
