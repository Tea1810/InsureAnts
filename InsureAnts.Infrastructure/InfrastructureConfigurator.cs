using InsureAnts.Application.DataAccess;
using InsureAnts.Application.DataAccess.Interfaces;
using InsureAnts.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsureAnts.Infrastructure;

public static class InfrastructureConfigurator
{
    static InfrastructureConfigurator() => QueryableAsyncResolver.Instance = EFQueryableAsyncResolver.Instance;

    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("InsureAntsDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ConnectionString 'InsureAntsDb' cannot be empty.");
        }

        services.AddDbContext<InsureAntsDbContext>(options =>
        {

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}