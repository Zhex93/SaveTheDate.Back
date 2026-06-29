using Microsoft.EntityFrameworkCore;
using SaveTheDate.Domain.Guests;
using SaveTheDate.Domain.Invitations;
using SaveTheDate.Domain.Roles;
using SaveTheDate.Domain.Users;

namespace SaveTheDate.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
