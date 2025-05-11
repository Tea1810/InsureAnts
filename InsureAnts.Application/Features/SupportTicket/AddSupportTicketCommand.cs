using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.SupportTicket
{
    public class AddSupportTicketCommand : ICommand<IResponse<InsureAnts.Domain.Entities.SupportTicket>>
    {   
        public required int Id { get; set; }
        public string? Description { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public TicketStatus Status { get; set; }
        public int ClientId { get; set; }

        public InsureAnts.Domain.Entities.Client? Client { get; set; }
    }

    [UsedImplicitly]
    internal class AddSupportTicketCommandValidator : AbstractValidator<InsureAnts.Domain.Entities.SupportTicket>
    {
        public AddSupportTicketCommandValidator()
        {
            RuleFor(command => command.Description).MaximumLength(200);
            RuleFor(command => command.TicketType).IsInEnum();
            RuleFor(command => command.Status).IsInEnum();
        }
    }

    internal class AddSupportTicketCommandHandler : ICommandHandler<AddSupportTicketCommand, IResponse<InsureAnts.Domain.Entities.SupportTicket>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddSupportTicketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.SupportTicket>> Handle(AddSupportTicketCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<InsureAnts.Domain.Entities.SupportTicket>(command);

            _unitOfWork.SupportTickets.Add(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Created<InsureAnts.Domain.Entities.SupportTicket>($"for feed {command.Id}")).For(entity);
        }
    }
}
