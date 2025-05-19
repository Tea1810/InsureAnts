using InsureAnts.Application.Data_Queries;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.Features.Abstractions;
using InsureAnts.Common;
using InsureAnts.Domain.Entities;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Application.Features.SupportTickets;

public class GetSupportTicketsQuery : AbstractQueryRequest<SupportTicket>
{
    public string SearchTerm { get; set; } = string.Empty;
    public TicketType TicketType { get; set; }
    public TicketStatus Status { get; set; }
    public string? Description { get; set; }
    public DateTime? AppointmentDate { get; set; }

    public override IQueryable<SupportTicket> ApplyFilter(IQueryable<SupportTicket> source)
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            source = source.Where(c => c.Client!.FirstName.Contains(SearchTerm) || c.Client.LastName.Contains(SearchTerm));
        }

        source = source.Where(c => c.Status == Status);

        return base.ApplyFilter(source);
    }
}

internal class GetSupportTicketsQueryHandler : IQueryHandler<GetSupportTicketsQuery, QueryResult<SupportTicket>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSupportTicketsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public ValueTask<QueryResult<SupportTicket>> Handle(GetSupportTicketsQuery query, CancellationToken cancellationToken)
    {
        return _unitOfWork.SupportTickets.All().GetResultAsync(query, cancellationToken).ToValueTask();
    }
}
