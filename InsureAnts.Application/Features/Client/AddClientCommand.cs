using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Client
{
    public class AddDealCommand: ICommand<IResponse<InsureAnts.Domain.Entities.Client>>
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public AvailabilityStatus Status { get; set; }
        public int NumberOfDeals { get; set; }

        public List<InsureAnts.Domain.Entities.Deal>? Deals { get; set; }
        public List<Insurance>? Insurances { get; set; }
        public List<ClientPackage>? ClientPackages { get; set; }
        public List<SupportTicket>? Tickets { get; set; }
    }

    [UsedImplicitly]
    internal class AddClientCommandValidator : AbstractValidator<InsureAnts.Domain.Entities.Client>
    {
        public AddClientCommandValidator()
        {
            RuleFor(command => command.FirstName).MaximumLength(50);
            RuleFor(command => command.LastName).MaximumLength(50);
            RuleFor(command => command.Email).EmailAddress();
            RuleFor(command => command.Gender).IsInEnum();
            RuleFor(command => command.Status).IsInEnum();
        }
    }

    internal class AddClientCommandHandler : ICommandHandler<AddDealCommand, IResponse<InsureAnts.Domain.Entities.Client>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async ValueTask<IResponse<InsureAnts.Domain.Entities.Client>> Handle(AddDealCommand command, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<InsureAnts.Domain.Entities.Client>(command);

            _unitOfWork.Clients.Add(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response.Success(Texts.Created<InsureAnts.Domain.Entities.Client>($"for feed {command.Id}")).For(entity);
        }
    }
}
