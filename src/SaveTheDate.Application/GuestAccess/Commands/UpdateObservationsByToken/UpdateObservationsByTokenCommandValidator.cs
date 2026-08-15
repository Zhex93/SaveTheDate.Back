using FluentValidation;

namespace SaveTheDate.Application.GuestAccess.Commands.UpdateObservationsByToken;

public class UpdateObservationsByTokenCommandValidator : AbstractValidator<UpdateObservationsByTokenCommand>
{
    public UpdateObservationsByTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.Observations).MaximumLength(1024);
    }
}
