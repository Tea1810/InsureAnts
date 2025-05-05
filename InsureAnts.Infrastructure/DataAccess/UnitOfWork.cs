using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Application.DataAccess.Repositories;
using InsureAnts.Domain.Entities;

namespace InsureAnts.Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    #region Private members
    private readonly InsureAntsDbContext _dbContext;
    #endregion


    #region Properties
    public IRepository<Client, int> Clients { get; }
    public IRepository<InsuranceType, int> InsuranceTypes { get; }
    public IRepository<Insurance, int> Insurances { get; }
    public IRepository<Package, int> Packages { get; }
    public IRepository<Deal, int> Deals { get; }
    public IRepository<SupportTicket, int> SupportTickets { get; }
    #endregion


    #region .ctor
    public UnitOfWork(InsureAntsDbContext dbContext)
    {
        _dbContext = dbContext;

        Clients = new Repository<Client, int>(dbContext);
        InsuranceTypes = new Repository<InsuranceType, int>(dbContext);
        Insurances = new Repository<Insurance, int>(dbContext);
        Packages = new Repository<Package, int>(dbContext);
        Deals = new Repository<Deal, int>(dbContext);
        SupportTickets = new Repository<SupportTicket, int>(dbContext);
    }
    #endregion


    #region Public methods
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);
    #endregion
}