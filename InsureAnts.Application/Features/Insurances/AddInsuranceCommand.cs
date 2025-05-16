using AutoMapper;
using FluentValidation;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Insurances;

public class AddInsuranceCommand : ICommand<IResponse<Insurance>>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Premium { get; set; }
    public double Coverage { get; set; }
    public int DurationInDays { get; set; }
    public AvailabilityStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public InsuranceType? InsuranceType { get; set; } = null;
}

[UsedImplicitly]
internal class AddInsuranceCommandValidator : AbstractValidator<Insurance>
{
    public AddInsuranceCommandValidator()
    {
        RuleFor(command => command.Name).MaximumLength(100);
        RuleFor(command => command.Description).MaximumLength(500);
    }
}

internal class AddInsuranceCommandHandler : ICommandHandler<AddInsuranceCommand, IResponse<Insurance>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddInsuranceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<IResponse<Insurance>> Handle(AddInsuranceCommand command, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Insurance>(command);

        _unitOfWork.InsuranceTypes.Track(entity.InsuranceType!);

        _unitOfWork.Insurances.Add(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Response.Success(Texts.Created<Insurance>(entity.Name)).For(entity);
    }
}
