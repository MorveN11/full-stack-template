using FluentValidation;

namespace Application.Commands.Users.UpdateProfilePhoto;

internal sealed class UpdateUserProfilePhotoCommandValidator
    : AbstractValidator<UpdateUserProfilePhotoCommand>
{
    private readonly HashSet<string> _validImageExtensions = [".png", ".jpg", ".jpeg"];

    public UpdateUserProfilePhotoCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required.");

        RuleFor(x => x.ProfilePhotoUri)
            .NotEmpty()
            .WithMessage("Profile photo is required.")
            .Must(x => x.IsAbsoluteUri)
            .WithMessage("Profile photo must be an absolute URI.")
            .Must(BeValidImageUri)
            .WithMessage(
                " Profile photo must be a valid image file with one of the following extensions: "
                    + string.Join(", ", _validImageExtensions)
            );
    }

    private bool BeValidImageUri(Uri uri)
    {
        string extension = Path.GetExtension(uri.ToString());

        return _validImageExtensions.Contains(extension);
    }
}
