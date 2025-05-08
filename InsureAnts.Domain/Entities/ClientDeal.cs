using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Domain.Entities;
[EntityName("Client Deal")]
public class ClientDeal : IEntity<(int, int)>
{
    public required int ClientId { get; set; }
    public required int DealId { get; set; }

    public Client? Client { get; set; }

    public Deal? Deal { get; set; }
    (int, int) IEntity<(int, int)>.Id => (ClientId, DealId);
}
