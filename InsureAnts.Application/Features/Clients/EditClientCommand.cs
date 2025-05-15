using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Clients;

public class EditClientCommand : EditCommand<Client, Client, int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public AvailabilityStatus Status { get; set; }
    public int NumberOfDeals { get; set; }

    public List<Deal>? Deals { get; set; }
    public List<Insurance>? Insurances { get; set; }
    public List<ClientPackage>? ClientPackages { get; set; }
}

[UsedImplicitly]
internal class EditClientCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditClientCommand, Client, Client, int>(unitOfWork)
{
    protected override IQueryable<Client> GetTrackedQuery() => UnitOfWork.Clients.AllTracked();
}


[UsedImplicitly]
internal class EditClientCommandValidator : AbstractValidator<Client>
{
    public EditClientCommandValidator()
    {
        RuleFor(command => command.FirstName).MaximumLength(50).NotEmpty();
        RuleFor(command => command.LastName).MaximumLength(50).NotEmpty();
        RuleFor(command => command.Email).EmailAddress().MaximumLength(100).NotEmpty();
        RuleFor(command => command.Phone).MaximumLength(15).NotEmpty();
        RuleFor(command => command.Address).NotEmpty().MaximumLength(100);
    }
}

internal class EditClientCommandHandler : ICommandHandler<EditClientCommand, IResponse<Client>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Client>> Handle(EditClientCommand command, CancellationToken cancellationToken)
    {
        var client = command.Entity!;

        var entity = _mapper.Map(command, client);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Updated<Client>(entity.Id.ToString())).For(entity);
    }
}
