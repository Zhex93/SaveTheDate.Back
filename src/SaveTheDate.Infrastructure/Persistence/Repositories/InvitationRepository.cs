using Microsoft.EntityFrameworkCore;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Invitations;
using SaveTheDate.Infrastructure.Persistence;

namespace SaveTheDate.Infrastructure.Persistence.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly AppDbContext _db;

    public InvitationRepository(AppDbContext db) => _db = db;

    public async Task<Invitation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _db.Invitations.FindAsync([id], cancellationToken);

    public async Task<Invitation?> GetByAccessTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        => await _db.Invitations
            .Include(i => i.Guest)
            .FirstOrDefaultAsync(i => i.AccessTokenHash == tokenHash, cancellationToken);

    public async Task<IEnumerable<Invitation>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.Invitations.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(Invitation invitation, CancellationToken cancellationToken = default)
        => await _db.Invitations.AddAsync(invitation, cancellationToken);

    public void Update(Invitation invitation) => _db.Invitations.Update(invitation);

    public void Delete(Invitation invitation) => _db.Invitations.Remove(invitation);
}
