using InsureAnts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsureAnts.Infrastructure.Configurations;

internal class InsuranceTypeConfiguration : IEntityTypeConfiguration<InsuranceType>
{
    public void Configure(EntityTypeBuilder<InsuranceType> builder)
    {
        builder.ToTable(nameof(InsuranceType));

        builder.HasKey(x => x.Id);

        builder.Property(it => it.Name).IsRequired().HasMaxLength(100);

        builder.HasMany(it => it.Insurances).WithOne(i => i.InsuranceType).HasForeignKey(i => i.InsuranceTypeId).OnDelete(DeleteBehavior.Cascade);
    }
}
