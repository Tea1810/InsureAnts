using InsureAnts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsureAnts.Infrastructure.Configurations;

internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable(nameof(Client));

        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Phone).IsRequired().HasMaxLength(15);
        builder.Property(c => c.Address).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Password).IsRequired();

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        //one-to-many
        builder.HasMany(c => c.Tickets).WithOne(t => t.Client).HasForeignKey(t => t.ClientId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.ClientPackages).WithOne(cp => cp.Client).HasForeignKey(cp => cp.ClientId).OnDelete(DeleteBehavior.Cascade);

        //many - to - many
        builder.HasMany(c => c.Deals).WithMany(d => d.Clients).UsingEntity<ClientDeal>(
            r => r.HasOne<Deal>().WithMany().HasForeignKey(r => r.DealId).OnDelete(DeleteBehavior.Cascade),
            l => l.HasOne<Client>().WithMany().HasForeignKey(l => l.ClientId).OnDelete(DeleteBehavior.Cascade),
            j => j.ToTable(nameof(ClientDeal))
            );

        builder.HasMany(c => c.Insurances).WithMany(i => i.Clients).UsingEntity<ClientInsurance>(
           r => r.HasOne<Insurance>().WithMany().HasForeignKey(r => r.InsuranceId).OnDelete(DeleteBehavior.Cascade),
           l => l.HasOne<Client>().WithMany().HasForeignKey(l => l.ClientId).OnDelete(DeleteBehavior.Cascade),
           j => j.ToTable(nameof(ClientInsurance))
           );
    }
}

