using InsureAnts.Domain.Abstractions;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Domain.Entities;

[EntityName("Package")]
public class Package : Entity<int>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double Premium { get; set; }
    public AvailabilityStatus Status { get; set; }
    public int DurationInDays { get; set; }

    public List<Insurance>? Insurances { get; set; }
    public List<Client>? Clients { get; set; }
}
