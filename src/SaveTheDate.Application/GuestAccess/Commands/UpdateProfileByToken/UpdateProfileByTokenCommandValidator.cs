using FluentValidation;

namespace SaveTheDate.Application.GuestAccess.Commands.UpdateProfileByToken;

public class UpdateProfileByTokenCommandValidator : AbstractValidator<UpdateProfileByTokenCommand>
{
    public UpdateProfileByTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Phone).NotEmpty();
    }
}
