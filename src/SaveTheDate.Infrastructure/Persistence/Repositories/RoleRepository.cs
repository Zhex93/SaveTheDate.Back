using Microsoft.EntityFrameworkCore;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Roles;
using SaveTheDate.Infrastructure.Persistence;

namespace SaveTheDate.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _db;

    public RoleRepository(AppDbContext db) => _db = db;

    public async Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _db.Roles.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.Roles.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
        => await _db.Roles.AddAsync(role, cancellationToken);

    public void Update(Role role) => _db.Roles.Update(role);

    public void Delete(Role role) => _db.Roles.Remove(role);
}
