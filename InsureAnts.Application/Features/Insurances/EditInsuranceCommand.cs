using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Insurances;

public class EditInsuranceCommand : EditCommand<Insurance, Insurance, int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Premium { get; set; }
    public double Coverage { get; set; }
    public int DurationInDays { get; set; }
    public AvailabilityStatus Status { get; set; }

    public InsuranceType? InsuranceType { get; set; } = null;
}

[UsedImplicitly]
internal class EditInsuranceCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditInsuranceCommand, Insurance, Insurance, int>(unitOfWork)
{
    protected override IQueryable<Insurance> GetTrackedQuery() => UnitOfWork.Insurances.AllTracked();
}


[UsedImplicitly]
internal class EditInsuranceCommandValidator : AbstractValidator<Insurance>
{
    public EditInsuranceCommandValidator()
    {
        RuleFor(command => command.Name).MaximumLength(100);
        RuleFor(command => command.Description).MaximumLength(500);
        RuleFor(command => command.Premium).GreaterThanOrEqualTo(1);
        RuleFor(command => command.Coverage).GreaterThanOrEqualTo(1);
        RuleFor(command => command.DurationInDays).GreaterThanOrEqualTo(1);
    }
}
internal class EditInsuranceCommandHandler : ICommandHandler<EditInsuranceCommand, IResponse<Insurance>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditInsuranceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Insurance>> Handle(EditInsuranceCommand command, CancellationToken cancellationToken)
    {
        var client = command.Entity!;

        var entity = _mapper.Map(command, client);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Updated<Insurance>(entity.Id.ToString())).For(entity);
    }
}
