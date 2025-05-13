using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.SupportTickets;

public class AddSupportTicketCommand : ICommand<IResponse<SupportTicket>>
{
    public string? Description { get; set; }
    public TicketType TicketType { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public TicketStatus Status { get; set; }
    public Client? Client { get; set; }
}

[UsedImplicitly]
internal class AddSupportTicketCommandValidator : AbstractValidator<SupportTicket>
{
    public AddSupportTicketCommandValidator()
    {
        RuleFor(command => command.Description).MaximumLength(200);
        RuleFor(command => command.TicketType).IsInEnum();
        RuleFor(command => command.Status).IsInEnum();
    }
}

internal class AddSupportTicketCommandHandler : ICommandHandler<AddSupportTicketCommand, IResponse<SupportTicket>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddSupportTicketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<SupportTicket>> Handle(AddSupportTicketCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<SupportTicket>(command);

        _unitOfWork.SupportTickets.Add(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Created<SupportTicket>($"from client {command.Client!.FirstName}")).For(entity);
    }
}
