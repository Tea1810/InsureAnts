using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Clients;

public class AddDealCommand : ICommand<IResponse<Client>>
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

    public List<Deal>? Deals { get; set; }
    public List<Insurance>? Insurances { get; set; }
    public List<ClientPackage>? ClientPackages { get; set; }
    public List<SupportTicket>? Tickets { get; set; }
}

[UsedImplicitly]
internal class AddClientCommandValidator : AbstractValidator<Client>
{
    public AddClientCommandValidator()
    {
        RuleFor(command => command.FirstName).MaximumLength(50);
        RuleFor(command => command.LastName).MaximumLength(50);
        RuleFor(command => command.Email).EmailAddress();
    }
}

internal class AddClientCommandHandler : ICommandHandler<AddDealCommand, IResponse<Client>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Client>> Handle(AddDealCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Client>(command);

        _unitOfWork.Clients.Add(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Created<Client>($"for feed {command.Id}")).For(entity);
    }
}
