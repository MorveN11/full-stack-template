using Domain.Entities.Auth.Roles;
using Domain.Entities.Auth.UserProfiles;
using Domain.Enums;
using Domain.Joins;
using Domain.Services;
using SharedKernel.Domain;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace Domain.Entities.Auth.Users;

public sealed class User : Entity
{
    public required string Email { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PasswordHash { get; set; }
    public bool EmailVerified { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Pending;
    public UserProfile? Profile { get; set; }
    public List<Role> Roles { get; set; } = [];
    public List<UserRole> UserRoles { get; set; } = [];

    public static bool IsValidUser(User? user, string email, out Error? error)
    {
        if (user is null)
        {
            error = UserErrors.NotFound(email);

            return false;
        }

        if (!user.EmailVerified)
        {
            error = UserErrors.EmailNotVerified;

            return false;
        }

        if (user.Status != UserStatus.Active)
        {
            error = UserErrors.Forbidden;

            return false;
        }

        error = Error.None;

        return true;
    }

    public static async Task<Result> SendOtpCodeAsync(
        IEmailService emailService,
        string email,
        string otpCode,
        CancellationToken cancellationToken = default
    )
    {
        string emailTemplate = await File.ReadAllTextAsync(
            "../Domain/EmailTemplates/OtpCodeEmail.html",
            cancellationToken
        );

        emailTemplate = emailTemplate.Replace("{{otpCode}}", otpCode);

        await emailService.SendEmailAsync(
            email,
            $"{otpCode} is your Verification Code",
            emailTemplate,
            isHtml: true
        );

        return Result.Success();
    }
}
