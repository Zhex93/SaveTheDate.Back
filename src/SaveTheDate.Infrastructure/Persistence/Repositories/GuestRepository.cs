using Microsoft.EntityFrameworkCore;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Guests;
using SaveTheDate.Infrastructure.Persistence;

namespace SaveTheDate.Infrastructure.Persistence.Repositories;

public class GuestRepository : IGuestRepository
{
    private readonly AppDbContext _db;

    public GuestRepository(AppDbContext db) => _db = db;

    public async Task<Guest?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _db.Guests.SingleOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task<IEnumerable<Guest>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.Guests.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(Guest guest, CancellationToken cancellationToken = default)
        => await _db.Guests.AddAsync(guest, cancellationToken);

    public void Update(Guest guest) => _db.Guests.Update(guest);

    public void Delete(Guest guest) => _db.Guests.Remove(guest);
}
