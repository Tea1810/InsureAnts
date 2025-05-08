using InsureAnts.Domain.Abstractions;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Domain.Entities;

[EntityName("Client")]
public class Client : Entity<int>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public required string Password { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public AvailabilityStatus Status { get; set; }
    public int NumberOfDeals { get; set; }

    public List<Deal>? Deals{ get; set; }
    public List<Insurance>? Insurances { get; set; }
    public List<ClientPackage>? ClientPackages { get; set; }
    public List<SupportTicket>? Tickets { get; set; }
}
