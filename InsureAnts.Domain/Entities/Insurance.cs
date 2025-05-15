using InsureAnts.Domain.Abstractions;
using InsureAnts.Domain.Enums;

namespace InsureAnts.Domain.Entities;

[EntityName("Insurance")]
public class Insurance : Entity<int>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double Premium { get; set; }
    public double Coverage { get; set; }
    public int DurationInDays { get; set; }
    public AvailabilityStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int InsuranceTypeId { get; set; }

    public InsuranceType? InsuranceType { get; set; }
    public List<Package>? Packages { get; set; }
    public List<Client>? Clients { get; set; }

    public override string ToString() => Name;

    public override bool Equals(object? obj)
    {
        if (obj is not Insurance other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

}
