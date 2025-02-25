using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Enums;
using Domain.OtpCodes;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Auth.ResendOtpCode;

internal sealed class ResendOtpCodeCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider timeProvider,
    IEmailService emailService
) : ICommandHandler<ResendOtpCodeCommand>
{
    public async Task<Result> Handle(
        ResendOtpCodeCommand command,
        CancellationToken cancellationToken
    )
    {
        User? user = await context
            .Users.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == command.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.Email));
        }

        if (user.Status != UserStatus.Pending)
        {
            return Result.Failure(UserErrors.EmailAlreadyVerified);
        }

        OtpCodeType otpCodeType = Enum.Parse<OtpCodeType>(command.CodeType);

        if (
            !await context.OtpCodes.AnyAsync(
                x => x.UserId == user.Id && x.Type == otpCodeType && !x.Used && !x.Verified,
                cancellationToken
            )
        )
        {
            return Result.Failure(OtpCodeErrors.NoPreviousOtpCode);
        }

        DateTime lastOtpCodeSentOnUtc = await context
            .OtpCodes.Where(x =>
                x.UserId == user.Id && !x.Used && !x.Verified && x.Type == otpCodeType
            )
            .OrderByDescending(x => x.CreatedOnUtc)
            .Select(x => x.CreatedOnUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastOtpCodeSentOnUtc.AddMinutes(1) > DateTime.UtcNow)
        {
            return Result.Failure(OtpCodeErrors.OtpCodeAlreadySent);
        }

        await context
            .OtpCodes.Where(o =>
                o.UserId == user.Id && !o.Used && !o.Verified && o.Type == otpCodeType
            )
            .ExecuteUpdateAsync(o => o.SetProperty(p => p.Used, true), cancellationToken);

        string otpCode = OtpCode.GenerateCode();

        var otpCodeEntity = new OtpCode
        {
            UserId = user.Id,
            Code = otpCode,
            Type = otpCodeType,
            CreatedOnUtc = timeProvider.UtcNow,
            ExpiredOnUtc = timeProvider.UtcNow.AddMinutes(15),
        };

        context.OtpCodes.Add(otpCodeEntity);

        await context.SaveChangesAsync(cancellationToken);

        string emailTemplate = await File.ReadAllTextAsync(
            "../Domain/EmailTemplates/OtpCodeEmail.html",
            cancellationToken
        );

        emailTemplate = emailTemplate.Replace("{{otpCode}}", otpCode);

        await emailService.SendEmailAsync(
            command.Email,
            $"{otpCode} is your Verification Code",
            emailTemplate,
            isHtml: true
        );

        return Result.Success();
    }
}
