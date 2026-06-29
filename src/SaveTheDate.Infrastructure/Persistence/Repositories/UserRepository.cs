using Microsoft.EntityFrameworkCore;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Users;
using SaveTheDate.Infrastructure.Persistence;

namespace SaveTheDate.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _db.Users.FindAsync([id], cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _db.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await _db.Users.AddAsync(user, cancellationToken);
}
