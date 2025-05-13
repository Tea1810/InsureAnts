using AutoMapper;
using InsureAnts.Application.Features.SupportTickets;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.MapperProfiles;

internal class SupportTicketProfile : Profile
{
    public SupportTicketProfile () {
        CreateMap<AddSupportTicketCommand, SupportTicket>();
        CreateMap<SupportTicket, AddSupportTicketCommand>();
    }
}
