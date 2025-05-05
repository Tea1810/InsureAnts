using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Domain.Entities;

[EntityName("Client Package")]
public class ClientPackage : IEntity<(int, int)>
{
    public required int ClientId { get; set; }
    public required int PackageId { get; set; }
    public int DealId { get; set; }
    public DateTime StartDate { get; set; }

    public Deal? Deal { get; set; }
    (int, int) IEntity<(int, int)>.Id => (ClientId, PackageId);
}

