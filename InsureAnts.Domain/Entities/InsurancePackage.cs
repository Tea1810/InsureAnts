using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Domain.Entities;

[EntityName("Insurance Package")]
public class InsurancePackage : IEntity<(int, int)>
{
    public required int InsuranceId { get; set; }
    public required int PackageId { get; set; }

    (int, int) IEntity<(int, int)>.Id => (InsuranceId, PackageId);
}
