using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Auth.OtpCodes;
using Domain.Entities.Auth.Users;
using Domain.Enums;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Auth.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider timeProvider,
    IEmailService emailService
) : ICommandHandler<ForgotPasswordCommand>
{
    public async Task<Result> Handle(
        ForgotPasswordCommand command,
        CancellationToken cancellationToken
    )
    {
        User? user = await context
            .Users.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == command.Email, cancellationToken);

        if (!User.IsValidUser(user, command.Email, out Error? error))
        {
            return Result.Failure(error!);
        }

        string otpCode = OtpCode.GenerateCode();

        var otpCodeEntity = new OtpCode
        {
            UserId = user!.Id,
            Code = otpCode,
            Type = OtpCodeType.ResetPassword,
            CreatedOnUtc = timeProvider.UtcNow,
            ExpiredOnUtc = timeProvider.UtcNow.AddMinutes(15),
        };

        context.OtpCodes.Add(otpCodeEntity);

        await context.SaveChangesAsync(cancellationToken);

        return await User.SendOtpCodeAsync(emailService, command.Email, otpCode, cancellationToken);
    }
}
