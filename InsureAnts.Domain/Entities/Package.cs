using InsureAnts.Domain.Abstractions;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Domain.Entities;

[EntityName("Package")]
public class Package : Entity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Premium { get; set; }
    public AvailabilityStatus Status { get; set; }
    public int DurationInDays { get; set; }

    public List<Insurance>? Insurances { get; set; }

    public List<ClientPackage>? ClientPackages { get; set; }
}
