using AutoMapper;
using InsureAnts.Application.Features.Clients;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.MapperProfiles;

internal class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<AddClientCommand, Client>();
        CreateMap<Client, AddClientCommand>();

        CreateMap<EditClientCommand, Client>()
            .ForMember(s => s.Deals, d => d.Ignore())
            .ForMember(s => s.Insurances, d => d.Ignore());
        CreateMap<Client, EditClientCommand>();
    }
}
