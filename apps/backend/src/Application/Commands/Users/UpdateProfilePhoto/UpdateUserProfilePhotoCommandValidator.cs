using FluentValidation;

namespace Application.Commands.Users.UpdateProfilePhoto;

internal sealed class UpdateUserProfilePhotoCommandValidator
    : AbstractValidator<UpdateUserProfilePhotoCommand>
{
    public UpdateUserProfilePhotoCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required.");

        RuleFor(x => x.ProfilePhotoUri)
            .NotEmpty()
            .WithMessage("Profile photo is required.")
            .Must(x => x.IsAbsoluteUri)
            .WithMessage("Profile photo must be an absolute URI.");
    }
}
