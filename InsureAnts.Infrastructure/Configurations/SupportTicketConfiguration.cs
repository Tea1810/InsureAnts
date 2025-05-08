using InsureAnts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsureAnts.Infrastructure.Configurations;

internal class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.ToTable(nameof(SupportTicket));

        builder.HasKey(st => st.Id);

        builder.HasOne(st => st.Client).WithMany(c => c.Tickets).HasForeignKey(st => st.ClientId).OnDelete(DeleteBehavior.Cascade);
    }
}
