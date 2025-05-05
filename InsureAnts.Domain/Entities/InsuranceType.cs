using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Domain.Entities;

[EntityName("Insurance Type")]
public class InsuranceType : Entity<int>
{
    public required string Name { get; set; }

    public List<Insurance>? Insurances { get; set; }
}
