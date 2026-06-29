using FluentValidation;

namespace SaveTheDate.Application.Invitations.Commands.ChangeState;

public class ChangeInvitationStateCommandValidator : AbstractValidator<ChangeInvitationStateCommand>
{
    public ChangeInvitationStateCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.State).IsInEnum();
    }
}
