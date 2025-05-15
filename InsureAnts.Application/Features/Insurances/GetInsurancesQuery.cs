using System.Reflection;
using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Common;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace InsureAnts.Application.Features.Insurances;

public class GetInsurancesQuery : AbstractQueryRequest<Insurance>
{
    public string SearchTerm { get; set; } = string.Empty;
    public AvailabilityStatusFilter StatusFilter { get; set; } = AvailabilityStatusFilter.All;
    public string InsuranceTypeFilter { get; set; } = string.Empty;

    public override IQueryable<Insurance> ApplyFilter(IQueryable<Insurance> source)
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            source = source.Where(i => i.Name.Contains(SearchTerm));
        }

        if (!string.IsNullOrEmpty(InsuranceTypeFilter))
        {
            source = source.Where(i => i.InsuranceType!.Name.Contains(InsuranceTypeFilter));
        }

        source = StatusFilter switch
        {
            AvailabilityStatusFilter.Active => source.Where(i => i.Status == AvailabilityStatus.Active),
            AvailabilityStatusFilter.Inactive => source.Where(i => i.Status == AvailabilityStatus.Inactive),
            _ => source
        };

        return base.ApplyFilter(source);
    }
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
        return _unitOfWork.Insurances.All().Include(i => i.InsuranceType).GetResultAsync(query, cancellationToken).ToValueTask();
    }
}

