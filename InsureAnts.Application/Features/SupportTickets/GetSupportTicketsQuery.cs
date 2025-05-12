using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Application.Features.SupportTickets;

public class GetSupportTicketsQuery : AbstractQueryRequest<Client>
{
    public string SearchTerm { get; set; } = string.Empty;
    public TicketType TicketType { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public TicketStatus Status { get; set; }

    public override IQueryable<SupportTicket> ApplyFilter(IQueryable<SupportTicket> source)
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            source = source.Where(c => c.FirstName.Contains(SearchTerm) || c.LastName.Contains(SearchTerm));
        }

        source = source.Where(c => c.Status == Status);

        return base.ApplyFilter(source);
    }
}

internal class GetFeedAlertsQueryHandler : IQueryHandler<GetSupportTicketsQuery, QueryResult<Client>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFeedAlertsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ValueTask<QueryResult<Client>> Handle(GetSupportTicketsQuery query, CancellationToken cancellationToken)
    {
        return _unitOfWork.Clients.All().GetResultAsync(query, cancellationToken).ToValueTask();
    }
}
