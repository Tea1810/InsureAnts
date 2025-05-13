using AutoMapper;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using JetBrains.Annotations;

namespace InsureAnts.Application.Features.Insurances;

public class EditInsuranceCommand : EditCommand<Insurance, Insurance, int>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public AvailabilityStatus Status { get; set; }
}

[UsedImplicitly]
internal class EditInsuranceCommandInitializer(IUnitOfWork unitOfWork) : EditCommandInitializer<EditInsuranceCommand, Insurance, Insurance, int>(unitOfWork)
{
    protected override IQueryable<Insurance> GetTrackedQuery() => UnitOfWork.Insurances.AllTracked();
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
