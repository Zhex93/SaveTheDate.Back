using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveTheDate.Domain.Guests;

namespace SaveTheDate.Infrastructure.Persistence.Configurations;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("guests");

        builder.HasKey(g => g.Phone);

        builder.Property(g => g.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.HasIndex(g => g.Id).IsUnique();

        builder.Property(g => g.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(g => g.Surname)
            .HasColumnName("surname")
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(g => g.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(g => g.Phone)
            .HasColumnName("phone")
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(g => g.RoleId)
            .HasColumnName("role_id");

        builder.HasIndex(g => g.Name);
        builder.HasIndex(g => g.Email);
    }
}
