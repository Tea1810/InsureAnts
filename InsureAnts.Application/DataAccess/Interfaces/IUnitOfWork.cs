using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Application.DataAccess.Interfaces;

public interface IUnitOfWork
{
    IRepository<Client, int> Clients { get; }
    IRepository<InsuranceType, int> InsuranceTypes { get; }
    IRepository<Insurance, int> Insurances { get; }
    IRepository<Package, int> Packages { get; }
    IRepository<Deal, int> Deals { get; }
    IRepository<SupportTicket, int> SupportTickets { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
