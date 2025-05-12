using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.Features.SupportTickets;

public class DeleteSupportTicketCommand : DeleteCommand<int>;
internal class DeleteSupportTicketCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteSupportTicketCommand, SupportTicket, int>(unitOfWork)
{
    protected override IRepository<SupportTicket, int> GetRepository() => UnitOfWork.SupportTickets;
}
