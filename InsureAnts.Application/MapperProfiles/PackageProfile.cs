using AutoMapper;
using InsureAnts.Application.Features.Insurances;
using InsureAnts.Application.Features.Packs;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.MapperProfiles;

internal class PackageProfile : Profile
{
    public PackageProfile()
    {
        CreateMap<AddPackageCommand, Package>();
        CreateMap<Package, AddPackageCommand>();

        CreateMap<EditPackageCommand, Package>()
            .ForMember(s => s.Insurances, d => d.Ignore());
        CreateMap<Package, EditPackageCommand>();
    }
}
