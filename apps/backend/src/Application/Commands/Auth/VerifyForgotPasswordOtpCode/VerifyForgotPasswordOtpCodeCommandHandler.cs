using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Auth.OtpCodes;
using Domain.Entities.Auth.Users;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Auth.VerifyForgotPasswordOtpCode;

internal sealed class VerifyForgotPasswordOtpCodeCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider timeProvider
) : ICommandHandler<VerifyForgotPasswordOtpCodeCommand>
{
    public async Task<Result> Handle(
        VerifyForgotPasswordOtpCodeCommand command,
        CancellationToken cancellationToken
    )
    {
        User? user = await context
            .Users.AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (!User.IsValidUser(user, command.Email, out Error? error))
        {
            return Result.Failure(error!);
        }

        OtpCode? otpCode = await context.OtpCodes.SingleOrDefaultAsync(
            o =>
                o.UserId == user!.Id
                && !o.Used
                && !o.Verified
                && o.Type == OtpCodeType.ResetPassword
                && o.ExpiredOnUtc > timeProvider.UtcNow
                && o.Code == command.OtpCode,
            cancellationToken
        );

        if (otpCode is null)
        {
            return Result.Failure(OtpCodeErrors.InvalidOtpCode);
        }

        otpCode.Verified = true;

        context.OtpCodes.Update(otpCode);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
