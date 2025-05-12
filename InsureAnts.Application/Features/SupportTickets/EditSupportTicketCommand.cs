using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.SupportTickets;

public class EditSupportTicketCommand : EditCommand<SupportTicket, SupportTicket, int>
{
    public bool WasSeen { get; set; }
}

[UsedImplicitly]
internal class EditSupportTicketCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditSupportTicketCommand, SupportTicket, SupportTicket, int>(unitOfWork)
{
    protected override IQueryable<SupportTicket> GetTrackedQuery() => UnitOfWork.SupportTickets.AllTracked();
}

internal class EditSupportTicketCommandHandler : ICommandHandler<EditSupportTicketCommand, IResponse<SupportTicket>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditSupportTicketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<SupportTicket>> Handle(EditSupportTicketCommand command, CancellationToken cancellationToken)
    {
        var ticket = command.Entity!;

        var entity = _mapper.Map(command, ticket);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Updated<SupportTicket>(entity.Id.ToString())).For(entity);
    }
}
