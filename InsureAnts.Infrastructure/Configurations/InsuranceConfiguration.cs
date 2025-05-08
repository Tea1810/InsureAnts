using InsureAnts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsureAnts.Infrastructure.Configurations;

internal class InsuranceConfiguration : IEntityTypeConfiguration<Insurance>
{
    public void Configure(EntityTypeBuilder<Insurance> builder)
    {
        builder.ToTable(nameof(Insurance));

        builder.HasKey(x => x.Id);

        builder.Property(i => i.Name).IsRequired().HasMaxLength(100);
        builder.Property(i => i.Description).IsRequired().HasMaxLength(500);

        builder.HasOne(i => i.InsuranceType).WithMany(it => it.Insurances).HasForeignKey(i => i.InsuranceTypeId).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.Packages).WithMany(p => p.Insurances).UsingEntity<InsurancePackage>(
          l => l.HasOne<Package>().WithMany().HasForeignKey(l => l.PackageId).OnDelete(DeleteBehavior.Cascade),
          r => r.HasOne<Insurance>().WithMany().HasForeignKey(r => r.InsuranceId).OnDelete(DeleteBehavior.Cascade),
          j => j.ToTable(nameof(InsurancePackage))
          );
    }
}
