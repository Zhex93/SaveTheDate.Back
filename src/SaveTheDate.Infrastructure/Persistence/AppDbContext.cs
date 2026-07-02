using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaveTheDate.Domain.Guests;
using SaveTheDate.Domain.Invitations;
using SaveTheDate.Domain.Roles;

namespace SaveTheDate.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Invitation> Invitations => Set<Invitation>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
