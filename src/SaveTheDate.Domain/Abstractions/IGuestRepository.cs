using SaveTheDate.Domain.Guests;

namespace SaveTheDate.Domain.Abstractions;

public interface IGuestRepository
{
    Task<Guest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Guest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Guest guest, CancellationToken cancellationToken = default);
    void Update(Guest guest);
    void Delete(Guest guest);
}
