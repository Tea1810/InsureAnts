using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Packs;

public class AddPackageCommand : ICommand<IResponse<Package>>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double Premium { get; set; }
    public AvailabilityStatus Status { get; set; }
    public int DurationInDays { get; set; }

    public List<Insurance>? Insurances { get; set; }

    public List<ClientPackage>? ClientPackages { get; set; }
}

[UsedImplicitly]
internal class AddPackageCommandValidator : AbstractValidator<Package>
{
    public AddPackageCommandValidator()
    {
        RuleFor(command => command.Name).MaximumLength(50);
        RuleFor(command => command.Description).MaximumLength(200);
        RuleFor(command => command.Status).IsInEnum();
    }
}

internal class AddPackageCommandHandler : ICommandHandler<AddPackageCommand, IResponse<Package>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddPackageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Package>> Handle(AddPackageCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Package>(command);

        _unitOfWork.Packages.Add(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Created<Package>($"for feed {entity.Id}")).For(entity);
    }
}

