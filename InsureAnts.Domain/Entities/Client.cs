using InsureAnts.Domain.Abstractions;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Domain.Entities;

[EntityName("Client")]
public class Client : Entity<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public AvailabilityStatus Status { get; set; }
    public int NumberOfDeals { get; set; }

    public List<Deal>? Deals { get; set; }
    public List<Insurance>? Insurances { get; set; }
    public List<ClientPackage>? ClientPackages { get; set; }
    public List<SupportTicket>? Tickets { get; set; }

    public override string ToString() => FirstName + LastName;
}
