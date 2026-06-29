using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveTheDate.Domain.Invitations;

namespace SaveTheDate.Infrastructure.Persistence.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("invitations");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(i => i.GuestId)
            .HasColumnName("guest_id");

        builder.Property(i => i.State)
            .HasColumnName("state")
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(i => i.Observations)
            .HasColumnName("observations")
            .HasMaxLength(1024);

        builder.HasOne<Domain.Guests.Guest>()
            .WithMany()
            .HasForeignKey(i => i.GuestId)
            .HasPrincipalKey(g => g.Id);
    }
}
