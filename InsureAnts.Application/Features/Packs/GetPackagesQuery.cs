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
    public AvailabilityStatus Status { get; set; }

    public override IQueryable<Package> ApplyFilter(IQueryable<Package> source)
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            source = source.Where(c => c.Name.Contains(SearchTerm));
        }

        source = source.Where(c => c.Status == Status);

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
        return _unitOfWork.Packages.All().GetResultAsync(query, cancellationToken).ToValueTask();
    }
}

