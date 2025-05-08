using InsureAnts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsureAnts.Infrastructure.Configurations;

internal class DealConfiguration : IEntityTypeConfiguration<Deal>
{
    public void Configure(EntityTypeBuilder<Deal> builder)
    {
        builder.ToTable(nameof(Deal));

        builder.HasKey(x => x.Id);

        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(500);

        builder.HasMany(d => d.ClientPackages).WithOne(cp => cp.Deal).HasForeignKey(cp => cp.DealId);
    }
}
