using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Common;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.Features.Deals;

public class GetDealsQuery : AbstractQueryRequest<Deal>
{
    public string SearchTerm { get; set; } = string.Empty;

    public override IQueryable<Deal> ApplyFilter(IQueryable<Deal> source)
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            source = source.Where(c => c.Name.Contains(SearchTerm) || c.Description.Contains(SearchTerm));
        }

        return base.ApplyFilter(source);
    }
}

internal class GetDealsQueryHandler : IQueryHandler<GetDealsQuery, QueryResult<Deal>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDealsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ValueTask<QueryResult<Deal>> Handle(GetDealsQuery query, CancellationToken cancellationToken)
    {
        return _unitOfWork.Deals.All().GetResultAsync(query, cancellationToken).ToValueTask();
    }
}
