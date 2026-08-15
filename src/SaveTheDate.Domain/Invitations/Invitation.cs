using SaveTheDate.Domain.Common;
using SaveTheDate.Domain.Guests;

namespace SaveTheDate.Domain.Invitations;

public class Invitation : BaseEntity
{
    public int GuestId { get; private set; }
    public InvitationState State { get; private set; }
    public string Observations { get; private set; } = string.Empty;
    public string? AccessTokenHash { get; private set; }
    public DateTime? AccessTokenCreatedAt { get; private set; }
    public Guest Guest { get; private set; } = null!;

    private Invitation() { }

    public Invitation(int guestId, string observations)
    {
        GuestId = guestId;
        State = InvitationState.Pending;
        SetObservations(observations);
    }

    public void SetGuestId(int guestId)
    {
        GuestId = guestId;
    }

    public void SetObservations(string observations)
    {
        Observations = observations ?? string.Empty;
    }

    public void ChangeState(InvitationState newState)
    {
        State = newState;
    }

    public void SetAccessToken(string tokenHash)
    {
        AccessTokenHash = tokenHash;
        AccessTokenCreatedAt = DateTime.UtcNow;
    }

    public void RevokeAccessToken()
    {
        AccessTokenHash = null;
        AccessTokenCreatedAt = null;
    }
}
