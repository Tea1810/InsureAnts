using InsureAnts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsureAnts.Infrastructure.Configurations;

internal class ClientPackageConfiguration : IEntityTypeConfiguration<ClientPackage>
{
    public void Configure(EntityTypeBuilder<ClientPackage> builder)
    {
        builder.ToTable(nameof(ClientPackage));

        builder.HasKey(cp => new { cp.ClientId, cp.PackageId }); // Composite key

        builder.HasOne(cp => cp.Client).WithMany(c => c.ClientPackages).HasForeignKey(cp => cp.ClientId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(cp => cp.Package).WithMany(p => p.ClientPackages).HasForeignKey(cp => cp.PackageId).OnDelete(DeleteBehavior.Cascade);
    }
}
