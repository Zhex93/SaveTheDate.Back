using SaveTheDate.Domain.Common;

namespace SaveTheDate.Domain.Invitations;

public class Invitation : BaseEntity
{
    public int GuestId { get; private set; }
    public InvitationState State { get; private set; }
    public string Observations { get; private set; } = string.Empty;

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
}
