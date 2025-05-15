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

    public IEnumerable<Deal>? Deals { get; set; }
    public IEnumerable<Insurance>? Insurances { get; set; }
    public IEnumerable<ClientPackage>? ClientPackages { get; set; }
}

[UsedImplicitly]
internal class EditClientCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditClientCommand, Client, Client, int>(unitOfWork)
{
    protected override IQueryable<Client> GetTrackedQuery() => UnitOfWork.Clients.AllTracked().Include(c => c.Deals).Include(c => c.Insurances);
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
        _mapper.Map(command, client);

        client.Deals!.RemoveAll(d => command.Deals!.All(cd => cd.Id != d.Id));
        client.Insurances!.RemoveAll(i => command.Insurances!.All(ci => ci.Id != i.Id));

        var newDeals = command.Deals!.Where(cd => client.Deals.All(d => d.Id != cd.Id));
        var newInsurances = command.Insurances!.Where(ci => client.Insurances.All(i => i.Id != ci.Id));

        foreach (var newDeal in newDeals)
        {
            _unitOfWork.Deals.Track(newDeal);
            client.Deals.Add(newDeal);
        }

        foreach (var newInsurance in newInsurances)
        {
            _unitOfWork.Insurances.Track(newInsurance);
            client.Insurances.Add(newInsurance);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Updated<Client>(client.Id.ToString())).For(client);
    }
}
