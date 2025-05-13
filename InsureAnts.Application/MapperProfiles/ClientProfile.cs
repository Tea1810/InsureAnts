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

        CreateMap<EditClientCommand, Client>();
        CreateMap<Client, EditClientCommand>();
    }
}
