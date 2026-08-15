using FluentValidation;

namespace SaveTheDate.Application.GuestAccess.Commands.UpdateRsvpByToken;

public class UpdateRsvpByTokenCommandValidator : AbstractValidator<UpdateRsvpByTokenCommand>
{
    public UpdateRsvpByTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.State).IsInEnum();
    }
}
