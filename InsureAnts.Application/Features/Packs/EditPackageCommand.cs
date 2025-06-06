﻿using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Packs;

public class EditPackageCommand : EditCommand<Package, Package, int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Premium { get; set; }
    public AvailabilityStatus Status { get; set; }
    public int DurationInDays { get; set; }
    public IEnumerable<Insurance>? Insurances { get; set; } = [];
}

[UsedImplicitly]
internal class EditPackageCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditPackageCommand, Package, Package, int>(unitOfWork)
{
    protected override IQueryable<Package> GetTrackedQuery() => UnitOfWork.Packages.AllTracked().Include(p => p.Insurances);
}

[UsedImplicitly]
internal class EditPackageCommandValidator : AbstractValidator<Package>
{
    public EditPackageCommandValidator()
    {
        RuleFor(command => command.Name).MaximumLength(50);
        RuleFor(command => command.Description).MaximumLength(200);
        RuleFor(command => command.Status).IsInEnum();
        RuleFor(command => command.Premium).GreaterThanOrEqualTo(1);
        RuleFor(command => command.DurationInDays).GreaterThanOrEqualTo(1);
    }
}


internal class EditPackageCommandHandler : ICommandHandler<EditPackageCommand, IResponse<Package>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditPackageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Package>> Handle(EditPackageCommand command, CancellationToken cancellationToken)
    {
        var package = command.Entity!;
        _mapper.Map(command, package);

        package.Insurances!.RemoveAll(i => command.Insurances!.All(ci => ci.Id != i.Id));

        var newInsurances = command.Insurances!.Where(ci => package.Insurances.All(i => i.Id != ci.Id));

        foreach (var newInsurance in newInsurances)
        {
            _unitOfWork.Insurances.Track(newInsurance);
            package.Insurances.Add(newInsurance);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Updated<Package>(package.Id.ToString())).For(package);
    }
}
