using FluentValidation;

namespace SaveTheDate.Application.Invitations.Commands.UpdateInvitation;

public class UpdateInvitationCommandValidator : AbstractValidator<UpdateInvitationCommand>
{
    public UpdateInvitationCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.GuestId).GreaterThan(0);
    }
}
