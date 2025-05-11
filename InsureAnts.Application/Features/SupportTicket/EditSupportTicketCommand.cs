using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.SupportTicket
{
    public class EditSupportTicketCommand : EditCommand<InsureAnts.Domain.Entities.SupportTicket, InsureAnts.Domain.Entities.SupportTicket, int>
    {
        public bool WasSeen { get; set; }
    }

    [UsedImplicitly]
    internal class EditSupportTicketCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditSupportTicketCommand, InsureAnts.Domain.Entities.SupportTicket, InsureAnts.Domain.Entities.SupportTicket, int>(unitOfWork)
    {
        protected override IQueryable<InsureAnts.Domain.Entities.SupportTicket> GetTrackedQuery() => UnitOfWork.SupportTickets.AllTracked();
    }

    internal class EditSupportTicketCommandHandler : ICommandHandler<EditSupportTicketCommand, IResponse<InsureAnts.Domain.Entities.SupportTicket>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditSupportTicketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.SupportTicket>> Handle(EditSupportTicketCommand command, CancellationToken cancellationToken)
        {
            var ticket = command.Entity!;

            var entity = _mapper.Map(command, ticket);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Updated<InsureAnts.Domain.Entities.SupportTicket>(entity.Id.ToString())).For(entity);
        }
    }
}
