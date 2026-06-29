using FluentValidation;

namespace SaveTheDate.Application.Invitations.Commands.CreateInvitation;

public class CreateInvitationCommandValidator : AbstractValidator<CreateInvitationCommand>
{
    public CreateInvitationCommandValidator()
    {
        RuleFor(x => x.GuestId).GreaterThan(0);
    }
}
