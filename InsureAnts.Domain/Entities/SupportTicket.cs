using InsureAnts.Domain.Abstractions;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Domain.Entities;

[EntityName("Support Ticket")]
public class SupportTicket : Entity<int>
{
    public string? Description { get; set; }
    public TicketType TicketType { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public TicketStatus Status { get; set; }
    public int ClientId { get; set; }

    public Client? Client { get; set; }
}
