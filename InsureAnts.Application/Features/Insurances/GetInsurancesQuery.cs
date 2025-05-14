using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Common;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Application.Features.Insurances;

public class GetInsurancesQuery : AbstractQueryRequest<Insurance>
{
    public string SearchTerm { get; set; } = string.Empty;

    public AvailabilityStatus Status { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Premium { get; set; }
    public double Coverage { get; set; }
    public int DurationInDays { get; set; }
    public DateTime CreatedAt { get; set; }
}

internal class GetInsurancesQueryHandler : IQueryHandler<GetInsurancesQuery, QueryResult<Insurance>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInsurancesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ValueTask<QueryResult<Insurance>> Handle(GetInsurancesQuery query, CancellationToken cancellationToken)
    {
        return _unitOfWork.Insurances.All().GetResultAsync(query, cancellationToken).ToValueTask();
    }
}

