namespace SaveTheDate.Api.Contracts.Invitations;

public record CreateInvitationRequest(int GuestId, string? Observations);
public record UpdateInvitationRequest(int GuestId, string? Observations);
public record ChangeInvitationStateRequest(string State);
