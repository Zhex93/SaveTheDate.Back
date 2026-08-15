using SaveTheDate.Domain.Invitations;

namespace SaveTheDate.Domain.Abstractions;

public interface IInvitationRepository
{
    Task<Invitation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Invitation?> GetByAccessTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invitation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Invitation invitation, CancellationToken cancellationToken = default);
    void Update(Invitation invitation);
    void Delete(Invitation invitation);
}
