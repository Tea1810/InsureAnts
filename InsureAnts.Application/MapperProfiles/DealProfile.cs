using AutoMapper;
using InsureAnts.Application.Features.Deals;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.MapperProfiles;

internal class DealProfile : Profile
{
    public DealProfile() 
    {
        CreateMap<AddDealCommand, Deal>();
        CreateMap<Deal, AddDealCommand>();

        CreateMap<EditDealCommand, Deal>();
        CreateMap<Deal, EditDealCommand>();
    }
}
