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
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Premium { get; set; }
    public AvailabilityStatus Status { get; set; }
    public int DurationInDays { get; set; }

    public IEnumerable<Insurance> Insurances { get; set; } = [];
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

        foreach (var item in entity.Insurances!)
        {
            _unitOfWork.Insurances.Track(item);
        }

        _unitOfWork.Packages.Add(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _unitOfWork.Insurances.UntrackAll();

        return Response.Success(Texts.Created<Package>($"for feed {entity.Id}")).For(entity);
    }
}

