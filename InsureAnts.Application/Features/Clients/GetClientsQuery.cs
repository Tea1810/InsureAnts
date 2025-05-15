using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Common;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Application.Features.Clients;

public class GetClientsQuery : AbstractQueryRequest<Client>
{
    public string SearchTerm { get; set; } = string.Empty;

    public AvailabilityStatusFilter StatusFilter { get; set; } = AvailabilityStatusFilter.All;

    public GenderFilter GenderFilter { get; set; } = GenderFilter.All;

    public override IQueryable<Client> ApplyFilter(IQueryable<Client> source)
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            source = source.Where(c => c.FirstName.Contains(SearchTerm) || c.LastName.Contains(SearchTerm));
        }

        source = StatusFilter switch
        {
            AvailabilityStatusFilter.Active => source.Where(c => c.Status == AvailabilityStatus.Active),
            AvailabilityStatusFilter.Inactive => source.Where(c => c.Status == AvailabilityStatus.Inactive),
            _ => source
        };

        source = GenderFilter switch
        {
            GenderFilter.Male => source.Where(c => c.Gender == Gender.Male),
            GenderFilter.Female => source.Where(c => c.Gender == Gender.Female),
            _ => source
        };

        return base.ApplyFilter(source);
    }
}

internal class GetFeedAlertsQueryHandler : IQueryHandler<GetClientsQuery, QueryResult<Client>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFeedAlertsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ValueTask<QueryResult<Client>> Handle(GetClientsQuery query, CancellationToken cancellationToken)
    {
        return _unitOfWork.Clients.All().GetResultAsync(query, cancellationToken).ToValueTask();
    }
}
