using InsureAnts.Domain.Abstractions;

namespace InsureAnts.Domain.Entities;

[EntityName("Client Insurance")]
public class ClientInsurance : IEntity<(int, int)>
{
    public required int ClientId { get; set; }
    public required int InsuranceId { get; set; }
    public DateTime StartDate { get; set; }

    (int, int) IEntity<(int, int)>.Id => (ClientId, InsuranceId);
}

