using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Clients;

public class AddClientCommand : ICommand<IResponse<Client>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public AvailabilityStatus Status { get; set; }
    public int NumberOfDeals { get; set; } = 1;
}

[UsedImplicitly]
internal class AddClientCommandValidator : AbstractValidator<Client>
{
    public AddClientCommandValidator()
    {
        RuleFor(command => command.FirstName).MaximumLength(50).NotEmpty();
        RuleFor(command => command.LastName).MaximumLength(50).NotEmpty();
        RuleFor(command => command.Email).EmailAddress().MaximumLength(100).NotEmpty();
        RuleFor(command => command.Phone).MaximumLength(15).NotEmpty();
        RuleFor(command => command.Address).NotEmpty().MaximumLength(100);
    }
}

internal class AddClientCommandHandler : ICommandHandler<AddClientCommand, IResponse<Client>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Client>> Handle(AddClientCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Client>(command);

        _unitOfWork.Clients.Add(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Created<Client>($"client with the name {entity.FirstName} {entity.LastName}")).For(entity);
    }
}
