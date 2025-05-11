using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Application.Features.Abstractions;

namespace InsureAnts.Application.Features.SupportTicket
{
    public class DeleteSupportTicketCommand : DeleteCommand<int>;
    internal class DeleteSupportTicketCommandHandler(IUnitOfWork unitOfWork) : DeleteCommandHandler<DeleteSupportTicketCommand, InsureAnts.Domain.Entities.SupportTicket, int>(unitOfWork)
    {
        protected override IRepository<InsureAnts.Domain.Entities.SupportTicket, int> GetRepository() => UnitOfWork.SupportTickets;
    }
}
