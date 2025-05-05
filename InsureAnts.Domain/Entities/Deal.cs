using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Domain.Entities;

[EntityName("Deal")]
public class Deal : Entity<int>
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int DurationInDays { get; set; }
    public double DiscountPercentage { get; set; }

    public List<ClientPackage>? ClientPackages { get; set; }
    public List<Client>? Clients { get; set; }
}
