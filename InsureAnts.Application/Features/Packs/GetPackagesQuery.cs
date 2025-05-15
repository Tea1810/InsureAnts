using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Common;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Application.Features.Packages;

public class GetPackagesQuery : AbstractQueryRequest<Package>
{
    public string SearchTerm { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public AvailabilityStatusFilter StatusFilter { get; set; } = AvailabilityStatusFilter.All;

    public string InsuranceFilter { get; set; } = string.Empty;

    public override IQueryable<Package> ApplyFilter(IQueryable<Package> source)
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            source = source.Where(p => p.Name.Contains(SearchTerm));
        }

        if (!string.IsNullOrEmpty(InsuranceFilter))
        {
            source = source.Where(p => p.Insurances!.Select(i => i.Name).ToList().Contains(InsuranceFilter));
        }

        source = StatusFilter switch
        {
            AvailabilityStatusFilter.Active => source.Where(p => p.Status == AvailabilityStatus.Active),
            AvailabilityStatusFilter.Inactive => source.Where(p => p.Status == AvailabilityStatus.Inactive),
            _ => source
        };

        return base.ApplyFilter(source);
    }
}

internal class GetPackagesQueryHandler : IQueryHandler<GetPackagesQuery, QueryResult<Package>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPackagesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ValueTask<QueryResult<Package>> Handle(GetPackagesQuery query, CancellationToken cancellationToken)
    {
        return _unitOfWork.Packages.All().Include(p => p.Insurances).GetResultAsync(query, cancellationToken).ToValueTask();
    }
}

