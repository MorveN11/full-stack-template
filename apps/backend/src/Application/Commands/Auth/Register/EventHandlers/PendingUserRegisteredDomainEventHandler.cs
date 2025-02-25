using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Domain.Enums;
using Domain.OtpCodes;
using Domain.Users.DomainEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Time;

namespace Application.Commands.Auth.Register.EventHandlers;

internal sealed class PendingUserRegisteredDomainEventHandler(
    IApplicationDbContext context,
    IEmailService emailService,
    IDateTimeProvider timeProvider
) : INotificationHandler<PendingUserRegisteredDomainEvent>
{
    public async Task Handle(
        PendingUserRegisteredDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        await context
            .OtpCodes.Where(o =>
                o.UserId == notification.UserId
                && !o.Used
                && !o.Verified
                && o.Type == OtpCodeType.Register
            )
            .ExecuteUpdateAsync(o => o.SetProperty(p => p.Used, true), cancellationToken);

        string otpCode = OtpCode.GenerateCode();

        var otpCodeEntity = new OtpCode
        {
            UserId = notification.UserId,
            Code = otpCode,
            Type = OtpCodeType.Register,
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
            notification.UserEmail,
            $"{otpCode} is your Verification Code",
            emailTemplate,
            isHtml: true
        );
    }
}
