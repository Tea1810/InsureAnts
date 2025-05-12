using AutoMapper;
using InsureAnts.Application.Features.InsuranceTypes;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.MapperProfiles;

internal class InsuranceTypeProfile : Profile
{
    public InsuranceTypeProfile() 
    {
        CreateMap<AddInsuranceTypeCommand, InsuranceType>();
        CreateMap<InsuranceType, AddInsuranceTypeCommand>();

        CreateMap<EditInsuranceTypeCommand, InsuranceType>();
        CreateMap<InsuranceType, EditInsuranceTypeCommand>();
    }
}
