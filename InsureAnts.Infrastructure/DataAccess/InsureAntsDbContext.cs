using InsureAnts.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InsureAnts.Infrastructure.DataAccess;

public class InsureAntsDbContext : IdentityDbContext<IdentityUser>
{
    public InsureAntsDbContext(DbContextOptions<InsureAntsDbContext> options) : base(options)
    { }

    public required DbSet<Client> Clients { get; set; }
    public required DbSet<InsuranceType> InsuranceTypes { get; set; }
    public required DbSet<Insurance> Insurances { get; set; }
    public required DbSet<Package> Packages { get; set; }
    public required DbSet<Deal> Deals { get; set; }
    public required DbSet<SupportTicket> SupportTickets { get; set; }
    public required DbSet<ClientPackage> ClientPackages { get; set; }
    public required DbSet<ClientInsurance> ClientInsurances { get; set; }
    public required DbSet<ClientDeal> ClientDeals { get; set; }
    public required DbSet<InsurancePackage> InsurancePackages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InsureAntsDbContext).Assembly);

        modelBuilder.HasAnnotation("Relational:HistoryTableName", "__efMigrationsHistory_SqlServer");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
    }
}