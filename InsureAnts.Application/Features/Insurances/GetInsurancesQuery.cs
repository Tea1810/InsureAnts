using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Application.Features.Insurances;

public class GetClientsQuery : AbstractQueryRequest<Client>
{
    public string SearchTerm { get; set; } = string.Empty;

    public AvailabilityStatus Status { get; set; }

    public Gender Gender { get; set; }

    public override IQueryable<Client> ApplyFilter(IQueryable<Client> source)
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            source = source.Where(c => c.FirstName.Contains(SearchTerm) || c.LastName.Contains(SearchTerm));
        }

        source = source.Where(c => c.Status == Status);

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

